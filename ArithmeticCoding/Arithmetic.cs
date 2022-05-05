using System.Numerics;
using System.Text;

namespace ArithmeticCoding
{
    public struct Fraction
    {
        public readonly BigInteger num;
        public readonly BigInteger den;

        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }

            for (int i = 2; i < numerator;)
            {
                if (i > 10000) break;
                if (numerator % i == 0 && denominator % i == 0)
                {
                    numerator = numerator / i;
                    denominator = denominator / i;
                }
                else i++;
            }
            num = numerator;
            den = denominator;
        }

        public static Fraction operator +(Fraction a) => a;
        public static Fraction operator -(Fraction a) => new Fraction(-a.num, a.den);

        public static Fraction operator +(Fraction a, Fraction b)
        {
            if (a.den != b.den) return new Fraction(a.num * b.den + b.num * a.den, a.den * b.den);
            return new Fraction(a.num + b.num, a.den);
        }

        public static Fraction operator -(Fraction a, Fraction b)
            => a + (-b);

        public static Fraction operator *(Fraction a, Fraction b)
            => new Fraction(a.num * b.num, a.den * b.den);

        public static bool operator <(Fraction a, Fraction b)
            => (a.num * b.den) < (b.num * a.den);
        public static bool operator <=(Fraction a, Fraction b)
            => (a.num * b.den) <= (b.num * a.den);
        public static bool operator >(Fraction a, Fraction b)
            => (a.num * b.den) > (b.num * a.den);
        public static bool operator >=(Fraction a, Fraction b)
            => (a.num * b.den) >= (b.num * a.den);

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.num == 0)
            {
                throw new DivideByZeroException();
            }
            return new Fraction(a.num * b.den, a.den * b.num);
        }

        public override string ToString() => $"{num} / {den}";
        //public override string ToString() => $"{(double)num / den}";
    }

    public class Node
    {
        public char Symbol { get; set; }
        public Fraction Low { get; set; }
        public Fraction High { get; set; }
        //public double Range { get; set; }
        public int Frequency { get; set; } //Количество данного символа в тексте
    }

    public class Arithmetic
    {
        private List<Node> nodes = new List<Node>();
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();
        private int len;

        private void Build(string source)
        {
            //Добавляем в словарь, тем самым считаем количество одинаковых букав
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i]))
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
            }
            //Добавляем сиволы в лист узлов и сортируем по количеству встречаемости символа
            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            nodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();
            nodes.Reverse();
        }

        private List<Node> GetSymbolsRanges(string source)
        {
            Fraction low = new Fraction(0, 1);
            foreach (Node node in nodes)
            {
                Fraction range = new Fraction(node.Frequency, source.Length); //длина отрезка данного символа (вероятность)
                node.Low = low; //начало отрезка
                node.High = low + range; //конец отрезка
                low += range;
            }
            return nodes;
        }

        public Fraction Encode(string source)
        {
            Build(source);
            GetSymbolsRanges(source);

            len = source.Length;
            List<Node> allNodes = new List<Node>();

            for (int i = 0; i < source.Length; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (source[i] == nodes[j].Symbol)
                    {
                        allNodes.Add(new Node() { Symbol = nodes[j].Symbol, Low = nodes[j].Low, High = nodes[j].High });
                        break;
                    }
                }
            }

            for (int i = 1; i < allNodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (allNodes[i].Symbol == nodes[j].Symbol)
                    {
                        allNodes[i].High = allNodes[i - 1].Low + (allNodes[i - 1].High - allNodes[i - 1].Low) * nodes[j].High;
                        allNodes[i].Low = allNodes[i - 1].Low + (allNodes[i - 1].High - allNodes[i - 1].Low) * nodes[j].Low;
                        break;
                    }
                }
            }

            return allNodes[allNodes.Count - 1].Low;
        }

        public string Decode(Fraction fraction)
        {
            StringBuilder result = new("");
            while (true)
            {
                if (result.Length >= len) break;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Low <= fraction && nodes[i].High > fraction)
                    {
                        result.Append(nodes[i].Symbol);
                        fraction = (fraction - nodes[i].Low) / (nodes[i].High - nodes[i].Low);
                        break;
                    }
                }
            }
            return result.ToString();
        }
    }
}