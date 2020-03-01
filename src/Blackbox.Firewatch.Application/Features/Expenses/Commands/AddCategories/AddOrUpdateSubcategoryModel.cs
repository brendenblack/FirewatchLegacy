using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Expenses.Commands.AddCategories
{
    public class AddOrUpdateSubcategoryModel
    {
        public int Id { get; set; } = 0;

        public string Label { get; set; }
    }
}
