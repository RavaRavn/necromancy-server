using System;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_logout_start_request : ClientHandler
    {
        public send_logout_start_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_logout_start_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int CastingTime = 10;
            client.Character.logoutCanceled = 0;

            IBuffer res = BufferProvider.Provide();
            IBuffer res2 = BufferProvider.Provide();

            res.WriteInt32(0); //0 = nothing happens, 1 = logout failed:1
            Router.Send(client, (ushort) AreaPacketId.recv_logout_start_request_r, res, ServerType.Area);


            res2.WriteInt32(10);

            Router.Send(client, (ushort) AreaPacketId.recv_logout_start, res2, ServerType.Area);

            Task.Delay(TimeSpan.FromMilliseconds((int) (CastingTime * 1000)))
                .ContinueWith(t1 => { LogOutRequest(client, packet); });
        }

        private void LogOutRequest(NecClient client, NecPacket packet)
        {
            byte logOutType = packet.Data.ReadByte();
            byte x = packet.Data.ReadByte();

            IBuffer res = BufferProvider.Provide();
            IBuffer res2 = BufferProvider.Provide();
            IBuffer res3 = BufferProvider.Provide();
            IBuffer res4 = BufferProvider.Provide();
            IBuffer res5 = BufferProvider.Provide();

            if (client.Character.logoutCanceled == 0)
            {
                if (logOutType == 0x00)
                {
                    res.WriteInt32(0);

                    Router.Send(client, (ushort) 0xD68C, res2, ServerType.Area);
                }

                if (logOutType == 0x01)
                {
                    byte[] byteArr = new byte[8] {0x00, 0x06, 0xEE, 0x91, 0, 0, 0, 0};


                    res3.WriteInt32(0);

                    res3.SetPositionStart();

                    for (int i = 4; i < 8; i++)
                    {
                        byteArr[i] += res3.ReadByte();
                    }

                    // TODO use packet format 
                    //  client.MsgConnection.Send(byteArr);

                    Thread.Sleep(4100);

                    byte[] byteArrr = new byte[9] {0x00, 0x07, 0x52, 0x56, 0, 0, 0, 0, 0};

                    res4.WriteInt32(0);
                    res4.WriteByte(0);

                    res4.SetPositionStart();

                    for (int i = 4; i < 9; i++)
                    {
                        byteArrr[i] += res4.ReadByte();
                    }

                    // TODO use packet format 
                    //  client.MsgConnection.Send(byteArr);
                }

                if (logOutType == 0x02)
                {
                    byte[] byteArr = new byte[8] {0x00, 0x06, 0xC9, 0x01, 0, 0, 0, 0};

                    res2.WriteInt32(0);

                    res2.SetPositionStart();

                    for (int i = 4; i < 8; i++)
                    {
                        byteArr[i] += res2.ReadByte();
                    }

                    // TODO use packet format 
                    //  client.MsgConnection.Send(byteArr);
                }
            }
        }
    }
}
