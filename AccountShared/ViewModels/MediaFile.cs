using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountShared.ViewModels
{
    public class MediaFile
    {
        public string FileName { get; set; }
        public Stream Content { get; set; }
        public string ContentType { get; set; }
    }
}
