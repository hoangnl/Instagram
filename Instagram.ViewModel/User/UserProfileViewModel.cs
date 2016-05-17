using Instagram.ViewModel.Feed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.User
{
    public class UserProfileViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        public string Avartar { get; set; }
        public bool Following { get; set; }
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set;
        }
        [Display(Name = "Thông tin về bạn")]
        public string Bio { get; set; }

        [Display(Name ="Số điện thoại")]
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        [Display(Name = "Giới tính")]
        public int Gender { get; set; }
        public byte[] Timestamp { get; set; }
        public bool AccountDisabled { get; set; }

        public int PostNo { get; set; }
        public int FollowerNo { get; set; }
        public int FollowingNo { get; set; }
        public virtual ICollection<FeedViewModel> Feeds { get; set; }
    }
}
