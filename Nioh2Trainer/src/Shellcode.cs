using System;

namespace Nioh2Trainer
{
    class Shellcode
    {
        private static byte[] m_LocalPlayerHook = {                             // ------------------ Local Player Hook ------------------
            0x48, 0x83, 0x79, 0x08, 0x01,                                       //0      cmp    QWORD PTR [rcx+0x8],0x1
            0x75, 0x0F,                                                         //5      jne    16 <code>
            0x50,                                                               //7      push   rax
            0x48, 0xB8, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01,         //8      movabs rax,LocalPlayer (starts @ 10th byte)
            0x48, 0x89, 0x08,                                                   //18     mov    QWORD PTR [rax],rcx
            0x58,                                                               //21     pop    rax
                                                                                //  code:
            0x44, 0x89, 0xF0,                                                   //22     mov    eax,r14d
            0x48, 0x3B, 0x41, 0x20,                                             //25     cmp    rax,QWORD PTR [rcx+0x20]
            0xE9, 0x00, 0x00, 0x00, 0x00                                        //29     jmp    jmpback
        };                                                                      //34
        private static byte[] m_HealthHook = {                                  // ------------------ Health Hook ------------------
            0x50,                                                               //0      push rax
            0x48, 0x8B, 0xC3,                                                   //1      mov rax,rbx
            0x48, 0x83, 0xE8, 0x10,                                             //4      sub rax,0x10
            0x48, 0x3B, 0x05, 0xF1, 0x0F, 0x01, 0x00,                           //8      cmp rax,[LocalPlayer] needs imm32(rel offset), 0xA
            0x58,                                                               //15     pop rax
            0x0F, 0x85, 0x0C, 0x00, 0x00, 0x00,                                 //16     jne enemy
            0x0F, 0xAF, 0x3D, 0xE3, 0x0F, 0x00, 0x00,                           //22     imul edi,[GODMODE] needs imm32(rel offset)
            0xE9, 0x07, 0x00, 0x00, 0x00,                                       //29     jmp code:
                                                                                //  enemy:
            0x0F, 0xAF, 0x3D, 0xDB, 0x0F, 0x00, 0x00,                           //34     imul edi,[DAMAGE] needs imm32(rel offset)
                                                                                //  code:
            0x8B, 0x43, 0x10,                                                   //41     mov eax,[rbx+0x10]
            0x29, 0xF8,                                                         //44     sub eax,edi
            0xE9, 0x32, 0xC6, 0x7B, 0x00                                        //46     jmp jmpback
        };                                                                      //51
        private static byte[] m_StaminaHook = {                                 // ------------------ Stamina Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x1B, 0x00, 0x00, 0x00,                                 //7      je code
            0x50,                                                               //13     push rax
            0x48, 0x8B, 0xC1,                                                   //14     mov rax,rcx
            0x48, 0x83, 0xE8, 0x40,                                             //17     sub rax,40
            0x48, 0x3B, 0x05, 0xF1, 0x0F, 0x01, 0x00,                           //21     cmp rax,[localPlayer]
            0x58,                                                               //28     pop rax
            0x0F, 0x85, 0x05, 0x00, 0x00, 0x00,                                 //29     jne code
            0xF3, 0x0F, 0x10, 0x59, 0x0C,                                       //35     movss xmm3,[rcx+0xC]
                                                                                //  code:
            0xF3, 0x0F, 0x11, 0x59, 0x08,                                       //40     movss [rcx+0x8],xmm3
            0xE9, 0xFA, 0x23, 0x82, 0x00                                        //45     jmp jmpback
        };                                                                      //50
        private static byte[] m_AnimaHook = {                                   // ------------------ Anima Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x1D, 0x00, 0x00, 0x00,                                 //7      je code
            0x50,                                                               //13     push rax
            0x48, 0x8B, 0xC1,                                                   //14     mov rax,rcx
            0x48, 0x2D, 0x60, 0x03, 0x00, 0x00,                                 //17     sub rax,0x360
            0x48, 0x3B, 0x05, 0xEF, 0x0F, 0x02, 0x00,                           //23     cmp rax, [localPlayer]
            0x58,                                                               //30     pop rax
            0x0F, 0x85, 0x05, 0x00, 0x00, 0x00,                                 //31     jne code
            0xF3, 0x0F, 0x10, 0x51, 0x04,                                       //37     movss xmm2,[rcx+0x4]
                                                                                //  code:
            0x0F, 0x2F, 0xD9,                                                   //42     comiss xmm3,xmm1
            0xF3, 0x0F, 0x11, 0x11,                                             //45     movss [rcx],xmm2
            0xE9, 0x8B, 0x10, 0x83, 0x00                                        //49     jmp jmpBack
        };                                                                      //54     
        private static byte[] m_YonkaiChargeHook = {                            // ------------------ Yonkai Charge Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x1D, 0x00, 0x00, 0x00,                                 //7      je code
            0x48, 0x3B, 0x05, 0xF9, 0x0F, 0x02, 0x00,                           //13     cmp rax,[localPlayer]
            0x0F, 0x85, 0x10, 0x00, 0x00, 0x00,                                 //20     jne code
            0xF3, 0x0F, 0x2A, 0x80, 0xC8, 0x00, 0x00, 0x00,                     //26     cvtsi2ss xmm0,[rax+0xC8]
            0xF3, 0x0F, 0x11, 0x80, 0xC4, 0x00, 0x00, 0x00,                     //34     movss [rax+0xC4],xmm0
                                                                                //  code:
            0xF3, 0x0F, 0x10, 0x80, 0xC4, 0x00, 0x00, 0x00,                     //42     movss xmm0,[rax+0xC4]
            0xE9, 0x09, 0xB9, 0x85, 0x00                                        //50     jmp jmpback
        };                                                                      //55 
        private static byte[] m_YonkaiCooldownHook = {                          // ------------------ Yonkai Cooldown Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x18, 0x00, 0x00, 0x00,                                 //7      je code
            0x4C, 0x3B, 0x05, 0xF9, 0x0F, 0x02, 0x00,                           //13     cmp r8,[localPlayer]
            0x0F, 0x85, 0x0B, 0x00, 0x00, 0x00,                                 //20     jne code
            0x41, 0xC7, 0x80, 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,   //26     mov [r8+0xD0],(float)0
                                                                                //  code:
            0xF3, 0x41, 0x0F, 0x10, 0x80, 0xD0, 0x00, 0x00, 0x00,               //37     movss xmm0,[r8+0xD0]
            0xE9, 0xB1, 0xDD, 0x85, 0x00                                        //46     jmp jmpback
        };                                                                      //51 
        private static byte[] m_YonkaiTimeHook = {                              //  ------------------ Yonkai Time Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x1D, 0x00, 0x00, 0x00,                                 //7      je code
            0x50,                                                               //13     push rax
            0x48, 0x8B, 0xC1,                                                   //14     mov rax,rcx
            0x48, 0x2D, 0x40, 0x01, 0x00, 0x00,                                 //17      sub rax,0x140
            0x48, 0x3B, 0x05, 0xEF, 0x0F, 0x04, 0x00,                           //23     cmp rax,[localPlayer]
            0x58,                                                               //30     pop rax
            0x0F, 0x85, 0x05, 0x00, 0x00, 0x00,                                 //31     jne code
            0xF3, 0x0F, 0x10, 0x49, 0x28,                                       //37     movss xmm1,[rcx+0x28]
                                                                                //  code:
            0xF3, 0x0F, 0x11, 0x49, 0x24,                                       //42     movss [rcx+0x24],xmm1
            0xE9, 0x16, 0x55, 0x85, 0x00                                        //47     jmp jmpback
        };                                                                      //52
        private static byte[] m_BuffDurationHook = {                            //  ------------------ Buff Duration Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x1D, 0x00, 0x00, 0x00,                                 //7      je code
            0x50,                                                               //13     push rax
            0x48, 0xA1, 0x00, 0x10, 0xDF, 0x4E, 0xF7, 0x7F, 0x00, 0x00,         //14     mov rax,[localPlayer]
            0x48, 0x8B, 0x00,                                                   //24     mov rax,[rax]
            0x58,                                                               //31     pop rax
            0x48, 0x39, 0x43, 0x08,                                             //27     cmp [rbx+0x08],rax
            0x0F, 0x85, 0x05, 0x00, 0x00, 0x00,                                 //32     jne code
            0xF3, 0x0F, 0x10, 0x43, 0x24,                                       //38     movss xmm0,[rbx+0x24]
                                                                                //  code:
            0xF3, 0x0F, 0x11, 0x43, 0x20,                                       //43     movss [rbx+0x20],xmm0
            0xE9, 0xC0, 0x74, 0x82, 0x00                                        //48     jmp jmpback
        };                                                                      //53
        private static byte[] m_ItemConsumeHook = {                             //  ------------------ Item Consume Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x04, 0x00, 0x00, 0x00,                                 //7      je code
            0x66, 0xBD, 0x00, 0x00,                                             //13     mov bp,0000
                                                                                //  code:
            0x66, 0x29, 0xE8,                                                   //17     sub ax,bp
            0x48, 0x8B, 0xCF,                                                   //20     mov rcx,rdi
            0xE9, 0x6C, 0x85, 0xA5, 0x00                                        //23     jmp jmpback
        };                                                                      //28
        private static byte[] m_NinjustOnmyoHook = {                            //  ------------------ Magic Consume Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x04, 0x00, 0x00, 0x00,                                 //7      je code
            0x66, 0xBF, 0x00, 0x00,                                             //13     mov di,0000
                                                                                //  code:
            0x66, 0x29, 0xF8,                                                   //17     sub ax,di
            0x48, 0x8B, 0xCE,                                                   //20     mov rcx,rsi
            0xE9, 0x39, 0x8C, 0xA5, 0x00                                        //23     jmp jmpback
        };                                                                      //28
        private static byte[] m_MaxItemsHook = {                                //  ------------------ Max Item Pick Up Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x04, 0x00, 0x00, 0x00,                                 //7      je code
            0x66, 0xBA, 0xFF, 0x7F,                                             //13     mov dx,0x7FFF
                                                                                //  code:
            0x66, 0x89, 0x51, 0x04,                                             //17     mov [rcx+0x4],dx
            0xC3,                                                               //21     ret
            0xE9, 0x2B, 0xD0, 0xC6, 0x00                                        //22     jmp jmpback
        };                                                                      //27
        private static byte[] m_GloryHook = {                                   //  ------------------ Max Glory Pick Up Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x05, 0x00, 0x00, 0x00,                                 //7      je code
            0xB8, 0xFF, 0x7F, 0x00, 0x00,                                       //13     mov eax,0x7FF
                                                                                //  code:
            0x89, 0x87, 0xBC, 0x04, 0x00, 0x00,                                 //18     mov [rdi+0x4BC],eax
            0xE9, 0x0E, 0x3F, 0xA3, 0x00                                        //24     jmp jmpback
        };                                                                      //29
        private static byte[] m_AmritaHook = {                                  //  ------------------ Max Amrita Pick Up Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x0A, 0x00, 0x00, 0x00,                                 //7      je code
            0x48, 0xB8, 0xFF, 0xFF, 0xFF, 0x7F, 0x00, 0x00, 0x00, 0x00,         //13     mov rax,0x7FFFFFFF
                                                                                //  code:
            0x48, 0x89, 0x87, 0x78, 0xB7, 0x07, 0x00,                           //23     mov [rdi+0x7B778],rax
            0xE9, 0xCB, 0x2F, 0xA2, 0x00                                        //30     jmp jmpback
        };                                                                      //35
        private static byte[] m_ProficiencyHook = {                             //  ------------------ Proficiency Multiplier Hook ------------------
            0x83, 0x3D, 0xF9, 0x0F, 0x01, 0x00, 0x00,                           //0      cmp dword ptr[enable],00
            0x0F, 0x84, 0x07, 0x00, 0x00, 0x00,                                 //7      je code
            0x0F, 0xAF, 0x1D, 0xF3, 0x0F, 0x00, 0x00,                           //13     imul ebx,[Multiplier]
                                                                                //  code:
            0x41, 0x03, 0x1C, 0x24,                                             //20     add ebx,[r12]
            0x39, 0xC3,                                                         //24     cmp eax,eax
            0xE9, 0x22, 0x99, 0xA2, 0x00                                        //26     jmp jmpback
        };                                                                      //31

