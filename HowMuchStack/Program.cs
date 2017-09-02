using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace RemainingStackSpace
{
    class Program
    {
        private static ManualResetEvent _event = new ManualResetEvent(false);

        static unsafe void Main(string[] args)
        {

            Console.WriteLine($"Stack space free {RemainingStackSpace}");

            var someVar = stackalloc byte[100000];

            Console.WriteLine($"Stack space free {RemainingStackSpace}");

            var t = new System.Threading.Thread(() => RunThread());
            t.Start();

            _event.WaitOne();
        }


        private static void RunThread()
        {
            int recurseCounter = 5;

            Console.WriteLine("Calling 5 times");

            RecurseFunction(recurseCounter);

            Console.WriteLine($"Exited {RemainingStackSpace}");

            Console.WriteLine("Calling 30 times");

            recurseCounter = 30;
            RecurseFunction(recurseCounter);

            Console.WriteLine($"Exited {RemainingStackSpace}");

            _event.Set();
        }

        private static unsafe void RecurseFunction(int numberOfTimes)
        {
            if (numberOfTimes == 0)
            {
                Console.WriteLine($"Remaining stack space {RemainingStackSpace}");
                return;
            }

            numberOfTimes--;
            var bytes = stackalloc byte[1024 * 100];
            RecurseFunction(numberOfTimes);
        }


        [ThreadStatic]
        private static IntPtr _stackBase;

        public static IntPtr StackBase
        {
            get
            {
                if (_stackBase == IntPtr.Zero)
                {
                    if (Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        _stackBase = GetStackBaseLinux();
                    }
                    else
                    {
                        _stackBase = GetStackBase();
                    }
                }
                return _stackBase;
            }
        }

        private unsafe static IntPtr GetStackBase()
        {
            MEMORY_BASIC_INFORMATION stackInfo = new MEMORY_BASIC_INFORMATION();
            IntPtr currentAddr = IntPtr.Subtract(new IntPtr(&stackInfo), 4096);
            VirtualQuery(currentAddr, ref stackInfo, sizeof(MEMORY_BASIC_INFORMATION));
            return stackInfo.AllocationBase;
        }

        private unsafe static IntPtr GetStackBaseLinux()
        {
            var pthread = pthread_self();
            var result = pthread_getattr_np(pthread, out PThreadAttributes attributes);
            result = pthread_attr_getstack(ref attributes, out IntPtr stackAddress, out IntPtr stackSize);
            return stackAddress;
        }

        public unsafe static long RemainingStackSpace
        {
            get
            {
                byte b = 0;
                IntPtr currentAddr = new IntPtr(&b);
                return currentAddr.ToInt64() - StackBase.ToInt64();
            }
        }

        [DllImport("libpthread.so.0")]
        private static extern IntPtr pthread_self();

        [DllImport("libpthread.so.0")]
        private static extern int pthread_getattr_np(IntPtr pthread, out PThreadAttributes attributes);

        [DllImport("libpthread.so.0")]
        private unsafe static extern int pthread_attr_getstack(ref PThreadAttributes threadId, out IntPtr stackaddr, out IntPtr stacksize);

        [DllImport("kernel32.dll")]
        private static extern int VirtualQuery(IntPtr lpAddress, ref MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

        [StructLayout(LayoutKind.Sequential)]
        private struct PThreadAttributes
        {
            public int __detachstate;
            public int __schedpolicy;
            public int __schedparam;
            public int __inheritsched;
            public int __scope;
            public IntPtr __guardsize;
            public int __stackaddr_set;
            public IntPtr __stackaddr;
            public uint __stacksize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }
    }
}
