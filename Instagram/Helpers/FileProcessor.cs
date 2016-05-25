using Instagram.Common;
using Instagram.ViewModel.Feed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Instagram.Helpers
{
    public class FileProcessor : IFileProcessor
    {
        string saveToFolder = "files";
        string stickerFolder = "/Images/Stickers/";
        int fileType = 1;
        public IEnumerable<FileViewModel> ProcessFile(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            switch (fileType)
            {
                case (int)FileType.Photo:
                    saveToFolder = "../files/photos/";
                    break;
                case (int)FileType.Video:
                    saveToFolder = "../files/videos/";
                    break;
                default:
                    break;
            }
            saveToFolder += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + "/";
            var physicalPath = HttpContext.Current.Server.MapPath(saveToFolder);
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }

            List<FileViewModel> uploadFileViewModels = new List<FileViewModel>();
            foreach (var file in uploadFiles)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var uploadFileName = file.FileName.Substring(file.FileName.IndexOf("\\") + 1);
                    var extension = uploadFileName.Substring(uploadFileName.LastIndexOf(".") + 1);
                    Guid guidName = Guid.NewGuid();
                    string fullFileName = physicalPath + "/" + guidName.ToString() + "_O." + extension;
                    bool goodFile = true;
                    FileViewModel fileViewModel = new FileViewModel();
                    switch (fileType)
                    {
                        case (int)FileType.Photo:
                            fileViewModel.FileFolderId = (int)FileType.Photo;
                            switch (extension.ToLower())
                            {
                                case "jpg":
                                    fileViewModel.FileTypeId = (int)FileExtension.JPG;
                                    break;
                                case "gif":
                                    fileViewModel.FileTypeId = (int)FileExtension.GIF;
                                    break;
                                case "jpeg":
                                    fileViewModel.FileTypeId = (int)FileExtension.JPEG;
                                    break;
                                case "png":
                                    fileViewModel.FileTypeId = (int)FileExtension.PNG;
                                    break;
                                case "mp4":
                                    fileViewModel.FileTypeId = (int)FileExtension.MP4;
                                    break;
                                default:
                                    goodFile = false;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }

                    if (goodFile)
                    {
                        file.SaveAs(fullFileName);
                        fileViewModel.Size = file.ContentLength;
                        fileViewModel.RealFileName = file.FileName;
                        fileViewModel.FileName = guidName;
                        uploadFileViewModels.Add(fileViewModel);
                    }
                }

            }
            return uploadFileViewModels;
        }

        public UserViewModel ProcessAvatar(HttpPostedFileBase photo, string userId)
        {
            var user = new UserViewModel();
            saveToFolder = "../files/Avatar/";
            var physicalPath = HttpContext.Current.Server.MapPath(saveToFolder);
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }
            if (photo != null && photo.ContentLength > 0 && photo.ContentLength / 1000 < 5000)
            {
                var uploadFileName = photo.FileName.Substring(photo.FileName.IndexOf("\\") + 1);
                var extension = uploadFileName.Substring(uploadFileName.LastIndexOf(".") + 1);
                string fullFileName = physicalPath + "/" + userId + "_O." + extension;
                bool goodFile = true;
                user.UserId = userId;
                switch (extension.ToLower())
                {
                    case "jpg":
                        user.FileTypeId = (int)FileExtension.JPG;
                        break;
                    case "gif":
                        user.FileTypeId = (int)FileExtension.GIF;
                        break;
                    case "jpeg":
                        user.FileTypeId = (int)FileExtension.JPEG;
                        break;
                    case "png":
                        user.FileTypeId = (int)FileExtension.PNG;
                        break;
                    case "mp4":
                        user.FileTypeId = (int)FileExtension.MP4;
                        break;
                    default:
                        goodFile = false;
                        break;
                }
                if (goodFile)
                {
                    if (File.Exists(fullFileName))
                    {
                        File.Delete(fullFileName);
                    }
                    photo.SaveAs(fullFileName);
                }
            }
            return user;

        }


        public List<string> GetStickerFolderList()
        {

            return Directory.GetDirectories(HttpContext.Current.Server.MapPath(stickerFolder)).ToList();
        }

        public List<string> GetStickerFileList(string folder)
        {
            var filesList = new List<string>();
            var combine = stickerFolder + folder + "/";
            var path = HttpContext.Current.Server.MapPath(combine);
            foreach (var item in Directory.GetFiles(path))
            {
                filesList.Add(combine + Path.GetFileName(item));
            }
            return filesList;
        }
    }
}