using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Expenses.Commands.AddCategories
{
    public class AddCategoryModel
    {
        public int Id { get; set; } = 0;
        public string Label { get; set; }

        public List<AddOrUpdateSubcategoryModel> Subcategories { get; set; } = new List<AddOrUpdateSubcategoryModel>();
    }
}
