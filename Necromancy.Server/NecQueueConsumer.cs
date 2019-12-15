using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Services;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Arrowgene.Services.Networking.Tcp.Consumer;
using Arrowgene.Services.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    public class NecQueueConsumer : IConsumer
    {
        public const int NoExpectedSize = -1;

        private readonly BlockingCollection<ClientEvent>[] _queues;
        private readonly Thread[] _threads;
        private readonly Dictionary<int, IClientHandler> _clientHandlers;
        private readonly Dictionary<int, IConnectionHandler> _connectionHandlers;
        private readonly Dictionary<ITcpSocket, NecConnection> _connections;
        private readonly NecLogger _logger;
        private readonly object _lock;
        private readonly int _maxUnitOfOrder;
        private ServerType _serverType;
        private NecSetting _setting;
        private volatile bool _isRunning;

        private CancellationTokenSource _cancellationTokenSource;

        public int HandlersCount => _clientHandlers.Count;

        public Action<NecConnection> ClientDisconnected;
        public Action<NecConnection> ClientConnected;
        public Action Started;
        public Action Stopped;

        public NecQueueConsumer(ServerType serverType, NecSetting setting, AsyncEventSettings socketSetting)
        {
            _setting = setting;
            _logger = LogProvider.Logger<NecLogger>(this);
            _maxUnitOfOrder = socketSetting.MaxUnitOfOrder;
            _queues = new BlockingCollection<ClientEvent>[_maxUnitOfOrder];
            _threads = new Thread[_maxUnitOfOrder];
            _lock = new object();
            _clientHandlers = new Dictionary<int, IClientHandler>();
            _connectionHandlers = new Dictionary<int, IConnectionHandler>();
            _connections = new Dictionary<ITcpSocket, NecConnection>();
            _serverType = serverType;
        }

        public void Clear()
        {
            _clientHandlers.Clear();
            _connectionHandlers.Clear();
        }

        public void AddHandler(IClientHandler clientHandler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_clientHandlers.ContainsKey(clientHandler.Id))
                {
                    _clientHandlers[clientHandler.Id] = clientHandler;
                }
                else
                {
                    _clientHandlers.Add(clientHandler.Id, clientHandler);
                }

                return;
            }

            if (_clientHandlers.ContainsKey(clientHandler.Id))
            {
                _logger.Error($"[{_serverType}] ClientHandlerId: {clientHandler.Id} already exists");
            }
            else
            {
                _clientHandlers.Add(clientHandler.Id, clientHandler);
            }
        }

        public void AddHandler(IConnectionHandler connectionHandler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_connectionHandlers.ContainsKey(connectionHandler.Id))
                {
                    _connectionHandlers[connectionHandler.Id] = connectionHandler;
                }
                else
                {
                    _connectionHandlers.Add(connectionHandler.Id, connectionHandler);
                }

                return;
            }

            if (_connectionHandlers.ContainsKey(connectionHandler.Id))
            {
                _logger.Error($"[{_serverType}] ConnectionHandlerId: {connectionHandler.Id} already exists");
            }
            else
            {
                _connectionHandlers.Add(connectionHandler.Id, connectionHandler);
            }
        }

        private void HandleReceived(ITcpSocket socket, byte[] data)
        {
            if (!socket.IsAlive)
            {
                return;
            }

            NecConnection connection;
            lock (_lock)
            {
                if (!_connections.ContainsKey(socket))
                {
                    _logger.Error(socket, $"[{_serverType}] Client does not exist in lookup");
                    return;
                }

                connection = _connections[socket];
            }

            List<NecPacket> packets = connection.Receive(data);
            foreach (NecPacket packet in packets)
            {
                NecClient client = connection.Client;
                if (client != null)
                {
                    HandleReceived_Client(client, packet);
                }
                else
                {
                    HandleReceived_Connection(connection, packet);
                }
            }
        }

        private void HandleReceived_Connection(NecConnection connection, NecPacket packet)
        {
            if (!_connectionHandlers.ContainsKey(packet.Id))
            {
                _logger.LogUnknownIncomingPacket(connection, packet, _serverType);
                return;
            }

            IConnectionHandler connectionHandler = _connectionHandlers[packet.Id];
            if (connectionHandler.ExpectedSize != NoExpectedSize && packet.Data.Size < connectionHandler.ExpectedSize)
            {
                _logger.Error(connection,
                    $"[{_serverType}] Ignoring Packed (Id:{packet.Id}) is smaller ({packet.Data.Size}) than expected ({connectionHandler.ExpectedSize})");
                return;
            }

            _logger.LogIncomingPacket(connection, packet, _serverType);
            packet.Data.SetPositionStart();
            try
            {
                connectionHandler.Handle(connection, packet);
            }
            catch (Exception ex)
            {
                _logger.Exception(connection, ex);
            }
        }

        private void HandleReceived_Client(NecClient client, NecPacket packet)
        {
            if (!_clientHandlers.ContainsKey(packet.Id))
            {
                _logger.LogUnknownIncomingPacket(client, packet, _serverType);
                return;
            }

            IClientHandler clientHandler = _clientHandlers[packet.Id];
            if (clientHandler.ExpectedSize != NoExpectedSize && packet.Data.Size < clientHandler.ExpectedSize)
            {
                _logger.Error(client,
                    $"[{_serverType}] Ignoring Packed (Id:{packet.Id}) is smaller ({packet.Data.Size}) than expected ({clientHandler.ExpectedSize})");
                return;
            }

            _logger.LogIncomingPacket(client, packet, _serverType);
            packet.Data.SetPositionStart();
            try
            {
                clientHandler.Handle(client, packet);
            }
            catch (Exception ex)
            {
                _logger.Exception(client, ex);
            }
        }

        private void HandleDisconnected(ITcpSocket socket)
        {
            NecConnection connection;
            lock (_lock)
            {
                if (!_connections.ContainsKey(socket))
                {
                    _logger.Error(socket, $"[{_serverType}] Disconnected client does not exist in lookup");
                    return;
                }

                connection = _connections[socket];
                _connections.Remove(socket);
                _logger.Debug($"[{_serverType}] Clients Count: {_connections.Count}");
            }

            Action<NecConnection> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(connection);
                }
                catch (Exception ex)
                {
                    _logger.Exception(connection, ex);
                }
            }

            _logger.Info(connection, $"[{_serverType}] Client disconnected");
        }

        private void HandleConnected(ITcpSocket socket)
        {
            NecConnection connection = new NecConnection(socket, new PacketFactory(_setting), _serverType);
            lock (_lock)
            {
                _connections.Add(socket, connection);
                _logger.Debug($"[{_serverType}] Clients Count: {_connections.Count}");
            }

            Action<NecConnection> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(connection);
                }
                catch (Exception ex)
                {
                    _logger.Exception(connection, ex);
                }
            }

            _logger.Info(connection, $"[{_serverType}] Client connected");
        }

        private void Consume(int unitOfOrder)
        {
            while (_isRunning)
            {
                ClientEvent clientEvent;
                try
                {
                    clientEvent = _queues[unitOfOrder].Take(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                switch (clientEvent.ClientEventType)
                {
                    case ClientEventType.ReceivedData:
                        HandleReceived(clientEvent.Socket, clientEvent.Data);
                        break;
                    case ClientEventType.Connected:
                        HandleConnected(clientEvent.Socket);
                        break;
                    case ClientEventType.Disconnected:
                        HandleDisconnected(clientEvent.Socket);
                        break;
                }
            }
        }

        void IConsumer.OnStart()
        {
            if (_isRunning)
            {
                _logger.Error($" [{_serverType}] Consumer already running.");
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;
            for (int i = 0; i < _maxUnitOfOrder; i++)
            {
                int uuo = i;
                _queues[i] = new BlockingCollection<ClientEvent>();
                _threads[i] = new Thread(() => Consume(uuo));
                _threads[i].Name = $"[{_serverType}] Consumer: {i}";
                _logger.Info($"[{_serverType}] Starting Consumer: {i}");
                _threads[i].Start();
            }
        }

        public void OnStarted()
        {
            Action started = Started;
            if (started != null)
            {
                started.Invoke();
            }
        }

        void IConsumer.OnReceivedData(ITcpSocket socket, byte[] data)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.ReceivedData, data));
        }

        void IConsumer.OnClientDisconnected(ITcpSocket socket)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.Disconnected));
        }

        void IConsumer.OnClientConnected(ITcpSocket socket)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.Connected));
        }

        void IConsumer.OnStop()
        {
            if (!_isRunning)
            {
                _logger.Error($" [{_serverType}] Consumer already stopped.");
                return;
            }

            _isRunning = false;
            _cancellationTokenSource.Cancel();
            for (int i = 0; i < _maxUnitOfOrder; i++)
            {
                Thread consumerThread = _threads[i];
                _logger.Info($"[{_serverType}] Shutting Consumer: {i} down...");
                Service.JoinThread(consumerThread, 10000, _logger);
                _logger.Info($"[{_serverType}] Consumer: {i} ended.");
                _threads[i] = null;
            }
        }

        public void OnStopped()
        {
            Action stopped = Stopped;
            if (stopped != null)
            {
                stopped.Invoke();
            }
        }
    }
}
