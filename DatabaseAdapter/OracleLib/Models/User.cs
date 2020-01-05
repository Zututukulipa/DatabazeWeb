using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib.Enums;

namespace DatabaseAdapter.OracleLib.Models
{
    public class User : IEquatable<User>
    {

        public int UserId { get; set; }


        public string Username { get; set; }


        public string Password { get; set; }


        public string FirstName { get; set; }


        public string MiddleName { get; set; }


        public string LastName { get; set; }


        public string Email { get; set; }

        public Roles Role { get; set; }
        public int StatusId { get; set; }


        public bool Admin { get; set; }

        public List<Group> Groups { get; set; }
        public string Bio { get; set; }

        public Dictionary<User, List<PrivateMessages>> UserConversations { get; set; }

        public User()
        {
            Username = "MockupBro";
            FirstName = "Ronald";
            MiddleName = "J.";
            LastName = "Mockup";
            Email = "RonaldBro@schoold.gg";
            Groups = new List<Group>();
            StatusId = 1;
            Bio =
                "Generic automatically generated bio, trying to emulate a little string that might be possible to observe in responsive mode :]";
            UserConversations = new Dictionary<User, List<PrivateMessages>>();

        }

        public User(int userId, string username, string firstName, string middleName, string lastName, string email, string bio)
        {
            UserId = userId;
            Username = username;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Email = email;
            Groups = new List<Group>();
            StatusId = 1;
            Bio = bio;
            UserConversations = new Dictionary<User, List<PrivateMessages>>();

        }

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UserId == other.UserId && Username == other.Username && FirstName == other.FirstName && MiddleName == other.MiddleName && LastName == other.LastName && Email == other.Email;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Username != null ? Username.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MiddleName != null ? MiddleName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Email != null ? Email.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
