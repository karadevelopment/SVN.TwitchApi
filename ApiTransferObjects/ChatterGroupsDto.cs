using System.Collections.Generic;

namespace SVN.TwitchApi.ApiTransferObjects
{
    public class ChatterGroupsDto
    {
        public List<string> vips { get; set; }
        public List<string> moderators { get; set; }
        public List<string> staff { get; set; }
        public List<string> admins { get; set; }
        public List<string> global_mods { get; set; }
        public List<string> viewers { get; set; }
    }
}