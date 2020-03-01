using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Expenses.Queries.GetCategories
{
    public class GetCategoriesResponse
    {
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
    }
}