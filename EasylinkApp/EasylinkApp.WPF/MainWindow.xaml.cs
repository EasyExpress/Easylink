using System.Security.Principal;
using System.Threading;
using System.Windows;

using Easylink;
using EasylinkApp.Business;

namespace EasylinkApp.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
 
         


            var database = DatabaseFactory.Create();

            DataContext = new MainWindowViewModel(database); 
        }

 
    }
}
