using System;
using System.Net;

namespace Extensions
{
    public static class VufocExtensions
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
