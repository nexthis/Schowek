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
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Schowek.Controls;
using Schowek.Styles.WindowChrome;

namespace Schowek
{
    /// <summary>
    /// Logika interakcji dla klasy SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window 
    {

        Key newKey ; 
        WinApi.KeyModifier[] newKeyModifier = new WinApi.KeyModifier[2];
        public SettingsWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(SettingsWindow_Loaded);
            Closing += new System.ComponentModel.CancelEventHandler(SettingsWindow_Closing);
            maxImageTextBox.Text = Properties.Settings.Default.ImageMaxWidth.ToString();
            if (Properties.Settings.Default.ColorPrimary == "#343a40") { toggleButtonColor.IsChecked = true; textBlockColor.Text = "Czarny Styl"; }
            else { toggleButtonColor.IsChecked = false; textBlockColor.Text = "Biały Styl"; }
            toggleButtonText.IsChecked = Properties.Settings.Default.NormalText;
            toggleButtonAppClose.IsChecked = !Properties.Settings.Default.AppClose;
            fileListToggle.IsChecked = Properties.Settings.Default.FileDropList;

            if(Properties.Settings.Default.MaxElement != 0)
            maxElementTB.Text = Properties.Settings.Default.MaxElement.ToString();

            TBkey1.Text = ((Key)Properties.Settings.Default.Key1).ToString();
            TBkey2.SelectedIndex = Properties.Settings.Default.Key2;
            TBkey3.SelectedIndex = Properties.Settings.Default.Key3;

            toggleButtonSystem.IsChecked=Properties.Settings.Default.SystemStart;
        }

        private void SettingsWindow_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Key1 = (int)Enum.Parse(typeof(Key), TBkey1.Text.ToString(), true) ;
            Properties.Settings.Default.Key2 = TBkey2.SelectedIndex;
            Properties.Settings.Default.Key3 = TBkey3.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Style _style = null;
            //if (Microsoft.Windows.Shell.SystemParameters2.Current.IsGlassEnabled == true)
           // {
           // _style = (Style)Resources["CustomWindowStyle"];
           // }
          //  this.Style = _style;
        }

        private void maxImageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int max;
            if(int.TryParse(maxImageTextBox.Text,out max))
            {
                if( max > 700 || max < 150)
                {
                    maxImageTextBox.Foreground = Brushes.Red;
                }
                else
                {
                    maxImageTextBox.Foreground = Brushes.Black; 
                    //TODO WAŻNE 
                    Properties.Settings.Default.ImageMaxWidth = max;
                }
            }
            else
            {
                maxImageTextBox.Foreground = Brushes.Red;
            }
        }

        //TEXT 
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.NormalText = true;
            for (int i = 0; i < MainWindow.AppWindow.listBox.Items.Count; i++)
            {
                if (MainWindow.AppWindow.listBox.Items[i].GetType() == typeof(textControl))
                {
                    var texCont = (MainWindow.AppWindow.listBox.Items[i] as textControl);
                    texCont.standardText();
                }
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.NormalText = false;
            for (int i = 0; i < MainWindow.AppWindow.listBox.Items.Count; i++)
            {
                if (MainWindow.AppWindow.listBox.Items[i].GetType() == typeof(textControl))
                {
                    var texCont = (MainWindow.AppWindow.listBox.Items[i] as textControl);
                    texCont.accentText(texCont.paseToClipboard,texCont.textFormat);
                }
            }
        }

        //COLOR
        private void ToggleButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ColorBorder = "#545d67";
            Properties.Settings.Default.ColorPrimary = "#343a40";
            Properties.Settings.Default.ColorWindows = "#616161";
            Properties.Settings.Default.ColorFont = "white";
            Properties.Settings.Default.ColorOver = "#23272b";
            new PaletteHelper().SetLightDark(true);
            textBlockColor.Text = "Czarny Styl";
        }

        private void ToggleButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ColorPrimary = "#16BCE0";
            Properties.Settings.Default.ColorBorder = "white";
            Properties.Settings.Default.ColorWindows = "#ebecec";
            Properties.Settings.Default.ColorFont = "black";
            Properties.Settings.Default.ColorOver = "#06758F";
            new PaletteHelper().SetLightDark(false);
            textBlockColor.Text = "Biały Styl";
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = (sender as TextBox);
            newKey = e.Key;
            (sender as TextBox).Text = e.Key.ToString();
        }

        private void TBkey3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var wtf = (sender as ComboBox);
            WinApi.KeyModifier key = WinApi.KeyModifier.None;
            switch (wtf.SelectedIndex)
            {
                case 0:
                    key = WinApi.KeyModifier.Alt;
                    break;
                case 1:
                    key = WinApi.KeyModifier.None;
                    break;
                case 2:
                    key = WinApi.KeyModifier.Ctrl;
                    break;
                case 3:
                    key = WinApi.KeyModifier.Shift;
                    break;
                case 4:
                    key = WinApi.KeyModifier.Win;
                    break;
            }
            newKeyModifier[int.Parse(wtf.Tag.ToString())] = key;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppWindow.hotKey.Unregister();
            MainWindow.AppWindow.hotKey.Dispose();
            if (newKey == Key.None  ) newKey = (Key)Enum.Parse(typeof(Key), TBkey1.Text.ToString(), true);
            Properties.Settings.Default.Key1=(int) newKey;
            Properties.Settings.Default.Key2 = (int)newKeyModifier[0];
            Properties.Settings.Default.Key3 = (int)newKeyModifier[1];
            MainWindow.AppWindow.hotKey = new WinApi.HotKey(newKey, newKeyModifier[0] | newKeyModifier[1], MainWindow.AppWindow.OnHotKeyHandler);

        }

        private void TBkey1_KeyDown(object sender, KeyEventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        //LINK
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                registryKey.SetValue("Schowek", System.Windows.Forms.Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue("Schowek");
            }
        }

        //SYSTEM START 
        private void ToggleButton_Checked_2(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.SystemStart = true;
            RegisterInStartup(true);
        }

        private void ToggleButton_Unchecked_2(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.SystemStart = false;
            RegisterInStartup(false);
        }
        //Not Close                                                     
        private void ToggleButton_Checked_3(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AppClose = false;
        }

        private void ToggleButton_Unchecked_3(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AppClose = true;
        }

        private void fileListToggle_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.FileDropList = true;
        }

        private void fileListToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.FileDropList = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var value = 0;
            if(int.TryParse(maxElementTB.Text,out value))
            {
                if(value >= 0)
                {
                    Properties.Settings.Default.MaxElement = value;
                    maxElementTB.Foreground = Brushes.Black;
                }
                else
                {
                    maxElementTB.Foreground = Brushes.Red;
                }
            }
            else
            {
                maxElementTB.Foreground = Brushes.Red;
            }
            //
        }
    }
}