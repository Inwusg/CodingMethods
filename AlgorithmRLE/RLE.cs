using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AlgorithmRLE
{
    public class RLE
    {
        private int numOrigStr = -1;
        public (string, int) BurrowsTransform(string str)
        {
            int length = str.Length;
            StringBuilder[] mas = new StringBuilder[length];
            mas[0] = new StringBuilder(str);
            for (int i = 1; i < length; i++)
            {
                mas[i] = new StringBuilder("").Append(mas[i - 1]);
                mas[i].Append(str[i - 1]);
                mas[i].Remove(0, 1);
            }

            string[] mas2 = new string[length];
            for (int i = 0; i < length; i++) mas2[i] = mas[i].ToString();

            var ordered = from item in mas2 orderby item select item;

            StringBuilder outStr = new("");
            int k = 0;
            foreach (var sortStr in ordered)
            {
                outStr.Append(sortStr[length - 1]);
                if (sortStr.Equals(str)) numOrigStr = k;
                k++;
            }

            return (outStr.ToString(), numOrigStr);
        }

        public int sizeCompressText;

        public string Encode(string str)
        {
            sizeCompressText = 0;
            int max_k = -1;
            int size = 0;
            str = BurrowsTransform(str).Item1;

            StringBuilder outStr = new StringBuilder("");
            int k = 1;
            for (int i = 1; i <= str.Length; i++)
            {
                if (i != str.Length && str[i - 1] == str[i]) k++;
                else
                {
                    outStr.Append(str[i - 1]);
                    outStr.Append("*");
                    outStr.Append(k);
                    if (k > max_k) max_k = k;
                    size++;
                    k = 1;
                }
            }

            int b1 = 2;
            int k1 = 1;
            while (max_k > b1)
            {
                b1 *= 2;
                k1++;
            }

            sizeCompressText = (k1 + 8) * size;
            //return str;
            return outStr.ToString();
        }

        public string Decode(string str)
        {
            StringBuilder strDecode = new("");
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '*')
                {
                    int k = 0;
                    StringBuilder number = new("");
                    for (int j = i + 1; j < str.Length; j++)
                    {
                        if (str[j] == '*')
                        {
                            number.Remove(number.Length - 1, 1);
                            k--;
                            break;
                        }
                        else
                        {
                            k++;
                            number.Append(str[j]);
                        }
                    }
                    int len = int.Parse(number.ToString());
                    for (int j = 0; j < len; j++)
                    {
                        strDecode.Append(str[i - 1]);
                    }
                    i += k;
                }
            }

            string s = strDecode.ToString();
            return ReversBurrowsTransform(s);

        }



        public string ReversBurrowsTransform(string str)
        {
            //StringBuilder[] mas_sb = new StringBuilder[n];
            //for (int i = 0; i < n; i++) mas_sb[i] = new StringBuilder("");
            //for (int j = 0; j < n; j++)
            //{
            //    for (int i = 0; i < n; i++)
            //    {
            //        mas_sb[i].Insert(0, s[i]);
            //    }
            //    string[] mas2 = new string[n];
            //    for (int i = 0; i < n; i++) mas2[i] = mas_sb[i].ToString();

            //    var ordered = from item in mas2 orderby item select item;
            //    int k = 0;
            //    foreach (var item in ordered)
            //    {
            //        mas_sb[k] = new StringBuilder(item);
            //        k++;
            //    }
            //}
            //return mas_sb[numOrigStr].ToString();

            //fast algorithm
            int n = str.Length;
            int m = 60000;


            int[] count = new int[m];
            for (int i = 0; i < m; i++) count[i] = 0;
            for (int i = 0; i < n; i++) count[str[i]]++;
            int sum = 0;
            for (int i = 0; i < m; i++)
            {
                sum += count[i];
                count[i] = sum - count[i];
            }

            int[] t = new int[n];
            for (int i = 0; i < n; i++) t[i] = 0;
            for (int i = 0; i < n; i++)
            {
                t[count[str[i]]] = i;
                count[str[i]]++;
            }
            

            int pos = t[numOrigStr];
            StringBuilder answer = new("");
            for (int i = 0; i < n; i++)
            {
                answer.Append(str[pos]);
                pos = t[pos];
            }
            return answer.ToString();
        }

    }
}