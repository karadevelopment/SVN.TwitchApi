using System.Collections.Generic;

namespace SVN.TwitchApi.ApiTransferObjects
{
    public class AuthResponseDto
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public List<string> scope { get; set; }
        public string token_type { get; set; }
    }
}