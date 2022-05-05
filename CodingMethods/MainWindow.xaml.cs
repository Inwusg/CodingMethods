using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
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
using AlgorithmLZ77;
using AlgorithmRLE;
using ArithmeticCoding;
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
        private Arithmetic _lab3;
        private RLE _lab4;
        private LZ77 _lab5;
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

        private void CompressionRatio2()
        {
            tbCompress.Text = ((double) (_text.Length * 8) / (_lab4.sizeCompressText)).ToString("F4");
        }

        private void CompressionRatio3()
        {
            List<(int, int, char)> ans = _lab5._ans;
            int a1 = -1;
            int a2 = -1;
            foreach (var item in ans)
            {
                if (item.Item1 > a1) a1 = item.Item1;
                if (item.Item1 > a2) a2 = item.Item2;
            }

            int b1 = 2;
            int k1 = 1;
            while (a1 > b1)
            {
                b1 *= 2;
                k1++;
            }

            int b2 = 2;
            int k2 = 1;
            while (a2 > b2)
            {
                b2 *= 2;
                k2++;
            }

            int size = k1 + k2 + 8; //длина на один блок
            int kol = 0;
            for (int i = 0; i < _text_preob.Length; i++)
            {
                if (_text_preob[i] == '(') kol++;
            }

            tbCompress.Text = ((double) (_text.Length * 8) / (kol * size)).ToString("F4");
        }


        private void CompressionRatio4()
        {
            //throw new NotImplementedException();
        }

        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH",
            MessageId = "type: System.Char[]; size: 56MB")]
        private Fraction fr = new Fraction();
        private void Code_Click(object sender, RoutedEventArgs e)
        {
            Code.IsEnabled = false;
            Decode.IsEnabled = false;
            FileButton.IsEnabled = false;

            Stopwatch sp = new();
            sp.Start();
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    _lab1 = new();
                    _text_preob = _lab1.Encode(_text);
                    CompressionRatio();
                    break;
                case 1:
                    _lab2 = new();
                    _text_preob = _lab2.Code(_text);
                    CompressionRatio();
                    break;
                case 2:
                    _lab3 = new();
                    fr = _lab3.Encode(_text);
                    _text_preob = fr.ToString();
                    CompressionRatio4();
                    break;
                case 3:
                    _lab4 = new();
                    //_text_preob = _lab4.BurrowsTransform(_text).ToString();
                    _text_preob = _lab4.Encode(_text);
                    CompressionRatio2();
                    break;
                case 4:
                    _lab5 = new();
                    _text_preob = _lab5.Encode(_text);
                    CompressionRatio3();
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    MessageBox.Show("Выберете метод кодирования");
                    sp.Stop();
                    FileButton.IsEnabled = true;
                    Code.IsEnabled = true;
                    Decode.IsEnabled = false;

                    RichTextBox2.Document.Blocks.Clear();
                    tbCode.Text = "";
                    tbDecode.Text = "";
                    tbCompress.Text = "";
                    return;
            }

            sp.Stop();
            tbCode.Text = sp.ElapsedMilliseconds.ToString();
            
            RichTextBox2.Document.Blocks.Clear();
            RichTextBox2.Document.Blocks.Add(new Paragraph(new Run(_text_preob)));

            FileButton.IsEnabled = true;
            Decode.IsEnabled = true;

        }

        

        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String; size: 437MB")]
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.Char[]; size: 168MB")]
        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            Code.IsEnabled = false;
            Decode.IsEnabled = false;
            FileButton.IsEnabled = false;
            string text = "";
            Stopwatch sp = new();
            sp.Start();
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    text = _lab1.Decode(_text_preob);
                    break;
                case 1:
                    text = _lab2.Decode(_text_preob);
                    break;
                case 2:
                    text = _lab3.Decode(fr);
                    break;
                case 3:
                    text = _lab4.Decode(_text_preob);
                    break;
                case 4:
                    text = _lab5.Decode(_text_preob);
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }
            sp.Stop();
            tbDecode.Text = sp.ElapsedMilliseconds.ToString();

            RichTextBox2.Document.Blocks.Clear();
            RichTextBox2.Document.Blocks.Add(new Paragraph(new Run(text)));

            FileButton.IsEnabled = true;
            Code.IsEnabled = true;
        }

        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.Char[]; size: 84MB")]
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //clean reachbox2 because choose new method coding
            FileButton.IsEnabled = true;
            Code.IsEnabled = true;
            Decode.IsEnabled = false;

            RichTextBox2.Document.Blocks.Clear();
            tbCode.Text = "";
            tbDecode.Text = "";
            tbCompress.Text = "";
        }
    }
}
