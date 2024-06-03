using MrG.AI.Client.Data.Action;
using MrG.AI.Client.Enum;
using MrG.AI.Client.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MrG.AI.Client.Data
{
    public class Server : ObservableObject
    {
        
        public static Server Instance { get; set; }

        public ServerType ServerType { get; private set; }
        public string Name { get; private set; }
        public string Ip { get; private set; }
        public string Token { get; private set; }
        
        public ObservableCollection<ActionApi> Actions = new ObservableCollection<ActionApi>();



        public Server(LocalServerEventArgs args): this(ServerType.Local, args.ServerName, args.ServerAddress)
        {

        }

        public Server(ServerType serverType, string name, string details)
        {
            ServerType = ServerType;
            Name = name;
            if (serverType == ServerType.Local)
            {
                Ip = details;
            }
            if(serverType == ServerType.Online)
            {
                Token = details;
            }
           
           
        }

        
        public void SetActions(List<ActionApi> actions)
        {
           
            actions.ForEach(action =>
            {
                if (!Actions.Contains(action))
                {
                    Actions.Add(action);
                }
            });
        }

        public string GetTestUrl()
        {
            return GetTestUrl(Ip);
        }

        public static string GetTestUrl(string ip)
        {
            return $"http://{ip}:8188/test";
        }

        public string GetSocketUrl()
        {
            if (ServerType == ServerType.Local)
            {
                return $"ws://{Ip}:8188/mrg_ws";
            }
            if (ServerType == ServerType.Online)
            {
                return $"wss://socket.mrg.com";
            }
            return null;
        }


        public static string GetMainUrl()
        {
            return "https://www.microsoft.com";
        }

        public static string GetRegisterUrl()
        {
            return "https://www.microsoft.com";
        }

        public static string GetHowToUrl()
        {
            return "https://www.microsoft.com";
        }

        public static string GetPrivacyUrl()
        {
            return "https://www.microsoft.com";
        }

        public static string GetTermsUrl()
        {
            return "https://www.microsoft.com";
        }

        public static string GetAboutUrl()
        {
            return "https://www.microsoft.com";
        }

        public static string GetContactUrl()
        {
            return "https://www.microsoft.com";
        }

        internal static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            return Regex.IsMatch(email, pattern);
        }


        public static bool IsValidIpAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return false;
            string pattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

            return Regex.IsMatch(ipAddress, pattern);
        }

        public static async Task<string> MakeHttpRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

    }

}
