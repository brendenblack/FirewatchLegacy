using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Infrastructure.Mapping
{
    public interface IMapFrom<T>
    {
        // https://github.com/JasonGT/NorthwindTraders/tree/master/Src/Application/Common/Mappings
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
