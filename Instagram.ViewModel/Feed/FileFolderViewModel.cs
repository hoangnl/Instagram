using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class FileFolderViewModel
    {
        public int FileFolderId { get; set; }
        public string Path { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
