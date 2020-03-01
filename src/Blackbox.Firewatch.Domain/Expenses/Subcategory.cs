using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain.Expenses
{
    public class Subcategory
    {
        private Subcategory() { }

        public Subcategory(Category parent, string label)
        {
            Id = -1;
            ParentCategory = parent;
            Label = label;
        }

        public int Id { get; private set; }

        public int ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }

        public string Label { get; set; }

        public bool IsSystemDefault { get; set; } = false;
    }
}
