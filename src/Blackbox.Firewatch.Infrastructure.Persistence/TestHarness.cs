using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Persistence
{
    /// <summary>
    /// Provides access to data used in creating test users for IdentityServer.
    /// </summary>
    /// <remarks>
    /// This implementation mixes concerns greatly, but it seemed the cleanest way to ensure the test
    /// project and the values established in AddPersistenceAndIdentity#AddPersistenceAndIdentity stay 
    /// in sync with compiler support.
    /// </remarks>
    public static class TestHarness
    {
        public static TestUserModel StandardUser1 { get; } = new TestUserModel("f26da293-02fb-4c90-be75-e4aa51e0bb17", "standarduser1@blackbox", "password");
        public static TestUserModel StandardUser2 { get; } = new TestUserModel("f26da293-02fb-4c90-be75-e4aa51e0bc17", "standarduser2@blackbox", "password");

    }

    public class TestUserModel
    {
        public TestUserModel(string id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public string Id { get; }
        public string Username { get; }
        public string Password { get; }
    }

}
