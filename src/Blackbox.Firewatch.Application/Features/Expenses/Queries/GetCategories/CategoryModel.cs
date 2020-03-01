using AutoMapper;
using Blackbox.Firewatch.Application.Infrastructure.Mapping;
using Blackbox.Firewatch.Domain.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Expenses.Queries.GetCategories
{
    public class CategoryModel : IMapFrom<Category>
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public Dictionary<int, string> Subcategories { get; set; } = new Dictionary<int, string>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryModel>()
                .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(s => s.Subcategories.ToDictionary(k => k.Id, v => v.Label)));
        }
    }
}
