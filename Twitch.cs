using SVN.TwitchApi.ApiTransferObjects;
using SVN.TwitchApi.Modules;
using SVN.TwitchApi.TransferObjects;
using SVN.Web.Request;
using System;
using System.Collections.Generic;

namespace SVN.TwitchApi
{
    public static class Twitch
    {
        public static string GetAuthUri(string clientId, string redirect)
        {
            return $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={clientId}&redirect_uri={redirect}&scope=user:read:email";
        }

        public static AuthResponseDto RequestAuth(string clientId, string clientSecret, string code, string redirect)
        {
            var headers = new Dictionary<string, string>
            {
            };
            var values = new Dictionary<string, string>
            {
               { "client_id", clientId },
               { "client_secret", clientSecret },
               { "code", code },
               { "grant_type", "authorization_code" },
               { "redirect_uri", redirect },
            };
            return Ajax.Post<AuthResponseDto>($"https://id.twitch.tv/oauth2/token", headers, values);
        }

        public static ValidationDto SendAccessToken(string clientId, string accessToken)
        {
            var headers = new Dictionary<string, string>
            {
               { "Authorization", $"OAuth {accessToken}" },
            };
            return Ajax.Get<ValidationDto>($"https://id.twitch.tv/oauth2/validate", headers);
        }

        public static ChannelDto GetChannel(string channel, string clientId)
        {
            var headers = new Dictionary<string, string>
            {
            };
            return Ajax.Get<ChannelDto>($"https://api.twitch.tv/kraken/channels/{channel}?client_id={clientId}", headers);
        }

        public static ChattersDto GetChatters(string channel)
        {
            var headers = new Dictionary<string, string>
            {
            };
            return Ajax.Get<ChattersDto>($"http://tmi.twitch.tv/group/user/{channel}/chatters", headers);
        }

        public static ChatBot CreateChatBot(string channel, Action<MessageDto> handle, Action<string> log)
        {
            return new ChatBot(channel, handle, log);
        }
    }
}