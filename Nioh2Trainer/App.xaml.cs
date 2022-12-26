using System.Windows;

namespace Nioh2Trainer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            Console trainer = new Console();
            trainer.Setup("Nioh 2 Trainer", "nioh2");
            while (!trainer.ShouldQuit)
            {
                trainer.DrawUserInterface();
                trainer.PollEvents();
            }
            trainer.Cleanup();
            Shutdown(0);
        }
    }
}
