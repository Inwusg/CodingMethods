using System.Text;

namespace HuffmanCoding
{
    public class BinaryTree : IComparable<BinaryTree>
    {
        private Node node;
        private Dictionary<char, int> frequency = new Dictionary<char, int>(); // символ - частота
        private Dictionary<char, string> code = new Dictionary<char, string>(); // символ - код
        private Dictionary<string, char> decode = new Dictionary<string, char>(); // код - символ
        private List<Node> Tree = new List<Node>();
        private List<int> kodir = new List<int>();
        public BinaryTree()
        {

        }
        private void TreeAdd()
        {
            foreach (var item in frequency)
            {
                Tree.Add(new Node(item.Key, item.Value, null, null));
            }

            if (Tree.Count == 1) kodir.Add(0);
        }
        private void Build()
        {

            while (Tree.Count > 1)
            {
                //Tree = Tree.OrderBy(x => x.Frequency).ToList();
                Node n = Tree.Min();
                Tree.Remove(n);
                Node n1 = Tree.Min();
                Tree.Remove(n1);
                Tree.Add(new Node(' ', n.Frequency + n1.Frequency, n, n1));
                //int f = Tree[0].Frequency = Tree[1].Frequency;
                //Node n = new Node('*', f, Tree[0], Tree[1]);
                //Tree.RemoveAt(0);
                //Tree.RemoveAt(0);
                //Tree.Add(n);
            }
            decode.Clear();
            code.Clear();
        }
        private void Kod(Node nod)
        {
            if (nod.LeftBaby == null && nod.RightBaby == null)
            {
                string str = "";
                foreach (var s in kodir)
                {
                    str += s;
                }
                code.Add(nod.Symbol, str);
                decode.Add(str, nod.Symbol);
            }
            else
            {
                if (nod.LeftBaby != null)
                {
                    Node n = nod.LeftBaby;
                    nod.LeftBaby = null;
                    kodir.Add(0);
                    Kod(n);
                    if (kodir.Count > 0) kodir.RemoveAt(kodir.Count - 1);
                }
                if (nod.RightBaby != null)
                {
                    Node n = nod.RightBaby;
                    nod.RightBaby = null;
                    kodir.Add(1);
                    Kod(n);
                    if (kodir.Count > 0) kodir.RemoveAt(kodir.Count - 1);
                }
            }
        }
        public string Decode(string input)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder cod = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                cod.Append(input[i]);
                if (decode.ContainsKey(cod.ToString()))
                {
                    result.Append(decode[cod.ToString()]);
                    cod.Clear();
                }

            }
            return result.ToString();
        }
        public string Encode(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (frequency.ContainsKey(input[i])) frequency[input[i]]++;
                else frequency.Add(input[i], 1);
            }
            frequency = frequency.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);  //по возрастанию 
            //tree.frequency = tree.frequency.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value); //по убыванию

            TreeAdd();
            Build();
            Kod(Tree[0]);

            StringBuilder result = new StringBuilder();
            foreach (char symbol in input)
            {
                result.Append(code[symbol]);
            }
            return result.ToString();
        }

        public int CompareTo(BinaryTree other)
        {
            return node.Frequency.CompareTo(other.node.Frequency);


        }
    }
}