using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HuffmanCoding;
using Microsoft.Win32;

//using Microsoft.Win32;

//using Microsoft.Win32;

namespace CodingMethods
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Code.IsEnabled = false;
            Decode.IsEnabled = false;
        }

        private BinaryTree _lab1;
        private string _text, _text_preob;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileButton.IsEnabled = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == false)
            {
                FileButton.IsEnabled = true;
                return;
            }

            FileButton.IsEnabled = true;
            Code.IsEnabled = true;
            Decode.IsEnabled = false;

            RichTextBox1.Document.Blocks.Clear();
            RichTextBox2.Document.Blocks.Clear();
            tbCode.Text = "";
            tbDecode.Text = "";
            tbCompress.Text = "";

            using (StreamReader reader = new StreamReader(openFileDialog.FileName))
            {
                _text = reader.ReadToEnd();
                if (_text.Length == 0)
                {
                    MessageBox.Show("Введена пустая строка");
                    Decode.IsEnabled = false;
                    Code.IsEnabled = false;
                }
                RichTextBox1.Document.Blocks.Add(new Paragraph(new Run(_text)));
            }
        }

        private void CompressionRatio()
        {
            tbCompress.Text = ((double) (_text.Length * 8) / _text_preob.Length).ToString("F4");
        }

        private void Code_Click(object sender, RoutedEventArgs e)
        {
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    Code.IsEnabled = false;
                    Decode.IsEnabled = false;
                    FileButton.IsEnabled = false;

                    Stopwatch sp = new();
                    sp.Start();
                    _lab1 = new();
                    _text_preob = _lab1.Encode(_text);
                    sp.Stop();
                    tbCode.Text = sp.ElapsedMilliseconds.ToString();
                    CompressionRatio();
                    //RichTextBox1.Text = _text_preob;
                    RichTextBox2.Document.Blocks.Clear();
                    RichTextBox2.Document.Blocks.Add(new Paragraph(new Run(_text_preob)));

                    FileButton.IsEnabled = true;
                    Decode.IsEnabled = true;
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    MessageBox.Show("Выберете метод кодирования");
                    break;
            }
        }

        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    Code.IsEnabled = false; 
                    Decode.IsEnabled = false;
                    FileButton.IsEnabled = false;

                    Stopwatch sp = new();
                    sp.Start();
                    var text = _lab1.Decode(_text_preob);
                    sp.Stop();
                    tbDecode.Text = sp.ElapsedMilliseconds.ToString();

                    RichTextBox2.Document.Blocks.Clear();
                    RichTextBox2.Document.Blocks.Add(new Paragraph(new Run(text)));

                    FileButton.IsEnabled = true;
                    Code.IsEnabled = true;
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //clean reachbox2 because choose new method coding
        }
    }
}
