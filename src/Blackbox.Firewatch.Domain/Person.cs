using Blackbox.Firewatch.Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain
{
    public class Person
    {
        public string Id { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
