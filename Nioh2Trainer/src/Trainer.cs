using System;
using System.Diagnostics;

namespace Nioh2Trainer
{
    class Trainer
    {
        //Process members
        private ProcessManager m_ProcessManager;
        private ProcessModule m_Module;
        private byte[] m_BufferDump;

        //Memory addresses
        private IntPtr m_LocalPlayerFunc;
        private IntPtr m_HealthFunc;
        private IntPtr m_StaminaFunc;
        private IntPtr m_AnimaFunc;
        private IntPtr m_YonkaiChargeFunc;
        private IntPtr m_YonkaiCooldownFunc;
        private IntPtr m_YonkaiTimeFunc;

        //Virtual Alloc Variables
        private IntPtr m_VirtualAllocExBase;
        private IntPtr m_LocalPlayerAddr;
        private IntPtr m_GodmodeAddr;
        private IntPtr m_DamageAddr;
        private IntPtr m_StaminaAddr;
        private IntPtr m_AnimaAddr;
        private IntPtr m_YonkaiChargeAddr;
        private IntPtr m_YonkaiCooldownAddr;
        private IntPtr m_YonkaiTimeAddr;

        //Virtual Alloc Hooks
        private IntPtr m_LocalPlayerHook;
        private IntPtr m_HealthHook;
        private IntPtr m_StaminaHook;
        private IntPtr m_AnimaHook;
        private IntPtr m_YonkaiChargeHook;
        private IntPtr m_YonkaiCooldownHook;
        private IntPtr m_YonkaiTimeHook;

        // Class Properties
        public string ProcessName
        {
            get { return (m_ProcessManager != null) ? m_ProcessManager.ProcessName : string.Empty; }
        }
        public uint ProcessID
        {
            get { return (m_ProcessManager != null) ? m_ProcessManager.ProcessID : 0; }
        }
        public IntPtr ModuleBase
        {
            get { return (m_ProcessManager != null) ? m_ProcessManager.GetModule(m_ProcessManager.ProcessName + ".exe").BaseAddress : IntPtr.Zero; }
        }

