using Blackbox.Firewatch.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;

namespace Blackbox.Firewatch.Application.Settings
{
    public class UserSettings
    {
        private UserSettings() { }

        public UserSettings(Person user)
        {
            User = user;
        }

        public Person User { get; private set; }

        public Currency DefaultCurrency { get; set; }
    }
}
