using System;
using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command
{
    public class ChatCommandHandler : ChatHandler
    {
        public const char ChatCommandStart = '/';
        public const char ChatCommandSeparator = ' ';

        private readonly Dictionary<string, ChatCommand> _commands;
        private readonly NecServer _server;

        public ChatCommandHandler(NecServer server)
        {
            _server = server;
            _commands = new Dictionary<string, ChatCommand>();
        }

        public void AddCommand(ChatCommand command)
        {
            _commands.Add(command.KeyToLowerInvariant, command);
        }

        public Dictionary<string, ChatCommand> GetCommands()
        {
            return new Dictionary<string, ChatCommand>(_commands);
        }

        public void HandleCommand(NecClient client, string command)
        {
            if (client == null)
            {
                return;
            }

            ChatMessage message = new ChatMessage(ChatMessageType.ChatCommand, client.Character.Name, command);
            List<ChatResponse> responses = new List<ChatResponse>();
            Handle(client, message, new ChatResponse(), responses);
            foreach (ChatResponse response in responses)
            {
                _server.Router.Send(response);
            }
        }

        public override void Handle(NecClient client, ChatMessage message, ChatResponse response,
            List<ChatResponse> responses)
        {
            if (client == null)
            {
                return;
            }

            if (message.Message == null || message.Message.Length <= 1)
            {
                return;
            }

            if (!message.Message.StartsWith(ChatCommandStart))
            {
                return;
            }

            string commandMessage = message.Message.Substring(1);
            string[] command = commandMessage.Split(ChatCommandSeparator);
            if (command.Length <= 0)
            {
                Logger.Error(client, $"Command '{message.Message}' is invalid");
                return;
            }

            string commandKey = command[0].ToLowerInvariant();
            if (!_commands.ContainsKey(commandKey))
            {
                Logger.Error(client, $"Command '{commandKey}' does not exist");
                responses.Add(ChatResponse.CommandError(client, $"Command does not exist: {commandKey}"));
                return;
            }

            ChatCommand chatCommand = _commands[commandKey];
            if (client.Account.State < chatCommand.AccountState)
            {
                Logger.Error(client,
                    $"Not entitled to execute command '{chatCommand.Key}' (State < Required: {client.Account.State} < {chatCommand.AccountState})");
                return;
            }

            int cmdLength = command.Length - 1;
            string[] subCommand;
            if (cmdLength > 0)
            {
                subCommand = new string[cmdLength];
                Array.Copy(command, 1, subCommand, 0, cmdLength);
            }
            else
            {
                subCommand = new string[0];
            }

            chatCommand.Execute(subCommand, client, message, responses);
        }
    }
}
