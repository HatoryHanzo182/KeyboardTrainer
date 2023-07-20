using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardTrainer.Services
{
    public class TextGenerator
    {
        static readonly String _available_values = "123 4567 890Q W ERTY UIOP ASDFG HJKL ZXCVB NMqw e rtyui opasdf ghj klz xcv bnm";
        Random _rand = new Random();
        public Queue<char> _text { get; set; } = new Queue<char>();

        public TextGenerator()
        {
            for (int i = 0; i < 80; i++) 
                _text.Enqueue(Genirate());
        }

        public char Genirate() { return _available_values[_rand.Next(0, _available_values.Length)]; }
    }
}
