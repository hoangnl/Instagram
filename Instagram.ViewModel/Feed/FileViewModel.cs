using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class FileViewModel
    {
        public long FileId { get; set; }
        public System.Guid FileName { get; set; }
        public int FileFolderId { get; set; }
        public string RealFileName { get; set; }
        public int FileTypeId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long FeedId { get; set; }
        public byte[] Timestamp { get; set; }
        public long Size { get; set; }
        public string PhotoLink { get; set; }

        //public virtual FeedViewModel Feed { get; set; }
        //public virtual FileFolderViewModel FileFolder { get; set; }
        public virtual FileTypeViewModel FileType { get; set; }
    }
}