        public static byte[] GetLocalPlayerHook(
            IntPtr hookAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            //Local Player Bytes to replace
            byte[] localPlayerBytes = BitConverter.GetBytes(playerAddr.ToInt64());

            //Replace localplayer mem buffer
            for (int i = 0; i < localPlayerBytes.Length; i++)
            {
                m_LocalPlayerHook[10 + i] = localPlayerBytes[i];
            }

            //Calculate jmp back
            int jmpBackOffset = (int)(jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 34);
            byte[] jmpBackOffsetBytes = BitConverter.GetBytes(jmpBackOffset);

            //Replace jump back bytes
            for (int i = 0; i < jmpBackOffsetBytes.Length; i++)
            {
                m_LocalPlayerHook[0x1E + i] = jmpBackOffsetBytes[i];
            }

            return m_LocalPlayerHook;
        }

        public static byte[] GetHealthHook(
            IntPtr hookAddr,
            IntPtr playerAddr,
            IntPtr godmodeAddr,
            IntPtr damageAddr,
            IntPtr jmpBackAddr
            )
        {
            //Calculate relative offsets
            int relLocalPlayer = (int)(
                playerAddr.ToInt64() - hookAddr.ToInt64() - 15
                );
            int relGodmode = (int)(
                godmodeAddr.ToInt64() - hookAddr.ToInt64() - 29
                );
            int relDamage = (int)(
                damageAddr.ToInt64() - hookAddr.ToInt64() - 41
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 51
                );

            //Replace local player bytes
            byte[] localPlayerBytes = BitConverter.GetBytes(relLocalPlayer);
            for (int i = 0; i < localPlayerBytes.Length; i++) { m_HealthHook[11 + i] = localPlayerBytes[i]; }

            //Replace Godmode bytes
            byte[] godmodeBytes = BitConverter.GetBytes(relGodmode);
            for (int i = 0; i < godmodeBytes.Length; i++) { m_HealthHook[25 + i] = godmodeBytes[i]; }

            //Replace Damage bytes
            byte[] damageBytes = BitConverter.GetBytes(relDamage);
            for (int i = 0; i < damageBytes.Length; i++) { m_HealthHook[37 + i] = damageBytes[i]; }

            //Replace RETURN bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_HealthHook[47 + i] = jmpBackBytes[i]; }

            return m_HealthHook;
        }