        public bool GODMODE
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_GodmodeAddr, 0) :
                    m_ProcessManager.WriteMemory<int>(m_GodmodeAddr, 1);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_GodmodeAddr) != 1;
            }
        }
        public bool DAMAGE
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_DamageAddr, 10000) :
                    m_ProcessManager.WriteMemory<int>(m_DamageAddr, 1);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_DamageAddr) != 1;
            }
        }
        public bool INFINITE_STAMINA
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_StaminaAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_StaminaAddr, 0);
            }
            get
            {
                if (m_ProcessManager.ReadMemory<int>(m_StaminaAddr) != 0)
                    return true;

                return false;
            }
        }
        public bool INFINITE_ANIMA
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_AnimaAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_AnimaAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_AnimaAddr) != 0;
            }
        }
        public bool INFINITE_YONKAI_CHARGE
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_YonkaiChargeAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_YonkaiChargeAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_YonkaiChargeAddr) != 0;
            }
        }
        public bool INFINITE_YONKAI_COOLDOWN
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_YonkaiCooldownAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_YonkaiCooldownAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_YonkaiCooldownAddr) != 0;
            }
        }
        public bool INFINITE_YONKAI_TIME
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_YonkaiTimeAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_YonkaiTimeAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_YonkaiTimeAddr) != 0;
            }
        }

        public Trainer()
        {
            m_ProcessManager = new ProcessManager();

            //Find Process and get Process ID
            if (!m_ProcessManager.FindProcess("nioh2"))
                throw new Exception("Error - Could not find \"nioh2.exe\"");

            //Open Handle to Process
            if (!m_ProcessManager.AttachProcess())
                throw new Exception("Error - Could not attach to process");

            //Take Buffer Dump
            m_Module = m_ProcessManager.GetModule("nioh2.exe");
            m_BufferDump = m_ProcessManager.TakeBufferDump(
                m_Module.BaseAddress,
                (uint)m_Module.ModuleMemorySize
                );

            //Allocate memory within process for hooks/variables
            if (IntPtr.Zero == m_ProcessManager.AllocMemory(m_Module.BaseAddress - 0x10000, 5000))
            {
                //Free memory needed and throw exception
                Unhook();
                throw new Exception("Error - Could not allocate memory for hooks and variables");
            }

            //Generate virtual memory for variables
            m_VirtualAllocExBase = m_Module.BaseAddress - 0x10000;          // Allocate 0x10000 before base address
            m_LocalPlayerAddr = m_VirtualAllocExBase;                       // 8 bytes in size(64 bit address)
            m_GodmodeAddr = m_LocalPlayerAddr + 8;                          // 4 bytes in size(32 bit value)
            m_DamageAddr = m_GodmodeAddr + 4;                               // 4 bytes in size(32 bit value)
            m_StaminaAddr = m_DamageAddr + 4;                               // 4 bytes in size(32 bit value)
            m_AnimaAddr = m_StaminaAddr + 4;                                // 4 bytes in size(32 bit value)
            m_YonkaiChargeAddr = m_AnimaAddr + 4;                           // 4 bytes in size(32 bit value)
            m_YonkaiCooldownAddr = m_YonkaiChargeAddr + 4;                  // 4 bytes in size(32 bit value)
            m_YonkaiTimeAddr = m_YonkaiCooldownAddr + 4;                    // 4 bytes in size(32 bit value)

            //Generate virtual memory for hooks(500 bytes from base)
            m_LocalPlayerHook = m_Module.BaseAddress - 0x10000 + 0x500;     // Allocate 500 bytes for hook
            m_HealthHook = m_LocalPlayerHook + 500;                         // Allocate 500 bytes for hook
            m_StaminaHook = m_HealthHook + 500;                             // Allocate 500 bytes for hook
            m_AnimaHook = m_StaminaHook + 500;                              // Allocate 500 bytes for hook
            m_YonkaiChargeHook = m_AnimaHook + 500;                         // Allocate 500 bytes for hook
            m_YonkaiCooldownHook = m_YonkaiChargeHook + 500;                // Allocate 500 bytes for hook
            m_YonkaiTimeHook = m_YonkaiCooldownHook + 500;                  // Allocate 500 bytes for hook

            //Check buffer returned matches size requested
            if (m_BufferDump.Length != m_Module.ModuleMemorySize)
                throw new Exception("Error - Buffer dump size does not match module size");
        }
        public void Cleanup()
        {
            m_ProcessManager.DisconnectProcess();
        }
        public void GetAoB()
        {
            System.Console.WriteLine("LocalPlayer:    nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindAoB(m_BufferDump, AoB.aobLocalPlayer).ToInt64(), 16));
            System.Console.WriteLine("Health:         nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindAoB(m_BufferDump, AoB.aobHealth).ToInt64(), 16));
            System.Console.WriteLine("Stamina:        nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindAoB(m_BufferDump, AoB.aobStamina).ToInt64(), 16));
            System.Console.WriteLine("Anima:          nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindAoB(m_BufferDump, AoB.aobAnima).ToInt64(), 16));
            System.Console.WriteLine("YonkaiCharge:   nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindAoB(m_BufferDump, AoB.aobYonkaiCharge).ToInt64(), 16));
            System.Console.WriteLine("YonkaiCooldown: nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindAoB(m_BufferDump, AoB.aobYonkaiCooldown).ToInt64(), 16));
            System.Console.WriteLine("YonkaiTime:     nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindAoB(m_BufferDump, AoB.aobYonkaiTime).ToInt64(), 16));
            System.Console.WriteLine();
            return;
        }
        public bool HookLocalPlayerFunc()
        {
            //Find aob address
            m_LocalPlayerFunc = m_Module.BaseAddress + m_ProcessManager.FindAoB(
                m_BufferDump,
                AoB.aobLocalPlayer,
                null
                ).ToInt32();

            //Get Payload
            byte[] payload = Shellcode.GetLocalPlayerHook(
                m_LocalPlayerHook,
                m_LocalPlayerAddr,
                m_LocalPlayerFunc + 5
                );
            m_ProcessManager.PatchMemory(m_LocalPlayerHook, payload);

            //Jump calculation(WARNING - signed 64 bit integer to signed 32 bit integer
            int jmpToHook = (int)(
                  m_LocalPlayerHook.ToInt64() - m_LocalPlayerFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.origLocalPlayerBytes.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Replace with jmp instruction and offset to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_LocalPlayerFunc, replaceBytes);
            return true;
        }
        public bool HookHealthFunc()
        {
            //Find health func
            m_HealthFunc = m_Module.BaseAddress + m_ProcessManager.FindAoB(
                m_BufferDump,
                AoB.aobHealth,
                null
                ).ToInt32();

            //Patch health hook
            byte[] payload = Shellcode.GetHealthHook(
                m_HealthHook,
                m_LocalPlayerAddr,
                m_GodmodeAddr,
                m_DamageAddr,
                m_HealthFunc + 5
                );
            m_ProcessManager.PatchMemory(m_HealthHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_HealthHook.ToInt64() - m_HealthFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.origHealthBytes.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_HealthFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_GodmodeAddr, 1);
            m_ProcessManager.WriteMemory<int>(m_DamageAddr, 1);

            return true;
        }
        public bool HookStaminaFunc()
        {
            //Find stamina func
            m_StaminaFunc = m_Module.BaseAddress + m_ProcessManager.FindAoB(
                m_BufferDump,
                AoB.aobStamina,
                null
                ).ToInt32();

            //Patch stamina func
            byte[] payload = Shellcode.GetStaminaHook(
                m_StaminaHook,
                m_StaminaAddr,
                m_LocalPlayerAddr,
                m_StaminaFunc + 5
                );
            m_ProcessManager.PatchMemory(m_StaminaHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_StaminaHook.ToInt64() - m_StaminaFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.origStaminaBytes.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_StaminaFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_StaminaAddr, 0);

            return true;
        }
        public bool HookAnimaFunc()
        {
            //Find anima func
            m_AnimaFunc = m_Module.BaseAddress + m_ProcessManager.FindAoB(
                m_BufferDump,
                AoB.aobAnima,
                null
                ).ToInt32();

            //Patch anima func
            byte[] payload = Shellcode.GetAnimaHook(
                m_AnimaHook,
                m_AnimaAddr,
                m_LocalPlayerAddr,
                m_AnimaFunc + 5
                );
            m_ProcessManager.PatchMemory(m_AnimaHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_AnimaHook.ToInt64() - m_AnimaFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.origAnimaBytes.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_AnimaFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_AnimaAddr, 0);

            return true;
        }
        public bool HookYonkaiChargeFunc()
        {
            //Find yonkai charge func
            m_YonkaiChargeFunc = m_Module.BaseAddress + m_ProcessManager.FindAoB(
                m_BufferDump,
                AoB.aobYonkaiCharge,
                null
                ).ToInt32();

            //Patch Yonkai charge func
            byte[] payload = Shellcode.GetYonkaiChargeHook(
                m_YonkaiChargeHook,
                m_YonkaiChargeAddr,
                m_LocalPlayerAddr,
                m_YonkaiChargeFunc + 5
                );
            m_ProcessManager.PatchMemory(m_YonkaiChargeHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_YonkaiChargeHook.ToInt64() - m_YonkaiChargeFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.origYonkaiChargeBytes.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_YonkaiChargeFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_YonkaiChargeAddr, 0);

            return true;
        }
        public bool HookYonkaiCooldownFunc()
        {
            //Find yonkai cooldown func
            m_YonkaiCooldownFunc = m_Module.BaseAddress + m_ProcessManager.FindAoB(
                m_BufferDump,
                AoB.aobYonkaiCooldown,
                null
                ).ToInt32();

            //Patch yonkai cooldown func
            byte[] payload = Shellcode.GetYonkaiCooldownHook(
                m_YonkaiCooldownHook,
                m_YonkaiCooldownAddr,
                m_LocalPlayerAddr,
                m_YonkaiCooldownFunc + 5
                );
            m_ProcessManager.PatchMemory(m_YonkaiCooldownHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_YonkaiCooldownHook.ToInt64() - m_YonkaiCooldownFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.origYonkaiCooldownBytes.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_YonkaiCooldownFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_YonkaiCooldownAddr, 0);

            return true;
        }
        public bool HookYonkaiTimeFunc()
        {
            //Find anima func
            m_YonkaiTimeFunc = m_Module.BaseAddress + m_ProcessManager.FindAoB(
                m_BufferDump,
                AoB.aobYonkaiTime,
                null
                ).ToInt32();

            //Patch anima func
            byte[] payload = Shellcode.GetYonkaiTimeHook(
                m_YonkaiTimeHook,
                m_YonkaiTimeAddr,
                m_LocalPlayerAddr,
                m_YonkaiTimeFunc + 5
                );
            m_ProcessManager.PatchMemory(m_YonkaiTimeHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_YonkaiTimeHook.ToInt64() - m_YonkaiTimeFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.origYonkaiTimeBytes.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_YonkaiTimeFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_YonkaiTimeAddr, 0);

            return true;
        }
        public bool Unhook()
        {
            // Free and replace with original(For Debug)
            m_ProcessManager.PatchMemory(
                m_HealthFunc,
                AoB.origHealthBytes
                );
            m_ProcessManager.PatchMemory(
                m_StaminaFunc,
                AoB.origStaminaBytes
                );
            m_ProcessManager.PatchMemory(
                m_AnimaFunc,
                AoB.origAnimaBytes
                );
            m_ProcessManager.PatchMemory(
                m_YonkaiChargeFunc,
                AoB.origYonkaiChargeBytes
                );
            m_ProcessManager.PatchMemory(
                m_YonkaiCooldownFunc,
                AoB.origYonkaiCooldownBytes
                );
            m_ProcessManager.PatchMemory(
                m_YonkaiTimeFunc,
                AoB.origYonkaiTimeBytes
                );
            m_ProcessManager.PatchMemory(
                m_LocalPlayerFunc,
                AoB.origLocalPlayerBytes
                );
            m_ProcessManager.FreeMemory(m_VirtualAllocExBase);

            return true;
        }
    }
}