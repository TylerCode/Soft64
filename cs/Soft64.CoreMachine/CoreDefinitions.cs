using System;

namespace Soft64
{
    public interface ISoft64
    {
        void Start();
        void Stop();
    }

    public class N64Machine : ISoft64
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}