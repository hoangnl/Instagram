using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class UserViewModel
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Avartar { get; set; }
        public bool Following { get; set; }
        public string UserName { get; set; }
        public Nullable<int> FileTypeId { get; set; }
        public virtual ICollection<FeedViewModel> Feeds { get; set; }
    }
}
