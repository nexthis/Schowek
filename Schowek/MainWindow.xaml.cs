using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Schowek.AutoUpdate;
using Schowek.Controls;
using Schowek.Coverter;
using Schowek.WinApi;


namespace Schowek
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        public static bool OffClipboard = false;
        public HotKey hotKey;



       public MainWindow()
       {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainWindow_Loaded);

           // MessageBox.Show(((Key)Properties.Settings.Default.Key1).ToString());
        
            hotKey = new HotKey((Key)Properties.Settings.Default.Key1, convertKeyToInt(Properties.Settings.Default.Key2) | convertKeyToInt(Properties.Settings.Default.Key3), OnHotKeyHandler);

            AppWindow = this;
            if (Properties.Settings.Default.ColorPrimary == "#343a40") new PaletteHelper().SetLightDark(true);
            WindowChrome.SetWindowChrome(this, new WindowChrome());
            SettingsWindow();
        }

        private KeyModifier convertKeyToInt(int key)
        {
            switch (key)
            {
                case 0:
                    {
                        return KeyModifier.Alt;
                    }
                break;
                case 1:
                    {
                        return KeyModifier.None;
                    }
                    break;
                case 2:
                    {
                        return KeyModifier.Ctrl;
                    }
                    break;
                case 3:
                    {
                        return KeyModifier.Shift;
                    }
                    break;
                case 4:
                    {
                        return KeyModifier.Win;
                    }
                    break;
            }
            return KeyModifier.Win;
        }


        public void OnHotKeyHandler(HotKey obj)
        {
            if (this.Visibility == Visibility.Visible)
            {
                foreach (Window window in Application.Current.Windows.OfType<SettingsWindow>())
                {
                    ((SettingsWindow)window).Close();
                }
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
                this.Topmost = true;
                this.Topmost = false;
            }
        }

        private bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(windowFix.WindowProc));
            var windowClipboardManager = new ClipboardManager(this);
            windowClipboardManager.ClipboardChanged += ClipboardChanged;
        }


        private void ComboBoxItem_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (listBox == null) return;
            switch (searchComboBox.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {

                          (listBox.Items[i] as UserControl).Visibility = Visibility.Visible;
                        
                    }
                    break;
                case 1:
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (listBox.Items[i].GetType() == typeof(textControl))
                        {
                            (listBox.Items[i] as UserControl).Visibility = Visibility.Visible;
                        }
                        else { (listBox.Items[i] as UserControl).Visibility = Visibility.Collapsed; }
                    }
                    break;
                case 2:
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (listBox.Items[i].GetType() == typeof(musicControl))
                        {
                            (listBox.Items[i] as UserControl).Visibility = Visibility.Visible;
                        }
                        else { (listBox.Items[i] as UserControl).Visibility = Visibility.Collapsed; }
                    }
                    break;
                case 3:
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (listBox.Items[i].GetType() == typeof(imageControl))
                        {
                            (listBox.Items[i] as UserControl).Visibility = Visibility.Visible;
                        }
                        else { (listBox.Items[i] as UserControl).Visibility = Visibility.Collapsed; }
                    }
                    break;
                case 4:
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (listBox.Items[i].GetType() == typeof(fileControl) || listBox.Items[i].GetType() == typeof(fileListControl))
                        {
                            (listBox.Items[i] as UserControl).Visibility = Visibility.Visible;
                        }
                        else { (listBox.Items[i] as UserControl).Visibility = Visibility.Collapsed; }
                    }
                    break;
            }
        }

        private void ClipboardChanged(object sender, EventArgs e)
        {

            if (OffClipboard == true) return;

            if(listBox.Items.Count == Properties.Settings.Default.MaxElement && Properties.Settings.Default.MaxElement !=0)
            {
                listBox.Items.RemoveAt(listBox.Items.Count-1);
                System.GC.Collect();
            }
            if (listBox.Items.Count > Properties.Settings.Default.MaxElement && Properties.Settings.Default.MaxElement != 0)
            {
                for (int i = Properties.Settings.Default.MaxElement; i < listBox.Items.Count; i++)
                {
                    listBox.Items.RemoveAt(i);
                    System.GC.Collect();
                }
            }
            try
            {
                if (Clipboard.ContainsImage())
                {
                    //TODO NAJPIERW SRAWDZ POŹNNNIEJ STWURZ A I ZOBACZ DO TEJ KONTROLKI BO MASZ TAM ASCYN: OffClipboard
                    //TODO for -> foreach
                    //NIEKTURE ELEMĘTY MAJĄ WSOBIE Clipboard.GetText(TextDataFormat.Text) POBIRZ TO NA POCZĄTKU PUŹNIEJ PODAJ TEKST JAKO PARAMETR
                    //var clip = Clipboard.GetImage();
                    var y = Clipboard.GetText(TextDataFormat.Html);
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (listBox.Items[i].GetType() == typeof(imageControl))
                        {
                            if ((listBox.Items[i] as imageControl).paseToClipboardURL == Regex.Match(y, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value) return;
                        }
                    }
                    imageControl imageControl = new imageControl(Clipboard.GetImage(), y);

                    listBox.Items.Insert(0, imageControl);
                    return;
                }
                if (Clipboard.ContainsText(TextDataFormat.Rtf))
                {
                    var x = Clipboard.GetText(TextDataFormat.Rtf);
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (listBox.Items[i].GetType() == typeof(textControl))
                        {
                            if ((listBox.Items[i] as textControl).paseToClipboard == x) return;
                        }
                    }
                    textControl text = new textControl(x, TextDataFormat.Rtf);
                    //if (listBox.Items.Cast<textControl>().Any(x => x.paseToClipboard == text.paseToClipboard))
                    //return;

                    listBox.Items.Insert(0, text);
                    return;
                }
                if (Clipboard.ContainsText(TextDataFormat.Html))
                {
                    var x = Clipboard.GetText(TextDataFormat.Html);
                    //MessageBox.Show(x);
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (listBox.Items[i].GetType() == typeof(textControl))
                        {
                            if ((listBox.Items[i] as textControl).paseToClipboard == x) return;
                        }
                    }
                    textControl text = new textControl(x, TextDataFormat.Html);

                    listBox.Items.Insert(0, text);

                    return;
                }

                if (Clipboard.ContainsText(TextDataFormat.Text))
                {
                    var x = Clipboard.GetText(TextDataFormat.Text); //TODO NAPRAW 
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (listBox.Items[i].GetType() == typeof(textControl))
                        {
                            if ((listBox.Items[i] as textControl).paseToClipboard.Replace("\r\n", string.Empty) == x.Replace("\r\n", string.Empty)) return;
                        }
                    }
                    textControl text = new textControl(x, TextDataFormat.Text);
                    listBox.Items.Insert(0, text);
                    return;
                }

                if (Clipboard.ContainsFileDropList())
                {
                    var data = Clipboard.GetFileDropList();
                    if (data.Count == 1)
                    {
                        string[] imageExtension = { ".jpg", ".png",".jpeg",".bmp" };
                        string[] musicExtension = { ".mp3", ".wav", ".m4a",".ogg",".flac" };
                        var Extension = System.IO.Path.GetExtension(data[0]);
                        foreach (string x in imageExtension)
                        {
                            if (Extension.ToLower().Contains(x))
                            {
                                for (int i = 0; i < listBox.Items.Count; i++)
                                {
                                    if (listBox.Items[i].GetType() == typeof(imageControl))
                                    {
                                        if ((listBox.Items[i] as imageControl).paseToClipboardFilePath == data[0]) return;
                                    }
                                }
                                imageControl imageControl = new imageControl(data[0]);
                                listBox.Items.Insert(0, imageControl);
                                return;
                            }
                        }
                        foreach (string x in musicExtension)
                        {
                            if (Extension.ToLower().Contains(x))
                            {
                                for (int i = 0; i < listBox.Items.Count; i++)
                                {
                                    if (listBox.Items[i].GetType() == typeof(musicControl))
                                    {
                                        if ((listBox.Items[i] as musicControl).paseToClipboardFilePath == data[0]) return;
                                    }
                                }
                                musicControl musicControl = new musicControl(data[0]);
                                listBox.Items.Insert(0, musicControl);
                                return;
                            }
                        }
                        if (Extension != "")
                        {
                            for (int i = 0; i < listBox.Items.Count; i++)
                            {
                                if (listBox.Items[i].GetType() == typeof(fileControl))
                                {
                                    if ((listBox.Items[i] as fileControl).paseToClipboardFilePath == data[0]) return;
                                }
                            }
                            fileControl fileControl = new fileControl(data[0]);
                            listBox.Items.Insert(0, fileControl);
                        }
                        else
                        {
                            if (Properties.Settings.Default.FileDropList == false) return;
                            for (int i = 0; i < listBox.Items.Count; i++)
                            {
                                if (listBox.Items[i].GetType() == typeof(fileListControl))
                                {
                                    if ((listBox.Items[i] as fileListControl).allFile.Contains(data[0])) return;
                                }
                            }
                            fileListControl fileList = new fileListControl(data);
                            listBox.Items.Insert(0, fileList);
                        }
                    }
                    else
                    {
                        if (Properties.Settings.Default.FileDropList == false) return;
                        for (int i = 0; i < listBox.Items.Count; i++)
                        {
                            if (listBox.Items[i].GetType() == typeof(fileListControl))
                            {
                                if ((listBox.Items[i] as fileListControl).allFile.Contains(data[0])) return;
                            }
                        }
                        fileListControl fileList = new fileListControl(data);
                        listBox.Items.Insert(0, fileList);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Błąd podczas pobierania wartości");
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Update update = new Update();
            update.NewVersion += new AutoUpdate.EventHandler(upDate);
        }
        //UPDATE
        private string FileUpdate="";
        private void upDate(object source, NewVersion e)
        {
            this.Dispatcher.Invoke(() =>
            updateDialog.IsOpen = true);
            FileUpdate = e.GetFileName();
            //MessageBox.Show(e.GetFileName());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(System.IO.Directory.GetCurrentDirectory());
            startDownload();
            updateSlider.Visibility = Visibility.Visible;
        }
        //Search TEXASDASDAWEasd
        private void Search(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].GetType() == typeof(fileListControl))
                {
                }
            }
            //searchTextBox.Text
        }

        private void startDownload()
        {
            if (!IsRunningAsAdministrator())
            {
                // Setting up start info of the new process of the same application
                ProcessStartInfo processStartInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().CodeBase);

                // Using operating shell and setting the ProcessStartInfo.Verb to “runas” will let it run as admin
                processStartInfo.UseShellExecute = true;
                processStartInfo.Verb = "runas";

                // Start the application as new process
                Process.Start(processStartInfo);

                // Shut down the current (old) process
                System.Windows.Application.Current.Shutdown();
            }
            Thread thread = new Thread(() => {


                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadFileAsync(new Uri("http://schowek.c0.pl/application/" + FileUpdate), System.IO.Directory.GetCurrentDirectory() +" /"+ FileUpdate); 
            });
            thread.Start();
        }

        public static bool IsRunningAsAdministrator()
        {
            // Get current Windows user
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();

            // Get current Windows user principal
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);

            // Return TRUE if user is in role "Administrator"
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                //label2.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
                updateSlider.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                Process.Start(System.IO.Directory.GetCurrentDirectory() + "/" + FileUpdate);
                System.Windows.Application.Current.Shutdown();
            });
        }


        private void listBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = GetDescendantByType(sender as ListBox, typeof(ScrollViewer)) as ScrollViewer;
            var verticalScrollBarVisibility = scrollViewer.ComputedVerticalScrollBarVisibility;

            if (verticalScrollBarVisibility == Visibility.Visible)
                scrollVisible.farAs = 35;
            else
                scrollVisible.farAs = 20;              
        }


        public Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null)
            {
                return null;
            }
            if (element.GetType() == type)
            {
                return element;
            }

            Visual foundElement = null;

            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                {
                    break;
                }
            }

            return foundElement;
        }

        public void ShowSnackbar()
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (slider.SelectedIndex == 1)
            slider.SelectedIndex = 0;
            else slider.SelectedIndex = 1;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!Properties.Settings.Default.AppClose)
                this.Visibility = Visibility.Hidden;
            else
                this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #region Setting
        Key newKey;
        WinApi.KeyModifier[] newKeyModifier = new WinApi.KeyModifier[2];
        public void SettingsWindow()
        {
            Loaded += new RoutedEventHandler(SettingsWindow_Loaded);
            Closing += new System.ComponentModel.CancelEventHandler(SettingsWindow_Closing);
            maxImageTextBox.Text = Properties.Settings.Default.ImageMaxWidth.ToString();
            if (Properties.Settings.Default.ColorPrimary == "#343a40") { toggleButtonColor.IsChecked = true; }
            else { toggleButtonColor.IsChecked = false;  }
            toggleButtonText.IsChecked = Properties.Settings.Default.NormalText;
            toggleButtonAppClose.IsChecked = !Properties.Settings.Default.AppClose;
            fileListToggle.IsChecked = Properties.Settings.Default.FileDropList;

            if (Properties.Settings.Default.MaxElement != 0)
                maxElementTB.Text = Properties.Settings.Default.MaxElement.ToString();

            TBkey1.Text = ((Key)Properties.Settings.Default.Key1).ToString();
            TBkey2.SelectedIndex = Properties.Settings.Default.Key2;
            TBkey3.SelectedIndex = Properties.Settings.Default.Key3;

            toggleButtonSystem.IsChecked = Properties.Settings.Default.SystemStart;
        }

        private void SettingsWindow_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Key1 = (int)Enum.Parse(typeof(Key), TBkey1.Text.ToString(), true);
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
            if (int.TryParse(maxImageTextBox.Text, out max))
            {
                if (max > 700 || max < 150)
                {
                    maxImageTextBox.Foreground = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    maxImageTextBox.Foreground = System.Windows.Media.Brushes.Black;
                    //TODO WAŻNE 
                    Properties.Settings.Default.ImageMaxWidth = max;
                }
            }
            else
            {
                maxImageTextBox.Foreground = System.Windows.Media.Brushes.Red;
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
                    texCont.accentText(texCont.paseToClipboard, texCont.textFormat);
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
        }

        private void ToggleButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ColorPrimary = "#16BCE0";
            Properties.Settings.Default.ColorBorder = "white";
            Properties.Settings.Default.ColorWindows = "#ebecec";
            Properties.Settings.Default.ColorFont = "black";
            Properties.Settings.Default.ColorOver = "#06758F";
            new PaletteHelper().SetLightDark(false);
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


        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            MainWindow.AppWindow.hotKey.Unregister();
            MainWindow.AppWindow.hotKey.Dispose();
            if (newKey == Key.None) newKey = (Key)Enum.Parse(typeof(Key), TBkey1.Text.ToString(), true);
            Properties.Settings.Default.Key1 = (int)newKey;
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var value = 0;
            if (int.TryParse(maxElementTB.Text, out value))
            {
                if (value >= 0)
                {
                    Properties.Settings.Default.MaxElement = value;
                    maxElementTB.Foreground = System.Windows.Media.Brushes.Black;
                }
                else
                {
                    maxElementTB.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                maxElementTB.Foreground = System.Windows.Media.Brushes.Red;
            }
            //
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
            System.GC.Collect();
        }




        #endregion

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }


    public class Ref<T> where T : class
    {
        private T instance;
        public Ref(T instance)
        {
            this.instance = instance;
        }

        public static implicit operator Ref<T>(T inner)
        {
            return new Ref<T>(inner);
        }

        public void Delete()
        {
            this.instance = null;
        }

        public T Instance
        {
            get { return this.instance; }
        }
    }

}
