﻿namespace ExtradosStore.Common.CustomExceptions.UserExceptions
{
    public class PhoneNumberAlreadyExistsException : Exception
    {

        public PhoneNumberAlreadyExistsException() : base("The phone number is already in use.")
        {
        }

        public PhoneNumberAlreadyExistsException(string message)
            : base(message)
        {

        }
    }

}