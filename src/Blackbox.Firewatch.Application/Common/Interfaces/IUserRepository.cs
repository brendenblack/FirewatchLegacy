using Blackbox.Firewatch.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Common.Interfaces
{
    public interface IUserRepository : IRepository
    {
        public DbSet<Person> People { get; }
    }
}
