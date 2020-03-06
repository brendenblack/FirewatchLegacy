﻿using Blackbox.Firewatch.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.UnitTests
{
    public class CommandTestBase : IDisposable
    {
        public CommandTestBase()
        {
            Context = ApplicationDbContextFactory.Create();
        }

        public ApplicationDbContext Context { get; }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }
}
