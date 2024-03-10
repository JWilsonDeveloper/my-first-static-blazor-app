using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class CategoryNode
    {
        public string Name { get; set; }
        public string FullCategory { get; set; }
        public List<CategoryNode> Children { get; } = new List<CategoryNode>();
        //public CategoryNode Parent { get; set; }
        public bool IsChecked { get; set; }
    }
}
