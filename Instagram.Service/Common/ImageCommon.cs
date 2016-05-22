
using Instagram.Model.EDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Service.Common
{
    public class ImageCommon
    {
        public static string GetAvatarLink(string userId, int? fileTypeId, FileType fileType)
        {
            string avatarLink;
            if (fileTypeId == null)
            {
                avatarLink = String.Format("~/Images/Avatar/avatar-default.png");
            }
            else
            {
                avatarLink = String.Format("~/files/Avatar/{0}", userId + "_O." + fileType.Name);
            }
            return avatarLink;

        }
    }
}
