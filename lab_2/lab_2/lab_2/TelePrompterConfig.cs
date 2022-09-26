using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace lab_2
{
    class TelePrompterConfig
    {
        private object LockHandle { get; } = new object();
        public int DelayInMilliseconds { get; private set; } = 200;
        public bool Done { get; set; } = false;

        public string FilePath { get; private set; }
        public TelePrompterConfig(string filePath)
        {
            FilePath = filePath;
        }
        
        public void UpdateDelay(int increment)
        {
            int newDelay = Max(DelayInMilliseconds + increment, 10);
            newDelay = Min(DelayInMilliseconds + increment, 500);
            lock (LockHandle)
            {
                DelayInMilliseconds = newDelay; 
            }
        }
        public void SetDelay(int delay)
        {
            int newDelay = Max(delay, 10);
            newDelay = Min(delay, 500);
            lock (LockHandle)
            {
                DelayInMilliseconds = newDelay;
            }
        }
    }
}
