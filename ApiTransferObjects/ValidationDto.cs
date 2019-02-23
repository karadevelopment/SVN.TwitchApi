using System.Collections.Generic;

namespace SVN.TwitchApi.ApiTransferObjects
{
    public class ValidationDto
    {
        public string client_id { get; set; }
        public string login { get; set; }
        public List<string> scopes { get; set; }
        public string user_id { get; set; }
    }
}