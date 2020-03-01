using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Common.Interfaces
{
    public interface IBankContext : IRepository
    {
        DbSet<Person> People { get; }

        DbSet<Account> Accounts { get; }

        DbSet<Transaction> Transactions { get; }
    }
}
