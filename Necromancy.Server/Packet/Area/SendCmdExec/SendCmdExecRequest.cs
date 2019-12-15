using System.Collections.Generic;
using Necromancy.Server.Chat.Command;

namespace Necromancy.Server.Packet.Area.SendCmdExec
{
    public class SendCmdExecRequest
    {
        public SendCmdExecRequest(string command)
        {
            Command = command;
            Parameter = new List<string>();
        }

        public string Command { get; set; }
        public List<string> Parameter { get; }

        public string CommandString()
        {
            return
                $"{ChatCommandHandler.ChatCommandStart}{Command} {string.Join(ChatCommandHandler.ChatCommandSeparator, Parameter)}";
        }
    }
}
