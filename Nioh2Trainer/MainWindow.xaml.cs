using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Windows.Controls;

namespace Nioh2Trainer
{
    public partial class MainWindow : Window
    {
        private Trainer m_Nioh2 = null;

        public MainWindow()
        { 
            InitializeComponent();

            //Set all element interaction methods
            this.labelProcess.MouseDown     += Menu_Click;
            this.labelCheats.MouseDown      += Menu_Click;
            this.labelInventory.MouseDown   += Menu_Click;
            this.labelStats.MouseDown       += Menu_Click;
            this.labelAbout.MouseDown       += Menu_Click;

            //Process menu methods
            this.buttonAttach.Click         += AttachToProcess_Click;

            //Cheats menu methods
            this.checkGodmode.Click         += Cheat_Click;
            this.checkOneHitKill.Click      += Cheat_Click;
            this.checkStamina.Click         += Cheat_Click;
            this.checkAnime.Click           += Cheat_Click;
            this.checkYonkaiCharge.Click    += Cheat_Click;
            this.checkYonkaiCooldown.Click  += Cheat_Click;
            this.checkYonkaiDuration.Click  += Cheat_Click;
            this.checkBuffDuration.Click    += Cheat_Click;

            //Inventory menu methods
            this.checkAmrita.Click          += Inventory_Click;
            this.checkGlory.Click           += Inventory_Click;
            this.checkItemPickUp.Click      += Inventory_Click;
            this.checkItemConsume.Click     += Inventory_Click;
            this.checkOnmyo.Click           += Inventory_Click;

            //Stats menu methods
            this.bProficiencyUp.Click       += Stats_Click;
            this.bProfieciencyDown.Click    += Stats_Click;

            //Set initial menu visable
            this.menuProcess.Visibility = Visibility.Visible;
        }
        public void Cleanup()
        {
            if (m_Nioh2 == null)
                return;

            m_Nioh2.Unhook();
            m_Nioh2.Cleanup();
        }

        private void Menu_Click(object sender, MouseButtonEventArgs args)
        {
            //Set all stack frames hidden
            this.menuProcess.Visibility     = Visibility.Hidden;
            this.menuCheats.Visibility      = Visibility.Hidden;
            this.menuInventory.Visibility   = Visibility.Hidden;
            this.menuStats.Visibility       = Visibility.Hidden;
            this.menuAbout.Visibility       = Visibility.Hidden;

            //Menu switch 
            if (this.labelProcess == sender)
            {
                this.menuProcess.Visibility = Visibility.Visible;
            }
            else if (this.labelCheats == sender)
            {
                this.menuCheats.Visibility = Visibility.Visible;
            }
            else if (this.labelInventory == sender)
            {
                this.menuInventory.Visibility = Visibility.Visible;
            }
            else if(this.labelStats == sender)
            {
                this.menuStats.Visibility = Visibility.Visible;
            }
            else if(this.labelAbout == sender)
            {
                this.menuAbout.Visibility = Visibility.Visible;
            }
        }
        private void Cheat_Click(object sender, RoutedEventArgs args)
        {
            if (m_Nioh2 == null)
                return;

            if (checkGodmode == sender)
            {
                m_Nioh2.GODMODE = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkOneHitKill == sender)
            {
                m_Nioh2.DAMAGE = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkStamina == sender)
            {
                m_Nioh2.STAMINA = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkAnime == sender)
            {
                m_Nioh2.ANIMA = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkYonkaiCharge == sender)
            {
                m_Nioh2.YONKAI_CHARGE = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkYonkaiCooldown == sender)
            {
                m_Nioh2.YONKAI_COOLDOWN = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkYonkaiDuration == sender)
            {
                m_Nioh2.YONKAI_TIME = (bool)(sender as CheckBox).IsChecked;
            }
            else if(checkBuffDuration == sender)
            {
                m_Nioh2.BUFF_DURATION = (bool)(sender as CheckBox).IsChecked;
            }
        }
        private void Inventory_Click(object sender, RoutedEventArgs args)
        {
            if(m_Nioh2 == null)
                return;

            if (checkAmrita == sender)
            {
                m_Nioh2.AMRITA_PICK_UP = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkGlory == sender)
            {
                m_Nioh2.GLORY_PICK_UP = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkItemPickUp == sender)
            {
                m_Nioh2.ITEM_PICK_UP = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkItemConsume == sender)
            {
                m_Nioh2.ITEM_CONSUME = (bool)(sender as CheckBox).IsChecked;
            }
            else if (checkOnmyo == sender)
            {
                m_Nioh2.NINJUSTU_ONMYO_CONSUME = (bool)(sender as CheckBox).IsChecked;
            }
        }

        private void Stats_Click(object sender, RoutedEventArgs args)
        {
            if(m_Nioh2 == null) 
                return;

            if(bProficiencyUp == sender)
            {
                int value = m_Nioh2.PROFICIENCY_GAIN;
                Int32.TryParse(tbProficiency.Text, out value);
                value++;
                tbProficiency.Text = value.ToString();
                m_Nioh2.PROFICIENCY_GAIN = value;
            }
            if(bProfieciencyDown == sender)
            {
                int value = m_Nioh2.PROFICIENCY_GAIN;
                Int32.TryParse(tbProficiency.Text, out value);
                if (value < 1) return;
                value--;
                tbProficiency.Text = value.ToString();
                m_Nioh2.PROFICIENCY_GAIN = value;
            }
        }

        private void AttachToProcess_Click(object sender, RoutedEventArgs args) 
        {
            // Return if instance exists
            if (m_Nioh2 != null) 
                return;

            try
            {
                m_Nioh2 = new Trainer();

                m_Nioh2.HookLocalPlayerFunc();
                m_Nioh2.HookHealthFunc();
                m_Nioh2.HookStaminaFunc();
                m_Nioh2.HookAnimaFunc();
                m_Nioh2.HookYonkaiChargeFunc();
                m_Nioh2.HookYonkaiCooldownFunc();
                m_Nioh2.HookYonkaiTimeFunc();
                m_Nioh2.HookBuffTimeFunc();
                m_Nioh2.HookItemConsumeFunc();
                m_Nioh2.HookNinjustuOnmyoFunc();
                m_Nioh2.HookItemPickUpFunc();
                m_Nioh2.HookAmritaPickUpFunc();
                m_Nioh2.HookGloryPickUpFunc();
                m_Nioh2.HookProficiencyFunc();

                this.labelStatus.Content = "Attached";
                this.labelStatus.Foreground = Brushes.Green;

                this.labelPID.Content           = "Process ID: 0x000" + Convert.ToString(m_Nioh2.PROCESS_ID, 16);
                this.labelModbBase.Content      = "Module Base: 0x" + Convert.ToString(m_Nioh2.MODULE_BASE_ADDRESS.ToInt64(), 16);
                this.labelStatusPrimary.Content = "Status: Attached";
            }
            catch (Exception E)
            {
                m_Nioh2 = null;
                this.labelStatusPrimary.Content = "Status: Failed";
                this.labelStatus.Content = "Failed";
                this.labelStatus.Foreground = Brushes.Red;
            }
        }
    }
}
