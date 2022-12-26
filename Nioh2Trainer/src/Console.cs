using System;
using System.Threading;
using WindowsAPI;

namespace Nioh2Trainer
{
    class Console
    {
        private Trainer m_Nioh2Trainer;
        private bool m_ShouldQuit;

        public bool ShouldQuit
        {
            get { return m_ShouldQuit; }
        }

        public Console() 
        {

        }
        public void Cleanup()
        {
            m_Nioh2Trainer.Unhook();
            m_Nioh2Trainer.Cleanup();
        }
        public bool Setup(string consoleName, string processName)
        {
            try
            {
                //Initialize trainer
                System.Console.Title = consoleName;
                m_Nioh2Trainer = new Trainer();

                //Create all function hooks(LocalPlayer and Health only functions that override current bytes)
                m_Nioh2Trainer.HookLocalPlayerFunc();
                m_Nioh2Trainer.HookHealthFunc();
                m_Nioh2Trainer.HookStaminaFunc();
                m_Nioh2Trainer.HookAnimaFunc();
                m_Nioh2Trainer.HookYonkaiChargeFunc();
                m_Nioh2Trainer.HookYonkaiCooldownFunc();
                m_Nioh2Trainer.HookYonkaiTimeFunc();
            }
            catch (Exception E)
            {
                System.Console.WriteLine(E.Message);
                return false;
            }

            return !(m_ShouldQuit = false);
        }
        public void DrawUserInterface()
        {
            System.Console.Clear();

            System.Console.WriteLine("Process Name: {0}", m_Nioh2Trainer.ProcessName);
            System.Console.WriteLine("Process ID:   0x{0}", Convert.ToString(m_Nioh2Trainer.ProcessID, 16));
            System.Console.WriteLine("Module Base:  0x{0}\n", Convert.ToString(m_Nioh2Trainer.ModuleBase.ToInt64(), 16));

            System.Console.WriteLine("[F1] Godmode         ->{0}<- ", m_Nioh2Trainer.GODMODE);
            System.Console.WriteLine("[F2] OneHitKill      ->{0}<- ", m_Nioh2Trainer.DAMAGE);
            System.Console.WriteLine("[F3] Inf Stamina     ->{0}<- ", m_Nioh2Trainer.INFINITE_STAMINA);
            System.Console.WriteLine("[F4] Inf Anima       ->{0}<- ", m_Nioh2Trainer.INFINITE_ANIMA);
            System.Console.WriteLine("[F5] Yonkai Charge   ->{0}<- ", m_Nioh2Trainer.INFINITE_YONKAI_CHARGE);
            System.Console.WriteLine("[F6] Yonkai Cooldown ->{0}<- ", m_Nioh2Trainer.INFINITE_YONKAI_COOLDOWN);
            System.Console.WriteLine("[F7] Yonkai Time     ->{0}<- ", m_Nioh2Trainer.INFINITE_YONKAI_TIME);
            System.Console.WriteLine("[F8] Exit");
        }
        public void PollEvents()
        {
            while (true)
            {
                //Put thread to sleep
                Thread.Sleep(100);

                //User input
                if (Win32.GetAsyncKeyState(Win32.VK_F1) > 0)
                {
                    m_Nioh2Trainer.GODMODE = !m_Nioh2Trainer.GODMODE;
                    break;
                }
                if (Win32.GetAsyncKeyState(Win32.VK_F2) > 0)
                {
                    m_Nioh2Trainer.DAMAGE = !m_Nioh2Trainer.DAMAGE;
                    break;
                }
                if (Win32.GetAsyncKeyState(Win32.VK_F3) > 0)
                {
                    m_Nioh2Trainer.INFINITE_STAMINA = !m_Nioh2Trainer.INFINITE_STAMINA;
                    break;
                }
                if (Win32.GetAsyncKeyState(Win32.VK_F4) > 0)
                {
                    m_Nioh2Trainer.INFINITE_ANIMA = !m_Nioh2Trainer.INFINITE_ANIMA;
                    break;
                }
                if (Win32.GetAsyncKeyState(Win32.VK_F5) > 0)
                {
                    m_Nioh2Trainer.INFINITE_YONKAI_CHARGE = !m_Nioh2Trainer.INFINITE_YONKAI_CHARGE;
                    break;
                }
                if (Win32.GetAsyncKeyState(Win32.VK_F6) > 0)
                {
                    m_Nioh2Trainer.INFINITE_YONKAI_COOLDOWN = !m_Nioh2Trainer.INFINITE_YONKAI_COOLDOWN;
                    break;
                }
                if (Win32.GetAsyncKeyState(Win32.VK_F7) > 0)
                {
                    m_Nioh2Trainer.INFINITE_YONKAI_TIME = !m_Nioh2Trainer.INFINITE_YONKAI_TIME;
                    break;
                }
                if (Win32.GetAsyncKeyState(Win32.VK_F8) > 0)
                {
                    m_ShouldQuit = true;
                    break;
                }
            }
        }
    }
}