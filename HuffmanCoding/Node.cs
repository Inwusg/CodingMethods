using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCoding
{
    internal class Node : IComparable<Node>
    {
        public char Symbol;
        public int Frequency;
        public Node LeftBaby;
        public Node RightBaby;
        public Node(char s, int f, Node l, Node r)
        {
            Symbol = s;
            Frequency = f;
            LeftBaby = l;
            RightBaby = r;
        }

        public int CompareTo(Node other)
        {
            return Frequency.CompareTo(other.Frequency);
        }

    }
}
