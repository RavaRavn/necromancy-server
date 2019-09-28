﻿using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_wanted_jail_draw_point : Handler
    {
        public send_wanted_jail_draw_point(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_wanted_jail_draw_point;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);//bool
            Router.Send(client.Map, (ushort) AreaPacketId.recv_wanted_jail_draw_point_r, res);
        }

    }
}