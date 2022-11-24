using System;
using System.Linq;

namespace FMAplication.Helpers
{
    public class Utility
    {
        public static string GetRealUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return "";

            try
            {
                var uri = new Uri(url);
                var path = uri.PathAndQuery.Trim('/');
                var key = string.Join("/", path.Split("/").Skip(1));
                var realUrl = $"https://{AzureSettings.StorageAccountName}.blob.core.windows.net/{AzureSettings.ContainerName}/{key}";
                return realUrl;
            }
            catch (Exception e)
            {
                return url;
            }
        }
    }
}