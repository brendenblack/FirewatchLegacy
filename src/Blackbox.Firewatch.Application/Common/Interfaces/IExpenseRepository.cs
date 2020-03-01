using Blackbox.Firewatch.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Common.Interfaces
{
    public interface IExpenseRepository : IRepository
    {
        //DbSet<Expense> Expenses { get; }

        DbSet<Category> ExpenseCategories { get; }
    }
}
