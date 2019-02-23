using SVN.TwitchApi.Enums;

namespace SVN.TwitchApi.TransferObjects
{
    public class MessageDto
    {
        public MessageType type { get; set; }
        public string user { get; set; }
        public string message { get; set; }
    }
}