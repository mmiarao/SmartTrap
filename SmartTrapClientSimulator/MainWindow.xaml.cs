using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartTrapClientSimulator
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            vm = new ViewModel(this);
            DataContext = vm;
            InitializeComponent();
        }
        ViewModel vm;

        private async void OnStartClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await vm.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void OnWdClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await vm.WatchDog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void OnTrapped(object sender, RoutedEventArgs e)
        {
            try
            {
                await vm.NotifyTrapped();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel(MainWindow view)
        {
            _view = view;
            _client = new SigfoxWebApiClient();
            SigfoxId = "742E2E";
        }
        MainWindow _view;
        SigfoxWebApiClient _client;
        public string SigfoxId { set; get; }
        public bool Trapped { set; get; }

        void OnPropertyChanged(string p)
        {
            _view.Dispatcher.InvokeAsync(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
            });
        }

        internal async Task Start()
        {
            await _client.SendCommand(SigfoxId, new Models.Commands.StartUpCommand());
        }

        internal async Task WatchDog()
        {
            await _client.SendCommand(SigfoxId, new Models.Commands.WatchDogCommand()
            {
                Trapped = Trapped
            });
        }

        internal async Task NotifyTrapped()
        {
            await _client.SendCommand(SigfoxId, new Models.Commands.TrappedCommand()
            {
                Trapped = Trapped
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
