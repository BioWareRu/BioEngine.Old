using System;

namespace BioEngine.Common.Base
{
    public class UserException : Exception
    {
        public UserException(string message) : base(message)
        {
        }
    }
}