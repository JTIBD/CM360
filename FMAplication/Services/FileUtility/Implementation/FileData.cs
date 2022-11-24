using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Services.FileUtility.Implementation
{
    public class FileData
    {
        public byte[] Data { get; set; }
        public string ContentType { get; set; }

        public string Name { get; set; }
    }
}
