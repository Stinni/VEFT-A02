using System;

namespace A02.Services.Exceptions
{
    public class AppObjectExistsException : Exception
    {
        private readonly string _msg;

        public AppObjectExistsException()
        {
            _msg = "App Object Already Exists In The Database";
        }

        public string GetMessage()
        {
            return _msg;
        }
    }
}
