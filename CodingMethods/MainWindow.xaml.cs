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
using ArithmeticCodingAlgorithm;
using HuffmanCoding;
using Microsoft.Win32;
using ShannonFano;

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
        private ShannonFanoAlg _lab2;
        //private ArithmeticCoding _lab3;
        private ArifmCode _lab3;
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
            int selectedIndex = ComboBox.SelectedIndex;
            Stopwatch sp = new();
            if (selectedIndex <= 6 && selectedIndex >= 0)
            {
                Code.IsEnabled = false;
                Decode.IsEnabled = false;
                FileButton.IsEnabled = false;
                sp.Start();
            }
            switch (selectedIndex)
            {
                case 0:
                    _lab1 = new();
                    _text_preob = _lab1.Encode(_text);
                    break;
                case 1:
                    _lab2 = new();
                    _text_preob = _lab2.Encode(_text);
                    break;
                case 2:
                    _lab3 = new();
                    _text_preob = _lab3.Encode(_text);
                    //_text_preob = _lab3.Code(_text);
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
            if (selectedIndex <= 6 && selectedIndex >= 0)
            {
                sp.Stop();
                tbCode.Text = sp.ElapsedMilliseconds.ToString();
                CompressionRatio();
                RichTextBox2.Document.Blocks.Clear();
                RichTextBox2.Document.Blocks.Add(new Paragraph(new Run(_text_preob)));
                FileButton.IsEnabled = true;
                Decode.IsEnabled = true;
            }
        }

        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ComboBox.SelectedIndex;
            Stopwatch sp = new();
            string text = "";
            if (selectedIndex <= 6 && selectedIndex >= 0)
            {
                Code.IsEnabled = false;
                Decode.IsEnabled = false;
                FileButton.IsEnabled = false;
                sp.Start();
            }
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    text = _lab1.Decode(_text_preob);
                    break;
                case 1:
                    text = _lab2.Decode(_text_preob);
                    break;
                case 2:
                    text = _lab3.Decode(_text_preob);
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
            if (selectedIndex <= 6 && selectedIndex >= 0)
            {
                sp.Stop();
                tbDecode.Text = sp.ElapsedMilliseconds.ToString();
                RichTextBox2.Document.Blocks.Clear();
                RichTextBox2.Document.Blocks.Add(new Paragraph(new Run(text)));
                FileButton.IsEnabled = true;
                Code.IsEnabled = true;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Decode.IsEnabled)
            {
                Decode.IsEnabled = false;
                Code.IsEnabled = true;
                RichTextBox2.Document.Blocks.Clear();
                tbCode.Text = "";
                tbDecode.Text = "";
                tbCompress.Text = "";
            }
        }
    }
}
