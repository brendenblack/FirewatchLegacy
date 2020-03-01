using Blackbox.Firewatch.Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain
{
    public class Person
    {
        public string Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
