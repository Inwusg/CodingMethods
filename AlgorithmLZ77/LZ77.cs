using System.Text;

namespace AlgorithmLZ77
{
    //public class Node
    //{
    //    public int Offset { get; set; }
    //    public int Length { get; set; }
    //    public char Next { get; set; }

    //    public Node(int offset, int length, char next)
    //    {

    //    }
    //}
    public class LZ77
    {
        private StringBuilder _buffer;
        private string _str;
        public List<(int, int, char)> _ans;
        public string Encode(string str)
        {
            _buffer = new StringBuilder("");
            _str = str;
            List<(int, int, char)> ans = new();
            int pos = 0;
            while (pos < str.Length)
            {
                int offset = 0;
                int lenght = 0;
                (offset, lenght) = findMatching(pos);
                pos += lenght;
                if (pos >= str.Length) ans.Add((offset, lenght, '$'));
                else ans.Add((offset, lenght, str[pos]));
                pos++;
            }

            StringBuilder result = new("");
            foreach (var item in ans)
            {
                result.Append($"({item.Item1},{item.Item2},{item.Item3})");
            }

            _ans = ans;
            return result.ToString();
        }

        private (int, int) findMatching(int pos)
        {
            StringBuilder str = new("");
            for (int i = pos; i < _str.Length; i++)
            {
                str.Append(_str[i]);
                int index = _buffer.ToString().IndexOf(str.ToString());
                if (index == -1)
                {
                    if (i == pos)
                    {
                        _buffer.Append(_str[pos]);
                        return (0, 0);
                    }
                    str.Remove(str.Length - 1, 1);
                    index = _buffer.ToString().IndexOf(str.ToString());
                    int oldLenBuf = _buffer.Length;
                    for (int j = pos; j <= i; j++) _buffer.Append(_str[j]);
                    return (oldLenBuf - index, str.Length);
                }

                if (i == _str.Length - 1)
                {
                    index = _buffer.ToString().IndexOf(str.ToString());
                    int oldLenBuf = _buffer.Length;
                    return (oldLenBuf - index, str.Length);
                }

            }

            return (-1, -1);
        }

        public string Decode(string str)
        {
            //Encode(str);
            StringBuilder result = new("");
            foreach (var item in _ans)
            {
                result.Append(result.ToString(), result.Length - item.Item1, item.Item2);
                if (item.Item3 != '$') result.Append(item.Item3);
            }

            return result.ToString();
        }

    }
}