using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skclusive.Tanglish.Core
{
    class TamilTransliterator : ITransliterator
    {
        static Dictionary<int, string> granda_bases = new Dictionary<int, string>()
        {
            {2972, "j"}, //granda
            {3000, "S"}, //granda
            {3001, "h"}, //granda
            {2999, "sh"}, //granda
        };

        static Dictionary<int, string> no_granda_bases = new Dictionary<int, string>()
        {
            {2972, "s"}, //no granda
            {3000, "s"}, //no granda
            {3001, "k"}, //no granda
            {2999, "s"}, //no granda
        };

        public TamilTransliterator(bool granda = true)
        {
            IsGranda = granda;

            bases =  granda ? Concat(_bases, granda_bases) : Concat(_bases, no_granda_bases);
        }

        private static Dictionary<int, string> Concat(Dictionary<int, string> first, Dictionary<int, string> second)
        {
            Dictionary<int, string> result = new Dictionary<int, string>(first);
            
            foreach (var item in second)
            {
                result.Add(item.Key, item.Value);
            }

            return result;
        }

        public bool IsGranda { get; private set; }

        static Dictionary<int, string> _bases = new Dictionary<int, string>()
        {
            {2965, "k"},
            {2969, "ng"},
            {2970, "s"},
            {2974, "nj"},
            {2975, "t"},
            {2979, "N"},
            {2980, "th"},
            {2984, "w"},
            {2986, "p"},
            {2990, "m"},
            {2991, "y"},
            {2992, "r"},
            {2994, "l"},
            {2997, "v"},
            {2996, "z"},
            {2995, "L"},
            {2993, "R"},
            {2985, "n"},
        };

      

        Dictionary<int, string> bases = null;

        //static Dictionary<int, string> bases = new Dictionary<int, string>()
        //{
        //    {2965, "k"},
        //    {2969, "ng"},
        //    {2970, "s"},
        //    {2974, "nj"},
        //    {2975, "t"},
        //    {2979, "N"},
        //    {2980, "th"},
        //    {2984, "w"},
        //    {2986, "p"},
        //    {2990, "m"},
        //    {2991, "y"},
        //    {2992, "r"},
        //    {2994, "l"},
        //    {2997, "v"},
        //    {2996, "z"},
        //    {2995, "L"},
        //    {2993, "R"},
        //    {2985, "n"},

        //    {2972, "j"}, //granda
        //    {3000, "S"}, //granda
        //    {3001, "h"}, //granda
        //    {2999, "sh"}, //granda
        //};

        static Dictionary<int, string> others = new Dictionary<int, string>()
        {
            {0,    "a"},
            {3021, ""},
            {3006, "A"},
            {3007, "i"},
            {3008, "I"},
            {3009, "u"},
            {3010, "U"},
            {3014, "e"},
            {3015, "E"},
            {3016, "ai"},
            {3018, "o"},
            {3019, "O"},
            {3020, "au"}
        };

        static Dictionary<int, string> uyir = new Dictionary<int, string>()
        {
            {2949, "a"},
            {2950, "A"},
            {2951, "i"},
            {2952, "I"},
            {2953, "u"},
            {2954, "U"},
            {2958, "e"},
            {2959, "E"},
            {2960, "ai"},
            {2962, "o"},
            {2963, "O"},
            {2964, "au"},
            {2947, "q"}
        };

        private string GetBase(int value)
        {
            return IsBase(value) ? bases[value] : Char.ToString((char)value);
        }

        private string GetUyir(int value)
        {
            return IsUyir(value) ? uyir[value] : Char.ToString((char)value);
        }

        private string GetOther(int value)
        {
            return IsOther(value) ? others[value] : Char.ToString((char)value);
        }

        private bool IsBase(int value)
        {
            return bases.ContainsKey(value);
        }

        private bool IsUyir(int value)
        {
            return uyir.ContainsKey(value);
        }

        private bool IsOther(int value)
        {
            return others.ContainsKey(value);
        }

        private int Peek(StreamReader reader)
        {
            return reader.Peek();
        }

        private int Read(StreamReader reader)
        {
            return reader.Read();
        }

        private bool CanRead(StreamReader reader)
        {
            return !reader.EndOfStream;
        }

        public string Convert(string tamil)
        {
            string tanglish = string.Empty;

            using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(tamil))))
            {
                MemoryStream stream = new MemoryStream();

                StreamWriter writer = new StreamWriter(stream);

                this.Convert(reader, writer);

                stream.Position = 0;

                using (StreamReader result = new StreamReader(stream))
                {
                    tanglish = result.ReadToEnd();
                }
            }

            return tanglish;
        }

        public void Convert(string inputFile, string outputFile)
        {
            //using (StreamWriter output = new StreamWriter(outputFile,Encoding.UTF8))
            //{
            //    using (StreamReader input = new StreamReader(inputFile, Encoding.UTF8))
            //    {
            //        Convert(input, output);
            //    }
            //}
        }

        public void Convert(StreamReader input, StreamWriter output)
        {
            while (CanRead(input))
            {
                //EchoUnicode(input, output);
                Transliterate(input, output);
            }

            output.Flush();
        }

        private void EchoUnicode(StreamReader input, StreamWriter output)
        {
            int read = Read(input);

            output.Write(read);
        }

        private void Transliterate(StreamReader input, StreamWriter output)
        {
            int read = Read(input);

            string basex = IsUyir(read) ? GetUyir(read) : GetBase(read);

            string nextx = string.Empty;

            int next = -1;

            if (CanRead(input) && IsBase(read))
            {
                next = Peek(input);

                if (IsOther(next))
                {
                    next = Read(input);
                }
                else
                {
                    next = 0;
                }
            }
            else if (IsBase(read))
            {
                next = 0;
            }

            if (next != -1)
            {
                nextx = GetOther(next);
            }

            output.Write(basex);

            output.Write(nextx);
        }
    }
}
