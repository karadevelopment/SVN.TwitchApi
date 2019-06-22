using SVN.Network.Communication.TCP;
using SVN.TwitchApi.Enums;
using SVN.TwitchApi.Properties;
using SVN.TwitchApi.TransferObjects;
using System;

namespace SVN.TwitchApi.Modules
{
    public class ChatBot : Client
    {
        private string Channel { get; }
        private Action<string> Log { get; }

        internal ChatBot(string name, string oauth, string channel, Action<MessageDto> handle, Action<string> log)
        {
            this.Channel = channel;
            this.Log = log;
            this.Start(name, oauth, handle);
        }

        private void Start(string name, string oauth, Action<MessageDto> handle)
        {
            base.OnConnectionHandleMessage = (x, y) =>
            {
                if (y is string z)
                {
                    this.Handle(z, handle);
                }
            };
            base.Start("irc.chat.twitch.tv", 6667);
            base.Send($"PASS {oauth}");
            base.Send($"NICK {name}");
            base.Send($"JOIN #{this.Channel}");
        }

        public void Send(string message)
        {
            this.Log($"sending: {message}");
            base.Send(string.Format(Settings.BotMessageSendFormat, this.Channel, message));
        }

        private void Handle(string message, Action<MessageDto> handle)
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

                handle(new MessageDto
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

                handle(new MessageDto
                {
                    type = MessageType.UserPublic,
                    user = user,
                    message = message,
                });
            }
        }
    }
}