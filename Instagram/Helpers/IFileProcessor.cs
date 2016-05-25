using Instagram.ViewModel.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instagram.Helpers
{
    public interface IFileProcessor
    {
        IEnumerable<FileViewModel>  ProcessFile(IEnumerable<HttpPostedFileBase> photo);

        UserViewModel ProcessAvatar(HttpPostedFileBase photo, string userId);

        List<string> GetStickerFolderList();

        List<string> GetStickerFileList(string folder);
    }
}