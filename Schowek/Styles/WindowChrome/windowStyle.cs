using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Schowek.Styles.WindowChrome
{
    public partial class WindowStyle : ResourceDictionary
    {
        SettingsWindow settingsWindow;
        public WindowStyle()
        {
            InitializeComponent();

        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            if (Properties.Settings.Default.AppClose == false && window.GetType() == typeof(MainWindow)) {
                window.Visibility = Visibility.Hidden;
                return; }
            window.Close();
        }

        private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            if (settingsWindow != null) {
                if(IsWindowOpen<SettingsWindow>("setWindows"))
                {
                    return;
                }
            }
            settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();

        }


        void ButtonLoaded(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            if(window.GetType() == typeof(SettingsWindow))
            {
                (sender as Button).Visibility = Visibility.Hidden;
            }
            
        }


        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = System.Windows.WindowState.Minimized;
        }
        private  bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

    }
}