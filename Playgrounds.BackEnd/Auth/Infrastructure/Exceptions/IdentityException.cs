using System;
namespace Auth.Infrastructure.Exceptions
{
    public class MemberCreationException : Exception
    {
        public MemberCreationException(string memberLogin, string memberEmail)
            : base($"A member with login {memberLogin} or email address {memberEmail} already exists.")
        {
        }
    }

    public class MemberLoginException : Exception
    {
        public MemberLoginException() : base($"Wrong login/password.")
        {
        }
    }
}