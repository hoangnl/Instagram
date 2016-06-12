using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Common
{
    public enum GenderEnum
    {
        [Display(Name = "Nam")]
        Male = 1,
        [Display(Name = "Nữ")]
        Female = 0
    }
    public enum FileType
    {
        Photo = 1,
        Video = 2
    }

    public enum FileExtension
    {
        JPG = 1,
        GIF = 2,
        JPEG = 3,
        PNG = 4,
        MP4 = 5
    }

    public enum MessageStatusTypes
    {
        Unread = 1,
        Read = 2
    }

    public enum MessageTypes
    {
        Message = 1
    }
}