        public static byte[] GetStaminaHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {

            //Calculate relative offsets
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relLocalPlayer = (int)(
                playerAddr.ToInt64() - hookAddr.ToInt64() - 28
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 50
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_StaminaHook[2 + i] = enableBytes[i]; }

            //Replace local player bytes
            byte[] localPlayerBytes = BitConverter.GetBytes(relLocalPlayer);
            for (int i = 0; i < localPlayerBytes.Length; i++) { m_StaminaHook[24 + i] = localPlayerBytes[i]; }

            //Replace jmp back bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_StaminaHook[46 + i] = jmpBackBytes[i]; }

            return m_StaminaHook;
        }

        public static byte[] GetAnimaHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            //Calculate relative offsets
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relLocalPlayer = (int)(
                playerAddr.ToInt64() - hookAddr.ToInt64() - 30
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 54
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_AnimaHook[2 + i] = enableBytes[i]; }

            //Replace local player bytes
            byte[] localPlayerBytes = BitConverter.GetBytes(relLocalPlayer);
            for (int i = 0; i < localPlayerBytes.Length; i++) { m_AnimaHook[26 + i] = localPlayerBytes[i]; }

            // Replace jmp back bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_AnimaHook[50 + i] = jmpBackBytes[i]; }

            return m_AnimaHook;
        }

