using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace BioEngine.Common.Ipb
{
    public class IpbUserInfo
    {
        public string Id { get; }
        public string UserName { get; }
        public string ProfileUrl { get; }
        public string AvatarUrl { get; }

        public bool IsParsed = true;

        public IpbUserInfo(string ipbResponse, ILogger ipbLogger)
        {
            if (string.IsNullOrEmpty(ipbResponse))
            {
                IsParsed = false;
                return;
            }
            JObject user;
            try
            {
                user = JObject.Parse(ipbResponse);
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb response: {ex.Message}");
                IsParsed = false;
                return;
            }
            try
            {
                Id = user.Value<string>("id");
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb id: {ex.Message}");
                IsParsed = false;
                return;
            }

            try
            {
                UserName = user.Value<string>("displayName");
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb id: {ex.Message}");
                IsParsed = false;
                return;
            }


            try
            {
                ProfileUrl = user.Value<string>("profileUrl");
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb profileUrl: {ex.Message}");
                IsParsed = false;
                return;
            }

            try
            {
                AvatarUrl = user.Value<string>("avatar");
            }
            catch (Exception)
            {
                //second formats
                Dictionary<string, string> avatarData;
                try
                {
                    avatarData = user["avatar"]["data"].ToObject<Dictionary<string, string>>();
                }
                catch (Exception exData)
                {
                    ipbLogger.LogError($"Error while parsing ipb avatarUrl: {exData.Message}");
                    IsParsed = false;
                    return;
                }
                if (avatarData.Any())
                {
                    AvatarUrl = $"{avatarData["scheme"]}/{avatarData["host"]}/{avatarData["path"]}";
                }
                else
                {
                    ipbLogger.LogError($"Empty avatar data");
                    IsParsed = false;
                }
            }
        }
    }
}