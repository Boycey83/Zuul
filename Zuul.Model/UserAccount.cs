using System;

namespace Zuul.Model
{
    public class UserAccount
    {
        public UserAccount()
        {
        }

        public UserAccount(string emailAddress, string username, string passwordSalt, string passwordHash, Guid token)
        {
            EmailAddress = emailAddress;
            Username = username;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            Token = token;
        }

        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual Guid Token { get; set; }
        public virtual bool IsActivated { get; set; }
        public virtual Guid? ResetToken { get; set; }
        public virtual DateTime? ResetTokenExpiry { get; set; }
    }
}
