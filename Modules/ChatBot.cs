using SVN.Network.Communication.TCP;
using SVN.TwitchApi.Enums;
using SVN.TwitchApi.Properties;
using SVN.TwitchApi.TransferObjects;
using System;

namespace SVN.TwitchApi.Modules
{
    public class ChatBot : Client
    {
        private string Name { get; }
        private string OAuth { get; }
        private string Channel { get; }
        private Action<MessageDto> Handler { get; }
        private Action<string> Log { get; }

        internal ChatBot(string name, string oauth, string channel, Action<MessageDto> handler, Action<string> log)
        {
            this.Name = name;
            this.OAuth = oauth;
            this.Channel = channel;
            this.Handler = handler;
            this.Log = log;
        }

        public void Start()
        {
            base.OnConnectionHandleMessage = (id, message) =>
            {
                if (message is string message2)
                {
                    this.Handle(message2);
                }
            };
            base.Start("irc.chat.twitch.tv", 6667);
            base.Send($"PASS {this.OAuth}");
            base.Send($"NICK {this.Name}");
            base.Send($"JOIN #{this.Channel}");
        }

        public void Send(string message)
        {
            this.Log($"sending: {message}");
            base.Send(string.Format(Settings.BotMessageSendFormat, this.Channel, message));
        }

        private void Handle(string message)
        {
            this.Log($"receiving: {message}");

            if (message == Settings.BotPing)
            {
                base.Send(Settings.BotPong);
            }
            else if (
                message.StartsWith(Settings.BotMessageReceiveSystemStart)
                || message.StartsWith(Settings.BotMessageReceiveJoinFormat1Start)
                || message.StartsWith(Settings.BotMessageReceiveJoinFormat2Start)
                )
            {
                message = message.Substring(message.IndexOf(Settings.BotMessageReceiveFormatStart) + Settings.BotMessageReceiveFormatStart.Length);
                message = message.Substring(message.IndexOf(Settings.BotMessageReceiveFormatStart) + Settings.BotMessageReceiveFormatStart.Length);

                this.Handler(new MessageDto
                {
                    type = MessageType.System,
                    user = "Twitch",
                    message = message,
                });
            }
            else if (message.Contains(Settings.BotMessageReceiveFormatStart))
            {
                message = message.Substring(message.IndexOf(Settings.BotMessageReceiveFormatStart) + Settings.BotMessageReceiveFormatStart.Length);
                var user = message.Remove(message.IndexOf('!'));
                message = message.Substring(message.IndexOf(Settings.BotMessageReceiveFormatStart) + Settings.BotMessageReceiveFormatStart.Length);

                this.Handler(new MessageDto
                {
                    type = MessageType.UserPublic,
                    user = user,
                    message = message,
                });
            }
        }
    }
}