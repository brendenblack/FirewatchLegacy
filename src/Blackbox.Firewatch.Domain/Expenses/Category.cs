using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Firewatch.Domain.Expenses
{
    public class Category
    {
        public int Id { get; set; }
        
        public string Label { get; set; }

        public bool IsSystemDefault { get; set; } = false;

        public virtual ICollection<Subcategory> Subcategories { get; } = new List<Subcategory>();

        public string CreatorId { get; set; }
        public DateTime CreatedOn { get; set; }

        public Subcategory AddSubcategory(string label)
        {
            var existing = this.Subcategories
                .FirstOrDefault(s => s.Label.Equals(label, StringComparison.OrdinalIgnoreCase));

            if (existing == null)
            {
                var subcategory = new Subcategory(this, label);
                this.Subcategories.Add(subcategory);
                return subcategory;
            }
            else
            {
                return existing;
            }
        }
    }
}
