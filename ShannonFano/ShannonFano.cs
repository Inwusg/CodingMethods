using System.Text;

namespace ShannonFano
{
    public class ShannonFanoAlg
    {
        private Dictionary<char, int> _frequency;
        private char[] _chars;
        private Dictionary<char, string> _code;
        private Dictionary<string, char> _decode;
        public ShannonFanoAlg()
        {
            _frequency = new();
            _code = new();
            _decode = new();
        }
        public string Encode(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (_frequency.ContainsKey(input[i])) _frequency[input[i]]++;
                else _frequency.Add(input[i], 1);
            }
            //_frequency = _frequency.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);  //по возрастанию 
            _frequency = _frequency.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value); //по убыванию

            int sumver = 0;
            int k = 0;
            _chars = new char[_frequency.Count];
            foreach (var item in _frequency.Keys)
            {
                _chars[k] = item;
                sumver += _frequency[item];
                k++;
            }
            GetCodeSimvol(0, _frequency.Count(), new StringBuilder(), sumver);

            StringBuilder result = new StringBuilder();
            foreach (char symbol in input)
            {
                result.Append(_code[symbol]);
            }
            return result.ToString();
        }

        public string Decode(string input)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder cod = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                cod.Append(input[i]);
                if (_decode.ContainsKey(cod.ToString()))
                {
                    result.Append(_decode[cod.ToString()]);
                    cod.Clear();
                }

            }
            return result.ToString();
        }

        private void GetCodeSimvol(int indexStart, int indexEnd, StringBuilder code, int sumver)
        {
            if (indexEnd - indexStart <= 1)
            {
                _code.Add(_chars[indexStart], code.ToString());
                _decode.Add(code.ToString(), _chars[indexStart]);
                return;
            }

            //get mid
            int sumver_polovina = sumver / 2;
            int new_sumver = 0;
            int old_sumver = -1;
            int indexmid = -1;
            for (int i = indexStart; i < indexEnd; i++)
            {
                new_sumver += _frequency[_chars[i]];
                indexmid = i;

                if (new_sumver >= sumver_polovina)
                {
                    if (old_sumver != -1)
                    {
                        if (new_sumver - sumver_polovina > sumver_polovina - old_sumver)
                        {
                            new_sumver = old_sumver;
                            indexmid--;
                        }
                    }
                    else
                    {
                        //new_sumver = sumver_polovina;

                    }
                    break;
                }

                old_sumver = new_sumver;
            }


            StringBuilder sb1 = new StringBuilder(code.ToString());
            StringBuilder sb2 = new StringBuilder(code.ToString());

            GetCodeSimvol(indexStart, indexmid + 1 , sb1.Append("0"), new_sumver);


            GetCodeSimvol(indexmid + 1, indexEnd, sb2.Append("1"), sumver - new_sumver);

            //recurs calculation 
        }

    }
}