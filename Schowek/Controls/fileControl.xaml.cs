using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schowek.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy fileControl.xaml
    /// </summary>

    /// <see>
    ///  Śledzenie pliku jeśli jego położenie się zmieni zmienić scieżke pliku
    /// </see>

    public partial class fileControl : UserControl
    {
        private System.Windows.Media.ImageSource icon;
        public string paseToClipboardFilePath;
        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        public fileControl(string filePath)
        {
            paseToClipboardFilePath = filePath;
            InitializeComponent();
            filePathTextBlock.Text = System.IO.Path.GetFileName(filePath);
            fileIconImage.Source = Icon;
            fileSizeTextBlock.Text = GetFileSize(filePath);
            
            
            fileSystemWatcher.Path = System.IO.Path.GetDirectoryName(filePath);
            fileSystemWatcher.Filter = System.IO.Path.GetFileName(filePath);
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Deleted += deletedFile;
            fileSystemWatcher.Renamed += renamedFile;
        }

        private void renamedFile(object sender, FileSystemEventArgs e)
        {
            paseToClipboardFilePath = e.FullPath;
            fileSystemWatcher.Path = System.IO.Path.GetDirectoryName(paseToClipboardFilePath);
            fileSystemWatcher.Filter = System.IO.Path.GetFileName(paseToClipboardFilePath);
        }

        private void deletedFile(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                MainWindow.AppWindow.listBox.Items.Remove(this);
            });
        }

        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                if (icon == null)
                {
                    using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(paseToClipboardFilePath))
                    {
                        icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                  sysicon.Handle,
                                  System.Windows.Int32Rect.Empty,
                                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    }
                }

                return icon;
            }
        }

        public string GetFileSize(string filePath)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = new FileInfo(filePath).Length;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }


        private  void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppWindow.listBox.Items.Remove(this);
            System.GC.Collect();
        }

        private async void RippleEffect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.AppWindow.ShowSnackbar();
            if (!File.Exists(paseToClipboardFilePath))
            {
                return;
            }
            try
            {
                MainWindow.OffClipboard = true;
                StringCollection collection = new StringCollection();
                collection.Add(paseToClipboardFilePath);
                Clipboard.SetFileDropList(collection);
                await Task.Delay(100);
                MainWindow.OffClipboard = false;
            }
            catch
            {

            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (File.Exists(paseToClipboardFilePath))
            {
                 Process.Start(paseToClipboardFilePath);
            }
        }
    }
}
