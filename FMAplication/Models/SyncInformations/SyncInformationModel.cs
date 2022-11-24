using System;

namespace FMAplication.Models.SyncInformations
{
    public class SyncInformationModel
    {
        public int Id { get; set; }
        public DateTime LastSyncTime { get; set; }
        public string LastSyncTimeStr { get; set; }
    }
}
