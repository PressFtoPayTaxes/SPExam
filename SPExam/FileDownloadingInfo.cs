using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPExam
{
    public class FileDownloadingInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileUrl { get; set; }
        public string SaveDirectory { get; set; }
    }
}
