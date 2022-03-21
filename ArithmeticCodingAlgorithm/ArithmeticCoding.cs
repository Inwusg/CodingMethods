namespace ArithmeticCodingAlgorithm
{
    public class ArithmeticCoding
    {
        private Dictionary<char, int> _frequency;
        private Dictionary<char, int> _segments;
        private int _lengthSegment;
        private Dictionary<char, string> _code;
        private Dictionary<string, char> _decode;
        public ArithmeticCoding()
        {
            _frequency = new();
            _segments = new();
            _lengthSegment = 0;
            _code = new();
            _decode = new();
        }

        public string Code(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (_frequency.ContainsKey(input[i])) _frequency[input[i]]++;
                else _frequency.Add(input[i], 1);
            }
            //_frequency = _frequency.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);  //по возрастанию 
            _frequency = _frequency.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value); //по убыванию
            _lengthSegment = 0;
            foreach (var item in _frequency.Keys)
            {
                _segments.Add(item, _lengthSegment + _frequency[item]);
                _lengthSegment += _frequency[item];
            }



            return String.Empty;
        }

        public string Decode(string iput)
        {
            return String.Empty;
        }


    }

}