using System;

namespace MoviesMobileApp.Services
{
    public class NetworkProblemException : Exception
    {
        public NetworkProblemException(string message)
            : base(message)
        {
        }

        public NetworkProblemException(Exception innerException)
            : base("Error accessing remote resource", innerException)
        {
        }
    }
}