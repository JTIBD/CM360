using System;
using System.Collections.Generic;
using System.Linq;
using FMAplication.Enumerations;
using FMAplication.MobileModels.DailyTasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace FMAplication.RequestModels
{
    public class UploadPosmTaskRequest
    {
        public IFormFile ExistingImageFile { get; set; }
        public IFormFile NewImageFile { get; set; }
        public List<IFormFile> Files { get; set; }
        
        public string Model { get; set; }
        public int DailyTaskId { get; set; }
        public TaskInCompleteReason Reason { get; set; }
        public string ReasonDetails { get; set; }

        public TaskStatus TaskStatus { get; set; }
        public List<int> Quantities{ get; set; }
        public List<string> PosmWorkTypies { get; set; }
    }
}
