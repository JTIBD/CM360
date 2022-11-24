using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication
{
    public class ApplicationConst
    {
        public const string AvCommunicationPath = "\\Files\\AvCommunications";
     
        public static readonly string[] ValidVideoFormat = { ".mp4", ".mpeg", ".avi", ".wmv" };
        public static readonly string[] ValidImageFormat = { ".jpg", ".jpeg", ".png" };

        private static readonly string ImageDomain = "https://stormsfsax01drishtisa.blob.core.windows.net";
        public static readonly string ImageBaseUrl = $"{AzureSettings.ConnectionString}/{AzureSettings.ContainerName}";
    }


    public class AzureSettings
    {
        public static string StorageAccountName { get; set; }
        public static string StorageAccountKey { get; set; }
        public static string ContainerName { get; set; }
        public static string ConnectionString { get; set; }

    }


}
