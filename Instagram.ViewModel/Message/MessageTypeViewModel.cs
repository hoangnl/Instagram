using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class MessageTypeViewModel
    {
        public int MessageTypeId { get; set; }
        public string Name { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
