using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Expenses.Commands.AddCategories
{
    public class AddCategoriesResponse
    {
        public List<int> CreatedIds { get; set; } = new List<int>();
    }
}