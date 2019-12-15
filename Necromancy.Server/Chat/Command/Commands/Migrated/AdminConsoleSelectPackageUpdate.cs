using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class AdminConsoleSelectPackageUpdate : ServerChatCommand
    {
        public AdminConsoleSelectPackageUpdate(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            int errcode = 0;
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(errcode); //Error message Call. 0 for success. see additional options in Sys_msg.csv
            /*
            1	You have unopened mails	SYSTEM_WARNING
            2	No mails to delete	SYSTEM_WARNING
            3	You have %d unreceived mails. Please check your inbox.	SYSTEM_WARNING
            -2414	Mail title includes banned words	SYSTEM_WARNING

            */

            Router.Send(client, (ushort) AreaPacketId.recv_select_package_update_r, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "GetMail";
    }
}
