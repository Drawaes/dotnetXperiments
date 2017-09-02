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
            
            Console.ReadLine();
        }


        private static void RunThread()
        {
            int recurseCounter = 5;

            Console.WriteLine("Calling 10 times");

            RecurseFunction(recurseCounter);

            Console.WriteLine($"Exited {RemainingStackSpace}");

            Console.WriteLine("Calling 20 times");

            recurseCounter = 10;
            RecurseFunction(recurseCounter);

            Console.WriteLine($"Exited {RemainingStackSpace}");

            _event.Set();
        }

        private static unsafe void RecurseFunction(int numberOfTimes)
        {
            if(numberOfTimes == 0)
            {
                Console.WriteLine($"Remaining stack space {RemainingStackSpace}");
                return;
            }

            numberOfTimes--;
            var bytes = stackalloc byte[100000];
            RecurseFunction(numberOfTimes);
        }


        [ThreadStatic]
        private static IntPtr _stackBase;

        public static IntPtr StackBase
        {
            get
            {
                if(_stackBase == IntPtr.Zero)
                {
                    _stackBase = GetStackBase();
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

        public unsafe static long RemainingStackSpace
        {
            get
            {
                byte b = 0;
                IntPtr currentAddr = new IntPtr(&b);
                return currentAddr.ToInt64() - StackBase.ToInt64();
            }
        }


        [DllImport("kernel32.dll")]
        private static extern int VirtualQuery(IntPtr lpAddress, ref MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

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
