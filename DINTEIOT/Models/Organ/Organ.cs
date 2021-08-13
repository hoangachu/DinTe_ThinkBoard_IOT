using System;
using System.Collections.Generic;

namespace DINTEIOT.Models
{
    public class Organ
    {
        public int organid { get; set; }
        public string organname { get; set; }
        public string organcode { get; set; }
        public int organparentid { get; set; }
        public string organparentname { get; set; }
        public DateTime createdate { get; set; }
        public int spokesmanid { get; set; }
        public string address { get; set; }
        public int phonenumber { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string url { get; set; }
        public int controlhierarchyid { get; set; }
        public List<Models.Organ> ListChild { get; set; }
        public long rownumber { get; set; }
    }
    public class ControlHierachy
    {
        public int controlhierarchyid { get; set; }
        public string controlhierarchyname { get; set; }
    }
 
    public class GridTree
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Name { get; set; }
        public bool ckIsForArticle { get; set; }
    }
}