        public static byte[] GetYonkaiChargeHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            //Calculate relative offsets
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relLocalPlayer = (int)(
                playerAddr.ToInt64() - hookAddr.ToInt64() - 20
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 55
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_YonkaiChargeHook[2 + i] = enableBytes[i]; }

            //Replace localPlayer bytes
            byte[] localPlayerBytes = BitConverter.GetBytes(relLocalPlayer);
            for (int i = 0; i < localPlayerBytes.Length; i++) { m_YonkaiChargeHook[16 + i] = localPlayerBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_YonkaiChargeHook[51 + i] = jmpBackBytes[i]; }

            return m_YonkaiChargeHook;
        }

        public static byte[] GetYonkaiCooldownHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            //Calculate relative offsets
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relLocalPlayer = (int)(
                playerAddr.ToInt64() - hookAddr.ToInt64() - 20
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 51
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_YonkaiCooldownHook[2 + i] = enableBytes[i]; }

            //Replace localPlayer bytes
            byte[] localPlayerBytes = BitConverter.GetBytes(relLocalPlayer);
            for (int i = 0; i < localPlayerBytes.Length; i++) { m_YonkaiCooldownHook[16 + i] = localPlayerBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_YonkaiCooldownHook[47 + i] = jmpBackBytes[i]; }

            return m_YonkaiCooldownHook;
        }

        public static byte[] GetYonkaiTimeHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            //Calculate relative offsets
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relLocalPlayer = (int)(
                playerAddr.ToInt64() - hookAddr.ToInt64() - 30
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 52
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_YonkaiTimeHook[2 + i] = enableBytes[i]; }

            //Replace localPlayer bytes
            byte[] localPlayerBytes = BitConverter.GetBytes(relLocalPlayer);
            for (int i = 0; i < localPlayerBytes.Length; i++) { m_YonkaiTimeHook[26 + i] = localPlayerBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_YonkaiTimeHook[48 + i] = jmpBackBytes[i]; }

            return m_YonkaiTimeHook;
        }
        public static byte[] GetBuffDurationHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            //Calculate relative offsets
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 53
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_BuffDurationHook[2 + i] = enableBytes[i]; }

            //Replace localPlayer bytes
            byte[] localPlayerBytes = BitConverter.GetBytes(playerAddr.ToInt64());
            for (int i = 0; i < localPlayerBytes.Length; i++) { m_BuffDurationHook[16 + i] = localPlayerBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_BuffDurationHook[49 + i] = jmpBackBytes[i]; }

            return m_BuffDurationHook;
        }
        public static byte[] GetItemConsumeHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            //Calculate relative offsets
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 28
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_ItemConsumeHook[2 + i] = enableBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_ItemConsumeHook[24 + i] = jmpBackBytes[i]; }

            return m_ItemConsumeHook;
        }
        public static byte[] GetNinjustuOnmyoConsumeHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            //Calculate relative offsets
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 28
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_NinjustOnmyoHook[2 + i] = enableBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_NinjustOnmyoHook[24 + i] = jmpBackBytes[i]; }

            return m_NinjustOnmyoHook;
        }
        public static byte[] GetAmritaPickUpHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 35
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_AmritaHook[2 + i] = enableBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_AmritaHook[31 + i] = jmpBackBytes[i]; }

            return m_AmritaHook;
        }
        public static byte[] GetGloryPickUpHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 29
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_GloryHook[2 + i] = enableBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_GloryHook[25 + i] = jmpBackBytes[i]; }

            return m_GloryHook;
        }
        public static byte[] GetItemPickUpHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr playerAddr,
            IntPtr jmpBackAddr
            )
        {
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 27
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_MaxItemsHook[2 + i] = enableBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_MaxItemsHook[23 + i] = jmpBackBytes[i]; }

            return m_MaxItemsHook;
        }
        public static byte[] GetProficiencyHook(
            IntPtr hookAddr,
            IntPtr enableAddr,
            IntPtr multAddr,
            IntPtr jmpBackAddr
            )
        {
            int relEnable = (int)(
                enableAddr.ToInt64() - hookAddr.ToInt64() - 7
                );
            int relMultiplier = (int)(
                multAddr.ToInt64() - hookAddr.ToInt64() - 20
                );
            int relJmpBack = (int)(
                jmpBackAddr.ToInt64() - hookAddr.ToInt64() - 31
                );

            //Replace enable bytes
            byte[] enableBytes = BitConverter.GetBytes(relEnable);
            for (int i = 0; i < enableBytes.Length; i++) { m_ProficiencyHook[2 + i] = enableBytes[i]; }

            //Replace multipler bytes
            byte[] multBytes = BitConverter.GetBytes(relMultiplier);
            for (int i = 0; i < multBytes.Length; i++) { m_ProficiencyHook[16 + i] = multBytes[i]; }

            //Replace jmpBack bytes
            byte[] jmpBackBytes = BitConverter.GetBytes(relJmpBack);
            for (int i = 0; i < jmpBackBytes.Length; i++) { m_ProficiencyHook[27 + i] = jmpBackBytes[i]; }

            return m_ProficiencyHook;
        }
    }
}