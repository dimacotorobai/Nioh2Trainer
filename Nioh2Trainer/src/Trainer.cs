using Nioh2Trainer.src;
using System;
using System.Diagnostics;
using System.Security.Permissions;

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
        private IntPtr m_BuffDurationFunc;
        private IntPtr m_ItemConsumeFunc;
        private IntPtr m_NinjustuOnmyoFunc;
        private IntPtr m_ItemPickUpFunc;
        private IntPtr m_AmritaPickUpFunc;
        private IntPtr m_GloryPickUpFunc;
        private IntPtr m_ProficiencyFunc;

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
        private IntPtr m_BuffDurationAddr;
        private IntPtr m_ItemConsumeAddr;
        private IntPtr m_NinjustuOnmyoAddr;
        private IntPtr m_ItemPickUpAddr;
        private IntPtr m_AmritaPickUpAddr;
        private IntPtr m_GloryPickUpAddr;
        private IntPtr m_ProficiencyAddr;
        private IntPtr m_ProficiencyMultAddr;

        //Virtual Alloc Hooks
        private IntPtr m_LocalPlayerHook;
        private IntPtr m_HealthHook;
        private IntPtr m_StaminaHook;
        private IntPtr m_AnimaHook;
        private IntPtr m_YonkaiChargeHook;
        private IntPtr m_YonkaiCooldownHook;
        private IntPtr m_YonkaiTimeHook;
        private IntPtr m_BuffDurationHook;
        private IntPtr m_ItemConsumeHook;
        private IntPtr m_NinjustuOnmyoHook;
        private IntPtr m_ItemPickUpHook;
        private IntPtr m_AmritaPickUpHook;
        private IntPtr m_GloryPickUpHook;
        private IntPtr m_ProficiencyHook;

        // Class Properties
        public string PROCESS_NAME
        {
            get { return (m_ProcessManager != null) ? m_ProcessManager.ProcessName : string.Empty; }
        }
        public uint PROCESS_ID
        {
            get { return (m_ProcessManager != null) ? m_ProcessManager.ProcessID : 0; }
        }
        public IntPtr MODULE_BASE_ADDRESS
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
        public bool STAMINA
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_StaminaAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_StaminaAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_StaminaAddr) != 0;
            }
        }
        public bool ANIMA
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
        public bool YONKAI_CHARGE
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
        public bool YONKAI_COOLDOWN
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
        public bool YONKAI_TIME
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
        public bool BUFF_DURATION
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_BuffDurationAddr, 1):
                    m_ProcessManager.WriteMemory<int>(m_BuffDurationAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_BuffDurationAddr) != 0;
            }
        }

        public bool ITEM_CONSUME
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_ItemConsumeAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_ItemConsumeAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_ItemConsumeAddr) != 0;
            }
        }

        public bool NINJUSTU_ONMYO_CONSUME
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_NinjustuOnmyoAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_NinjustuOnmyoAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_NinjustuOnmyoAddr) != 0;
            }
        }

        public bool ITEM_PICK_UP
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_ItemPickUpAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_ItemPickUpAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_ItemPickUpAddr) != 0;
            }
        }

        public bool AMRITA_PICK_UP
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_AmritaPickUpAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_AmritaPickUpAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_AmritaPickUpAddr) != 0;
            }
        }
        public bool GLORY_PICK_UP
        {
            set
            {
                _ = value ?
                    m_ProcessManager.WriteMemory<int>(m_GloryPickUpAddr, 1) :
                    m_ProcessManager.WriteMemory<int>(m_GloryPickUpAddr, 0);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_GloryPickUpAddr) != 0;
            }
        }
        public int PROFICIENCY_GAIN
        {
            set
            {
                m_ProcessManager.WriteMemory<int>(m_ProficiencyMultAddr, value);
            }
            get
            {
                return m_ProcessManager.ReadMemory<int>(m_ProficiencyMultAddr);
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

            //Get Module
            m_Module = m_ProcessManager.GetModule("nioh2.exe");

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
            m_BuffDurationAddr = m_YonkaiTimeAddr + 4;                      // 4 bytes in size(32 bit value)

            m_ItemConsumeAddr = m_BuffDurationAddr + 4;                     // 4 bytes in size(32 bit value)
            m_NinjustuOnmyoAddr = m_ItemConsumeAddr + 4;                    // 4 bytes in size(32 bit value)
            m_ItemPickUpAddr = m_NinjustuOnmyoAddr + 4;                     // 4 bytes in size(32 bit value)
            m_AmritaPickUpAddr = m_ItemPickUpAddr + 4;                      // 4 bytes in size(32 bit value)
            m_GloryPickUpAddr = m_AmritaPickUpAddr + 4;                     // 4 bytes in size(32 bit value)

            m_ProficiencyAddr = m_GloryPickUpAddr + 4;                      // 4 bytes in size(32 bit value)
            m_ProficiencyMultAddr = m_ProficiencyAddr + 4;                  // 4 bytes in size(32 bit value)

            //Generate virtual memory for hooks(500 bytes from base)
            m_LocalPlayerHook = m_Module.BaseAddress - 0x10000 + 0x500;     // Allocate 500 bytes for hook
            m_HealthHook = m_LocalPlayerHook + 500;                         // Allocate 500 bytes for hook
            m_StaminaHook = m_HealthHook + 500;                             // Allocate 500 bytes for hook
            m_AnimaHook = m_StaminaHook + 500;                              // Allocate 500 bytes for hook
            m_YonkaiChargeHook = m_AnimaHook + 500;                         // Allocate 500 bytes for hook
            m_YonkaiCooldownHook = m_YonkaiChargeHook + 500;                // Allocate 500 bytes for hook
            m_YonkaiTimeHook = m_YonkaiCooldownHook + 500;                  // Allocate 500 bytes for hook
            m_BuffDurationHook = m_YonkaiTimeHook + 500;                    // Allocate 500 bytes for hook

            m_ItemConsumeHook = m_BuffDurationHook + 500;                   // Allocate 500 bytes for hook
            m_NinjustuOnmyoHook = m_ItemConsumeHook + 500;                  // Allocate 500 bytes for hook
            m_ItemPickUpHook = m_NinjustuOnmyoHook + 500;                   // Allocate 500 bytes for hook
            m_AmritaPickUpHook = m_ItemPickUpHook + 500;                    // Allocate 500 bytes for hook
            m_GloryPickUpHook = m_AmritaPickUpHook + 500;                   // Allocate 500 bytes for hook

            m_ProficiencyHook = m_GloryPickUpHook + 500;                    // Allocate 500 bytes for hook

            //Allocate memory within process for hooks/variables
            if (IntPtr.Zero == m_ProcessManager.AllocMemory(m_VirtualAllocExBase, 0x5000))
            {
                //Free Memory and Try Again
                m_ProcessManager.FreeMemory(m_VirtualAllocExBase);
                if (IntPtr.Zero == m_ProcessManager.AllocMemory(m_VirtualAllocExBase, 0x5000))
                {
                    //Free memory needed and throw exception
                    Unhook();
                    throw new Exception("Error - Could not allocate memory for hooks and variables");
                }
            }

            //Take Buffer Dump
            m_BufferDump = m_ProcessManager.TakeBufferDump(
                m_Module.BaseAddress,
                (uint)m_Module.ModuleMemorySize
                );
            if (m_BufferDump.Length != m_Module.ModuleMemorySize)
                throw new Exception("Error - Buffer dump size does not match module size");
        }
        public void Cleanup()
        {
            m_ProcessManager.DisconnectProcess();
        }
        public void GetAoB()
        {

            System.Console.WriteLine("LocalPlayer:      nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.LOCALPLAYER).ToInt64(), 16));
            System.Console.WriteLine("Health:           nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.HEALTH).ToInt64(), 16));
            System.Console.WriteLine("Stamina:          nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.STAMINA).ToInt64(), 16));
            System.Console.WriteLine("Anima:            nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.ANIMA).ToInt64(), 16));
            System.Console.WriteLine("YonkaiCharge:     nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.YONKAI_CHARGE).ToInt64(), 16));
            System.Console.WriteLine("YonkaiCooldown:   nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.YONKAI_COOLDOWN).ToInt64(), 16));
            System.Console.WriteLine("BuffDuration:     nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.YONKAI_TIME).ToInt64(), 16));
            System.Console.WriteLine("ItemConsume:      nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.ITEM_CONSUME).ToInt64(), 16));
            System.Console.WriteLine("NinjustuConsume:  nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.NINJUSTU_ONMYO_CONSUME).ToInt64(), 16));
            System.Console.WriteLine("ItemPickUp:       nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.ITEM_PICK_UP).ToInt64(), 16));
            System.Console.WriteLine("AmritaPickUp:     nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.AMRITA_PICK_UP).ToInt64(), 16));
            System.Console.WriteLine("GloryPickUp:      nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.GLORY_PICK_UP).ToInt64(), 16));
            System.Console.WriteLine("Proficiency:      nioh2.exe + 0x{0}", Convert.ToString(m_ProcessManager.FindSignature(m_BufferDump, Signatures.PROFICIENCY_GAIN).ToInt64(), 16));
            System.Console.WriteLine();
        }

        public void HookLocalPlayerFunc()
        {
            //Find aob address
            m_LocalPlayerFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.LOCALPLAYER,
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
            byte[] replaceBytes = new byte[AoB.LOCAL_PLAYER_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Replace with jmp instruction and offset to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_LocalPlayerFunc, replaceBytes);
            
        }
        public void HookHealthFunc()
        {
            //Find health func
            m_HealthFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.HEALTH,
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
            byte[] replaceBytes = new byte[AoB.HEALTH_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_HealthFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_GodmodeAddr, 1);
            m_ProcessManager.WriteMemory<int>(m_DamageAddr, 1);

            
        }
        public void HookStaminaFunc()
        {
            //Find stamina func
            m_StaminaFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.STAMINA,
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
            byte[] replaceBytes = new byte[AoB.STAMINA_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_StaminaFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_StaminaAddr, 0);

            
        }
        public void HookAnimaFunc()
        {
            //Find anima func
            m_AnimaFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.ANIMA,
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
            byte[] replaceBytes = new byte[AoB.ANIMA_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_AnimaFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_AnimaAddr, 0);

            
        }
        public void HookYonkaiChargeFunc()
        {
            //Find yonkai charge func
            m_YonkaiChargeFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.YONKAI_CHARGE,
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
            byte[] replaceBytes = new byte[AoB.YONKAI_CHARGE_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_YonkaiChargeFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_YonkaiChargeAddr, 0);

            
        }
        public void HookYonkaiCooldownFunc()
        {
            //Find yonkai cooldown func
            m_YonkaiCooldownFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.YONKAI_COOLDOWN,
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
            byte[] replaceBytes = new byte[AoB.YONKAI_COOLDOWN_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_YonkaiCooldownFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_YonkaiCooldownAddr, 0);

            
        }
        public void HookYonkaiTimeFunc()
        {
            //Find anima func
            m_YonkaiTimeFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.YONKAI_TIME,
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
            byte[] replaceBytes = new byte[AoB.YONKAI_TIME_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_YonkaiTimeFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_YonkaiTimeAddr, 0);

            
        }
        public void HookBuffTimeFunc()
        {
            //Find anima func
            m_BuffDurationFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.BUFF_DURATION,
                null
                ).ToInt32();

            //Patch anima func
            byte[] payload = Shellcode.GetBuffDurationHook(
                m_BuffDurationHook,
                m_BuffDurationAddr,
                m_LocalPlayerAddr,
                m_BuffDurationFunc + 5
                );
            m_ProcessManager.PatchMemory(m_BuffDurationHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_BuffDurationHook.ToInt64() - m_BuffDurationFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.BUFF_DURATION_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_BuffDurationFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_BuffDurationAddr, 0);
        }
        public void HookItemConsumeFunc()
        {
            //Find anima func
            m_ItemConsumeFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.ITEM_CONSUME,
                null
                ).ToInt32();

            //Patch anima func
            byte[] payload = Shellcode.GetItemConsumeHook(
                m_ItemConsumeHook,
                m_ItemConsumeAddr,
                IntPtr.Zero,
                m_ItemConsumeFunc + 5
                );
            m_ProcessManager.PatchMemory(m_ItemConsumeHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_ItemConsumeHook.ToInt64() - m_ItemConsumeFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.ITEM_CONSUME_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_ItemConsumeFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_ItemConsumeAddr, 0);

            
        }
        public void HookNinjustuOnmyoFunc()
        {
            //Find anima func
            m_NinjustuOnmyoFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.NINJUSTU_ONMYO_CONSUME,
                null
                ).ToInt32();

            //Patch anima func
            byte[] payload = Shellcode.GetNinjustuOnmyoConsumeHook(
                m_NinjustuOnmyoHook,
                m_NinjustuOnmyoAddr,
                IntPtr.Zero,
                m_NinjustuOnmyoFunc + 5
                );
            m_ProcessManager.PatchMemory(m_NinjustuOnmyoHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_NinjustuOnmyoHook.ToInt64() - m_NinjustuOnmyoFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.NINJUSTU_ONMYO_CONSUME_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_NinjustuOnmyoFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_NinjustuOnmyoAddr, 0);

            
        }
        public void HookItemPickUpFunc()
        {
            //Find anima func
            m_ItemPickUpFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.ITEM_PICK_UP,
                null
                ).ToInt32();

            //Patch anima func
            byte[] payload = Shellcode.GetItemPickUpHook(
                m_ItemPickUpHook,
                m_ItemPickUpAddr,
                IntPtr.Zero,
                m_ItemPickUpFunc + 5
                );
            m_ProcessManager.PatchMemory(m_ItemPickUpHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_ItemPickUpHook.ToInt64() - m_ItemPickUpFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.ITEM_PICK_UP_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_ItemPickUpFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_ItemPickUpAddr, 0);

            
        }
        public void HookAmritaPickUpFunc()
        {
            //Find anima func
            m_AmritaPickUpFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.AMRITA_PICK_UP,
                null
                ).ToInt32();

            //Patch anima func
            byte[] payload = Shellcode.GetAmritaPickUpHook(
                m_AmritaPickUpHook,
                m_AmritaPickUpAddr,
                IntPtr.Zero,
                m_AmritaPickUpFunc + 5
                );
            m_ProcessManager.PatchMemory(m_AmritaPickUpHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_AmritaPickUpHook.ToInt64() - m_AmritaPickUpFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.AMRITA_PICK_UP_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_AmritaPickUpFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_AmritaPickUpAddr, 0);

            
        }
        public void HookGloryPickUpFunc()
        {
            //Find anima func
            m_GloryPickUpFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.GLORY_PICK_UP,
                null
                ).ToInt32() + 1;

            //Patch anima func
            byte[] payload = Shellcode.GetGloryPickUpHook(
                m_GloryPickUpHook,
                m_GloryPickUpAddr,
                IntPtr.Zero,
                m_GloryPickUpFunc + 5
                );
            m_ProcessManager.PatchMemory(m_GloryPickUpHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_GloryPickUpHook.ToInt64() - m_GloryPickUpFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.GLORY_PICK_UP_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_GloryPickUpFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_GloryPickUpAddr, 0);

            
        }
        public void HookProficiencyFunc()
        {
            //Find anima func
            m_ProficiencyFunc = m_Module.BaseAddress + m_ProcessManager.FindSignature(
                m_BufferDump,
                Signatures.PROFICIENCY_GAIN,
                null
                ).ToInt32();

            //Patch anima func
            byte[] payload = Shellcode.GetProficiencyHook(
                m_ProficiencyHook,
                m_ProficiencyAddr,
                m_ProficiencyMultAddr,
                m_ProficiencyFunc + 5
                );
            m_ProcessManager.PatchMemory(m_ProficiencyHook, payload);

            //Calc jump address
            int jmpToHook = (int)(
                m_ProficiencyHook.ToInt64() - m_ProficiencyFunc.ToInt64() - 5
                );
            byte[] addressBytes = BitConverter.GetBytes(jmpToHook);

            //Nop memory
            byte[] replaceBytes = new byte[AoB.PROFICIENCY_GAIN_BYTES.Length];
            for (int i = 0; i < replaceBytes.Length; i++) { replaceBytes[i] = 0x90; }

            //Jump to hook
            replaceBytes[0] = 0xE9;
            for (int i = 0; i < addressBytes.Length; i++) { replaceBytes[1 + i] = addressBytes[i]; }
            m_ProcessManager.PatchMemory(m_ProficiencyFunc, replaceBytes);

            //Initialize default values
            m_ProcessManager.WriteMemory<int>(m_ProficiencyAddr, 1);
            m_ProcessManager.WriteMemory<int>(m_ProficiencyMultAddr, 1);

            
        }

        public void Unhook()
        {
            // Free and replace with original(For Debug)
            m_ProcessManager.PatchMemory(
                m_HealthFunc,
                AoB.HEALTH_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_StaminaFunc,
                AoB.STAMINA_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_AnimaFunc,
                AoB.ANIMA_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_YonkaiChargeFunc,
                AoB.YONKAI_CHARGE_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_YonkaiCooldownFunc,
                AoB.YONKAI_COOLDOWN_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_YonkaiTimeFunc,
                AoB.YONKAI_TIME_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_BuffDurationFunc,
                AoB.BUFF_DURATION_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_ItemConsumeFunc,
                AoB.ITEM_CONSUME_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_NinjustuOnmyoFunc,
                AoB.NINJUSTU_ONMYO_CONSUME_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_ItemPickUpFunc,
                AoB.ITEM_PICK_UP_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_AmritaPickUpFunc,
                AoB.AMRITA_PICK_UP_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_GloryPickUpFunc,
                AoB.GLORY_PICK_UP_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_ProficiencyFunc,
                AoB.PROFICIENCY_GAIN_BYTES
                );
            m_ProcessManager.PatchMemory(
                m_LocalPlayerFunc,
                AoB.LOCAL_PLAYER_BYTES
                );
            m_ProcessManager.FreeMemory(m_VirtualAllocExBase);
        }
    }
}