using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DINTEIOT.Models
{
    public class TreeViewNode
    {

        public int id { get; set; }
        public int? parentid { get; set; }
        public List<TreeViewNode> subs { get; set; }
        public string title { get; set; }
        public string displaynumber { get; set; }
        public string url { get; set; } // đường dẫn 
        public bool ckshowinhome { get; set; } // checkbox có hiển thị ngoài giao diện người dung ko

    }
}
