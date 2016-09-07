using System;

namespace A02.Services.Exceptions
{
    public class AppObjectNotFoundException : Exception
    {
        private readonly string _msg;

        public AppObjectNotFoundException()
        {
            _msg = "App Object Could Not Be Found";
        }

        public string GetMessage()
        {
            return _msg;
        }
    }
}
