using System;
using System.Net;

namespace Unity_Free_Online_Config.Runtime.Scripts.Extensions
{
    public static class SpreadsheetExtensions
    {
        public static string LoadSpreadsheet(string url)
        {
            var client = new WebClient();
            var result = client.DownloadString(url);
        
            return result;
        }

        public static void LoadSpreadsheetAsync(string url, Action<string> callback)
        {
            var client = new WebClient();
            
            client.DownloadStringTaskAsync(url);
            client.DownloadStringCompleted += (sender, args) =>
            {
                var results = args.Result;
            
                callback?.Invoke(results);
            };
        }
    }
}
