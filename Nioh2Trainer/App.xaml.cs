using System.Windows;

namespace Nioh2Trainer
{
    public partial class App : Application
    {
        MainWindow window;
        App()
        {
            //Console trainer = new Console();
            //trainer.Setup("Nioh 2 Trainer");
            //while (!trainer.ShouldQuit)
            //{
            //    trainer.DrawUserInterface();
            //    trainer.PollEvents();
            //}
            //trainer.Cleanup();
            //Shutdown(0);
        }
        private void Application_Startup(object sender, StartupEventArgs args)
        {
            window = new MainWindow();
            window.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            window.Cleanup();
            Shutdown(0);
        }
    }
}
