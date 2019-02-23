namespace SVN.TwitchApi.ApiTransferObjects
{
    public class ChattersDto
    {
        public LinksDto _links { get; set; }
        public int chatter_count { get; set; }
        public ChatterGroupsDto chatters { get; set; }
    }
}