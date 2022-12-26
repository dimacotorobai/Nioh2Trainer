using System;
using System.Runtime.InteropServices;

namespace WindowsAPI
{
    //Process Access Flags
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        PROCESS_ALL_ACCESS = 0x000F0000 | 0x00100000 | 0xFFFF,
        PROCESS_CREATE_PROCESS = 0x00000080,
        PROCESS_CREATE_THREAD = 0x00000002,
        PROCESS_DUP_HANDLE = 0x00000040,
        PROCESS_QUERY_INFORMATION = 0x00000400,
        PROCESS_QUERY_LIMITED_INFORMATION = 0x00001000,
        PROCESS_SET_INFORMATION = 0x00000200,
        PROCESS_SET_QUOTA = 0x00000100,
        PROCESS_SUSPEND_RESUME = 0x00000800,
        PROCESS_TERMINATE = 0x00000001,
        PROCESS_VM_OPERATION = 0x00000008,
        PROCESS_VM_READ = 0x00000010,
        PROCESS_VM_WRITE = 0x00000020,
        SYNCHRONIZE = 0x00100000
    }

    //Snapshot Flags
    [Flags]
    public enum SnapshotFlags : uint
    {
        HeapList = 0x00000001,
        Process = 0x00000002,
        Thread = 0x00000004,
        Module = 0x00000008,
        Module32 = 0x00000010,
        Inherit = 0x80000000,
        All = 0x0000001F,
        NoHeaps = 0x40000000
    }

    //Memory Allocation Flags
    [Flags]
    public enum AllocationType
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
        Release = 0x8000,
        Reset = 0x80000,
        Physical = 0x400000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        LargePages = 0x20000000
    }

    //Memory Protection Flags
    [Flags]
    public enum MemoryProtection
    {
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        GuardModifierflag = 0x100,
        NoCacheModifierflag = 0x200,
        WriteCombineModifierflag = 0x400
    }

    //ProcessEntry32 Data Struct
    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESSENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szExeFile;
    };

    //ModuleEntry32 Data Struct
    [StructLayout(LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    public struct MODULEENTRY32
    {
        internal uint dwSize;
        internal uint th32ModuleID;
        internal uint th32ProcessID;
        internal uint GlblcntUsage;
        internal uint ProccntUsage;
        internal IntPtr modBaseAddr;
        internal uint modBaseSize;
        internal IntPtr hModule;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        internal string szModule;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        internal string szExePath;
    }

    public static class Win32
    {
        public const int VK_F1 = 0x70;
        public const int VK_F2 = 0x71;
        public const int VK_F3 = 0x72;
        public const int VK_F4 = 0x73;
        public const int VK_F5 = 0x74;
        public const int VK_F6 = 0x75;
        public const int VK_F7 = 0x76;
        public const int VK_F8 = 0x77;

        /*
         * OpenProcess
        */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

        /*
         * CloseHandle
        */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);

        /*
         * CreateToolhelp32Snapshot
        */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot(SnapshotFlags dwFlags, uint th32ProcessID);

        /*
         * Process32First
        */
        [DllImport("kernel32.dll")]
        public static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        /*
         * Process32Next
        */
        [DllImport("kernel32.dll")]
        public static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        /*
         * Module32First
        */
        [DllImport("kernel32.dll")]
        public static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        /*
         * Module32Next
        */
        [DllImport("kernel32.dll")]
        public static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        /*
         * ReadProcessMemory
        */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesRead);

        /*
         * WriteProcessMemory
        */
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        /*
         * VirtualAllocEx
        */
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        /*
         * VirtualProtectEx
        */
        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

        /*
         * VirtualFreeEx
        */
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType dwFreeType);

        /*
         * FindWindow
        */
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // FOR CONSOLE APPLICATIONS USE:
        [DllImport("user32.dll")]
        public static extern ushort GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
    }
}