using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkFileUpload.Classes
{
    public class FileDisplayData
    {
        public string FileName { get; set; }
        public string IconType { get; set; }

        public string ToolTip { get; set; }

        public FileDisplayData(string fileName, string icontype, string tooltip)
        {
            FileName = fileName;
            IconType = icontype;
            ToolTip = tooltip;
        }
    }
}
