using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class MessageWrapperViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
