using System;

namespace MoviesMobileApp.Utils
{
    internal class ReentryMonitor : IDisposable
    {
        private int referenceCount;

        public bool IsNotifying => referenceCount != 0;

        public IDisposable Enter()
        {
            ++referenceCount;

            return this;
        }

        public void Dispose()
        {
            --referenceCount;
        }
    }
}