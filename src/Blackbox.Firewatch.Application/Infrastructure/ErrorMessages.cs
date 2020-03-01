using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Infrastructure
{
    public static class ErrorMessages
    {
        [Obsolete("A person id is a string.")]
        public static string PERSON_NOT_EXISTS(int id = 0)
        {
            return PERSON_NOT_EXISTS(id.ToString());            
        }

        public static string PERSON_NOT_EXISTS(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return "The requested user does not exist.";
            }
            else
            {
                return $"The requested user with id {id} does not exist.";
            }

        }
    }
}
