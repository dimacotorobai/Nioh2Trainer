using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsAPI;

namespace Nioh2Trainer
{
    static class Constants
    {
        public const uint MAX_BUFFER_DUMP_SIZE = 0x7EFFEFBC;  // ~2GB
    }

    class ProcessManager
    {
        private IntPtr m_ProcessHandle;
        private Process m_Process;

        //Class Properties
        public string ProcessName { get { return m_Process.ProcessName; } }
        public uint ProcessID { get { return (uint)m_Process.Id; } }

        public ProcessManager()
        {
            m_Process = default;
            m_ProcessHandle = default;
        }
        ~ProcessManager()
        {
            if (m_ProcessHandle != IntPtr.Zero)
                DisconnectProcess();
        }

        public bool FindProcess(string processName)
        {
            //Get process with specified process anme
            Process[] pProcesses = Process.GetProcessesByName(processName);
            if (pProcesses.Length <= 0) return false;

            m_Process = pProcesses[0];
            return true;
        }

        public ProcessModule GetModule(string moduleName)
        {
            ProcessModuleCollection modules = m_Process.Modules;
            foreach (ProcessModule module in modules)
            {
                if (moduleName == module.ModuleName) return module;
            }
            return default;
        }

        public bool AttachProcess()
        {
            //Attach handle to process
            m_ProcessHandle = Win32.OpenProcess(
                (uint)ProcessAccessFlags.PROCESS_ALL_ACCESS,
                false,
                (uint)m_Process.Id
                );

            return m_ProcessHandle != IntPtr.Zero;
        }

        public bool DisconnectProcess()
        {
            if (m_ProcessHandle != IntPtr.Zero)
            {
                Win32.CloseHandle(m_ProcessHandle);
                m_ProcessHandle = IntPtr.Zero;
            }
            return m_ProcessHandle == IntPtr.Zero;
        }

        public T ReadMemory<T>(IntPtr address) where T : struct
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            IntPtr memPtr = Marshal.AllocHGlobal(buffer.Length);

            Win32.ReadProcessMemory(m_ProcessHandle, address, buffer, buffer.Length, out _);
            Marshal.Copy(buffer, 0, memPtr, buffer.Length);
            T result = (T)Marshal.PtrToStructure(memPtr, typeof(T));
            Marshal.FreeHGlobal(memPtr);

            return result;
        }

        public bool WriteMemory<T>(IntPtr address, T value) where T : struct
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            IntPtr memPtr = Marshal.AllocHGlobal(buffer.Length);

            Marshal.StructureToPtr(value, memPtr, true);
            Marshal.Copy(memPtr, buffer, 0, buffer.Length);
            Marshal.FreeHGlobal(memPtr);

            return Win32.WriteProcessMemory(m_ProcessHandle, address, buffer, buffer.Length, out _);

        }
        public bool PatchMemory(IntPtr address, byte[] replace, uint oldProtect = 0)
        {
            return Win32.VirtualProtectEx(
                    m_ProcessHandle,
                    address,
                    (uint)replace.Length,
                    (uint)MemoryProtection.ExecuteReadWrite,
                    out oldProtect
                )
                && Win32.WriteProcessMemory(
                    m_ProcessHandle,
                    address,
                    replace,
                    replace.Length,
                    out _
                )
                && Win32.VirtualProtectEx(
                    m_ProcessHandle,
                    address,
                    (uint)replace.Length,
                    oldProtect,
                    out _
                );
        }

        public IntPtr AllocMemory(IntPtr address, uint size)
        {
            return Win32.VirtualAllocEx(
                m_ProcessHandle,
                address,
                size,
                (uint)AllocationType.Commit | (uint)AllocationType.Reserve,
                (uint)MemoryProtection.ExecuteReadWrite
                );
        }

        public bool FreeMemory(IntPtr address)
        {
            return Win32.VirtualFreeEx(
                m_ProcessHandle,
                address,
                0,
                AllocationType.Release
                );
        }

        public byte[] TakeBufferDump(IntPtr address, uint size)
        {
            //Buffer size limit
            if (size > Constants.MAX_BUFFER_DUMP_SIZE) return null;

            //Take buffer dump
            byte[] BufferDump = new byte[size];
            Win32.ReadProcessMemory(m_ProcessHandle, address, BufferDump, BufferDump.Length, out _);
            return BufferDump;
        }

        public IntPtr FindSignature(byte[] bufferDump, byte[] signature, byte[] mask = null)
        {
            //Create Mask
            if (mask == null)
            {
                mask = new byte[signature.Length];
                for (int i = 0; i < signature.Length; i++)
                {
                    mask[i] = 1;
                }
            }
            //Search for pattern
            for (int i = 0; i < bufferDump.Length; i++)
            {
                int counter = 0;
                for (int j = 0; j < signature.Length; j++)
                {
                    if (mask[j] == 0 || signature[j] == bufferDump[i + j])
                    {
                        counter++;
                        continue;
                    }
                    break;
                }

                // Check if AoB is found in Buffer Dump
                if (counter == signature.Length) return new IntPtr(i);
            }

            return IntPtr.Zero;
        }
    }
}