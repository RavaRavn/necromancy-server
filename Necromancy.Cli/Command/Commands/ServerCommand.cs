using System;
using Necromancy.Cli.Argument;
using Necromancy.Server;
using Necromancy.Server.Setting;

namespace Necromancy.Cli.Command.Commands
{
    public class ServerCommand : ConsoleCommand
    {
        private NecServer _server;
        private readonly LogWriter _logWriter;

        public ServerCommand(LogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        public override void Shutdown()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (_server == null)
            {
                NecSetting setting = new NecSetting();
                _server = new NecServer(setting);
            }

            if (parameter.Arguments.Contains("start"))
            {
                _server.Start();
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Contains("stop"))
            {
                _server.Stop();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public override string Key => "server";


        public override string Description =>
            $"Wizardry Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";
    }
}
