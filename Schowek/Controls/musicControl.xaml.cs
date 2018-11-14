using NAudio.Wave;
using Schowek.NAudioClass;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Threading;

namespace Schowek.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy musicControl.xaml
    /// </summary>
    public partial class musicControl : UserControl
    {
        private IWavePlayer playbackDevice;
        private WaveStream fileStream;
        DispatcherTimer timer = new DispatcherTimer();
        public string paseToClipboardFilePath;
        public musicControl(string filePath)
        {
            InitializeComponent();
            Load(filePath);
            musicName.Text = System.IO.Path.GetFileNameWithoutExtension(filePath);
            paseToClipboardFilePath = filePath;
        }

        public void Load(string fileName)
        {
            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFile(fileName);
        }

        private void CloseFile()
        {
            fileStream?.Dispose();
            fileStream = null;
        }
        //TODU Look here

        private void OpenFile(string fileName)
        {
            try
            {
                var inputStream = new AudioFileReader(fileName);
                fileStream = inputStream;
                var aggregator = new SampleAggregator(inputStream);
                aggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
                aggregator.PerformFFT = true;
                playbackDevice.Init(aggregator);
                minutesPosition.Text = fileStream.TotalTime.Minutes.ToString();
                secondsPosition.Text = fileStream.TotalTime.Seconds.ToString("00");
                slider.Maximum = fileStream.TotalTime.TotalSeconds;
                minutesAcyualPosition.Text = "0";
                secondsAcyualPosition.Text = "00";
                Action<object, EventArgs> action = (object send, EventArgs f) =>
                {
                    try
                    {
                        slider.Value = fileStream.CurrentTime.TotalSeconds;
                        minutesAcyualPosition.Text = fileStream.CurrentTime.Minutes.ToString();
                        secondsAcyualPosition.Text = fileStream.CurrentTime.Seconds.ToString("00");
                        timeTest();
                    }
                    catch
                    {

                    }
                };
                timer.Tick += new EventHandler(action);
                timer.Interval = TimeSpan.FromSeconds(1);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Problem opening file");
                CloseFile();
            }
        }

        private void EnsureDeviceCreated()
        {
            if (playbackDevice == null)
            {
                CreateDevice();
            }
        }

        private void CreateDevice()
        {
            playbackDevice = new WaveOut { DesiredLatency = 200 };
        }

        public void Play()
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {

                foreach (var item in MainWindow.AppWindow.listBox.Items)
                {
                    if(item.GetType() == typeof(musicControl)) {
                        (item as musicControl).Pause();
                        (item as musicControl).playButton.IsChecked = false;
                    }
                }
                playbackDevice.Play();
                playButton.IsChecked = true;
            }
        }

        public void Pause()
        {
            playbackDevice?.Pause();
        }

        public void Stop()
        {
            playbackDevice?.Stop();
            if (fileStream != null)
            {
                fileStream.Position = 0;
            }
        }

        public void Dispose()
        {
            Stop();
            CloseFile();
            playbackDevice?.Dispose();
            playbackDevice = null;
        }

        private void timeTest()
        {
            if (fileStream.CurrentTime == fileStream.TotalTime)
            {
                Stop();
                playButton.IsChecked = false;
                minutesAcyualPosition.Text = "0";
                secondsAcyualPosition.Text = "00";
                slider.Value = 0;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            timeTest();
            Play();
            timer.Start();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            timeTest();
            Pause();
            timer.Stop();
        }

        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            timer.Start();
            fileStream.Position = ((long)(slider.Value * fileStream.WaveFormat.AverageBytesPerSecond));
            //MessageBox.Show("KURWA JESTEM ZAJEBISTY");
        }

        private void slider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            TimeSpan time = new TimeSpan(0, 0, (int)slider.Value);
            minutesAcyualPosition.Text = time.Minutes.ToString();
            secondsAcyualPosition.Text = time.Seconds.ToString("00");
        }

        private void slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            timer.Stop();
        }

       

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Dispose();
            MainWindow.AppWindow.listBox.Items.Remove(this);
            System.GC.Collect();
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.AppWindow.ShowSnackbar();
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
    }
}
