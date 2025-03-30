using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkFileUpload.Classes
{
    public class FileData
    {
        public string FileName { get; set; }
        public byte[] Hash { get; set; }

        public FileData(string fileName, byte[] hash)
        {
            FileName = fileName;
            Hash = hash;
        }

    }
}
