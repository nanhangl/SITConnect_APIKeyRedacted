using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITConnect
{
    public class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CreditCard { get; set; }
        public DateTime? DateTimeEmailVerified { get; set; }
        public DateTime DateTimeRegistered { get; set; }
        public DateTime PasswordLastChanged { get; set; }
        public int IncorrectPasswordAttempts { get; set; }
        public DateTime? LockoutDateTime { get; set; }

        public Account()
        {

        }

        public Account (int id, string firstname, string lastname, string email, string passwordhash, string passwordsalt, DateTime dob, string creditcard, DateTime? datetimeemailverified, DateTime datetimeregistered, DateTime passwordlastchanged, int incorrectpassword, DateTime? lockoutdatetime)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            PasswordHash = passwordhash;
            PasswordSalt = passwordsalt;
            DateOfBirth = dob;
            CreditCard = creditcard;
            DateTimeEmailVerified = datetimeemailverified;
            DateTimeRegistered = datetimeregistered;
            PasswordLastChanged = passwordlastchanged;
            IncorrectPasswordAttempts = incorrectpassword;
            LockoutDateTime = lockoutdatetime;
        }
    }
}