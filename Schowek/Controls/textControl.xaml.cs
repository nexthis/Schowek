using HTMLConverter;
using Schowek.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Schowek.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy textControl.xaml
    /// </summary>
    public partial class textControl : UserControl
    {

        public string paseToClipboard;
        public string normalText;
        public TextDataFormat textFormat;
        public textControl(string text,TextDataFormat format)
        {
            InitializeComponent();
            paseToClipboard = text;
            try { normalText = Clipboard.GetText(); }
            catch { normalText = "problem przy kopiowaniu przepraszam ;-)"; }

            textFormat = format;
            if(Properties.Settings.Default.NormalText == true)
            {
                standardText();
                return;
            } 
            accentText(text, format);
            int numLines = text.Length - text.Replace(Environment.NewLine, string.Empty).Length;


        }
    
        



        private void RippleEffectDecorator_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.AppWindow.ShowSnackbar();
            //paseToClipboard & textFormat
            //TODU dokończ to co zacząłeś  && modus operandi ?? 
            TextRange textRange = new TextRange(
                richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd
            );
            try
            {
                var dataObject = new DataObject();
                if (textFormat == TextDataFormat.Html)
                    dataObject.SetData(DataFormats.Html, paseToClipboard);
                if (textFormat == TextDataFormat.Rtf)
                    dataObject.SetData(DataFormats.Rtf, paseToClipboard);
                dataObject.SetData(DataFormats.Text, textRange.Text);
                Clipboard.SetDataObject(dataObject, true);
            }
            catch
            { }

        }

        private FlowDocument SetRTF(string xamlString)
        {
            StringReader stringReader = new StringReader(xamlString);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            Section sec = XamlReader.Load(xmlReader) as Section;
            FlowDocument doc = new FlowDocument();
            while (sec.Blocks.Count > 0)
                doc.Blocks.Add(sec.Blocks.FirstBlock);
            return doc;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //TODO CZYSZCZENIE RAMU !!
            MainWindow.AppWindow.listBox.Items.Remove(this);
            System.GC.Collect();

        }
        private int time =30; 
        DispatcherTimer timer = new DispatcherTimer();
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            
            if(time < 30)
            {
                timer.Stop();
                use.Header = "Użyj";
                time = 30;
                Panel.SetZIndex(RippleEffect, 2);
                return;
            }
            Action<object, EventArgs> action = (object send, EventArgs f) =>
             {
                 time -= 1;
                 use.Header = "Użyj (" + time + ")";
                 if (time == 0) {
                     Panel.SetZIndex(RippleEffect, 2);
                     time = 30;
                     timer.Stop();
                     use.Header = "Użyj";
                 }
             };
            Panel.SetZIndex(RippleEffect, 0);
            
            timer.Tick += new EventHandler(action);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }



        private void richTextBox_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = (System.Windows.Documents.Hyperlink) e.Source;
            Process.Start(hyperlink.NavigateUri.ToString()); 
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            standardText();
        }

        public void accentText(string text, TextDataFormat format)
        {
            if (format == TextDataFormat.Html)
            {
                text = text.Substring(text.IndexOf('<'));
                StringBuilder sb = new StringBuilder();
                sb.Append(HtmlToXamlConverter.ConvertHtmlToXaml(text,true));
                richTextBox.Document = (FlowDocument)XamlReader.Parse(sb.ToString());
            }
            if (format == TextDataFormat.Rtf)
            {
                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(text));
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.Load(stream, DataFormats.Rtf);
            }
            if (format == TextDataFormat.Text)
            {
                richTextBox.AppendText(text);
            }
        }

        public void standardText()
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.AppendText(normalText);
        }

    }



}
