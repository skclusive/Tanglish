using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skclusive.Tanglish.Core
{
    class EnglishTransliterator : ITransliterator
    {
        public EnglishTransliterator(bool granda = true)
        {
            IsGranda = granda;

            mei = granda ? Concat(_mei, granda_mei) : Concat(_mei, no_granda_mei);

            s_mei = new Dictionary<int, int>(granda ? s_granda_mei : s_no_granda_mei);

            sub_mei = new Dictionary<int, Dictionary<int, int>>(_sub_mei);

            sub_mei.Add('s', s_mei);
        }

        private static Dictionary<int, int> Concat(Dictionary<int, int> first, Dictionary<int, int> second)
        {
            Dictionary<int, int> result = new Dictionary<int, int>(first);
            
            foreach (var item in second)
            {
                result.Add(item.Key, item.Value);
            }

            return result;
        }

        public bool IsGranda { get; private set; }

        #region Uyir

        static Dictionary<int, string> uyir = new Dictionary<int, string>()
        {
            {'a', "அ"},
            {'A', "ஆ"},
            {'i', "இ"},
            {'I', "ஈ"},
            {'u', "உ"},
            {'U', "ஊ"},
            {'e', "எ"},
            {'E', "ஏ"},
            {'o', "ஒ"},
            {'O', "ஓ"},
            {'q', "ஃ"},
        };

        static Dictionary<int, string> a_uyir = new Dictionary<int, string>()
        {
            {'i', "ஐ"},
            {'u', "ஔ"},
        };

        static Dictionary<int, string> o_uyir = new Dictionary<int, string>()
        {   
            {'w', "ஔ"},
        };

        static Dictionary<int, Dictionary<int, string>> sub_uyir = new Dictionary<int, Dictionary<int, string>>()
        {
            {'a', a_uyir},
            {'o', o_uyir},
        };

        private string GetUyir(int value)
        {
            return uyir.ContainsKey(value) ? uyir[value] : Char.ToString((char)value);
        }

        private string GetSubUyir(int bas, int sub)
        {
            if (IsSubUyir(bas, sub))
            {
                Dictionary<int, string> subbase = sub_uyir[bas];

                return subbase[sub];
            }

            return string.Empty;
        }

        private bool IsUyir(int value)
        {
            return uyir.ContainsKey(value);
        }

        private bool IsSubUyir(int bas, int sub)
        {
            if (HasSubUyir(bas))
            {
                Dictionary<int, string> subbase = sub_uyir[bas];

                return subbase != null && subbase.ContainsKey(sub);
            }

            return false;
        }

        private bool HasSubUyir(int value)
        {
            return sub_uyir.ContainsKey(value);
        }

        #endregion

        #region Mei

        static Dictionary<int, int> _mei = new Dictionary<int, int>()
        {
            {'k', 2965},
            {'g', 2965},
            {'s', 2970},
            {'c', 2970},
            {'t', 2975},
            {'d', 2975},
            {'N', 2979},
            {'w', 2984},
            {'p', 2986},
            {'b', 2986},
            {'m', 2990},
            {'y', 2991},
            {'r', 2992},
            {'l', 2994},
            {'v', 2997},
            {'z', 2996},
            {'L', 2995},
            {'R', 2993},
            {'n', 2985},
        };

        static Dictionary<int, int> granda_mei = new Dictionary<int, int>()
        {
            {'j', 2972}, //granda
            {'S', 3000}, //granda
            {'h', 3001}, //granda
        };

        static Dictionary<int, int> no_granda_mei = new Dictionary<int, int>()
        {
            {'j', 2970}, //no granda
            {'S', 2970}, //no granda
            {'h', 2965}, //no granda
        };

        Dictionary<int, int> mei = null;

        //static Dictionary<int, int> mei = new Dictionary<int, int>()
        //{
        //    {'k', 2965},
        //    {'g', 2965},
        //    {'s', 2970},
        //    {'c', 2970},
        //    {'j', 2972}, //granda
        //    {'S', 3000}, //granda
        //    {'h', 3001}, //granda
        //    {'t', 2975},
        //    {'d', 2975},
        //    {'N', 2979},
        //    {'w', 2984},
        //    {'p', 2986},
        //    {'b', 2986},
        //    {'m', 2990},
        //    {'y', 2991},
        //    {'r', 2992},
        //    {'l', 2994},
        //    {'v', 2997},
        //    {'z', 2996},
        //    {'L', 2995},
        //    {'R', 2993},
        //    {'n', 2985},
        //};

        static Dictionary<int, int> n_mei = new Dictionary<int, int>()
        {
            {'g', 2969},
            {'j', 2974},
        };

        static Dictionary<int, int> t_mei = new Dictionary<int, int>()
        {
            {'h', 2980},
        };

        Dictionary<int, int> s_mei = null;

        static Dictionary<int, int> s_granda_mei = new Dictionary<int, int>()
        {
            {'h', 2999}, //granda
        };

        static Dictionary<int, int> s_no_granda_mei = new Dictionary<int, int>()
        {
            {'h', 2970}, //granda
        };

        //static Dictionary<int, int> s_mei = new Dictionary<int, int>()
        //{
        //    {'h', 2999}, //granda
        //};

        //static Dictionary<int, Dictionary<int, int>> sub_mei = new Dictionary<int, Dictionary<int, int>>()
        //{
        //    {'n', n_mei},
        //    {'t', t_mei},
        //    {'s', s_mei}, //granda
        //};

        Dictionary<int, Dictionary<int, int>> sub_mei = null;

        static Dictionary<int, Dictionary<int, int>> _sub_mei = new Dictionary<int, Dictionary<int, int>>()
        {
            {'n', n_mei},
            {'t', t_mei},
            //{'s', s_mei}, //granda
        };

        private int GetMei(int value)
        {
            return mei.ContainsKey(value) ? mei[value] : (char)value;
        }

        private bool IsMei(int value)
        {
            return mei.ContainsKey(value);
        }

        private bool HasSubMei(int value)
        {
            return sub_mei.ContainsKey(value);
        }

        private bool IsSubMei(int bas, int sub)
        {
            if (HasSubMei(bas))
            {
                Dictionary<int, int> subbase = sub_mei[bas];

                return subbase != null && subbase.ContainsKey(sub);
            }

            return false;
        }

        private int GetSubMei(int bas, int sub)
        {
            if (IsSubMei(bas, sub))
            {
                Dictionary<int, int> subbase = sub_mei[bas];

                return subbase[sub];
            }

            return -1;
        }

        #endregion

        #region UyirMei

        static Dictionary<int, int> uyir_mei = new Dictionary<int, int>()
        {
            {'a', -1},
            {'A', 3006},
            {'i', 3007},
            {'I', 3008},
            {'u', 3009},
            {'U', 3010},
            {'e', 3014},
            {'E', 3015},
            {'o', 3018},
            {'O', 3019},
        };

        static Dictionary<int, int> a_uyir_mei = new Dictionary<int, int>()
        {
            {'i', 3016},
            {'u', 3020},
        };

        static Dictionary<int, int> o_uyir_mei = new Dictionary<int, int>()
        {
            {'w', 3020},
        };

        static Dictionary<int, Dictionary<int, int>> sub_uyir_mei = new Dictionary<int, Dictionary<int, int>>()
        {
            {'a', a_uyir_mei},
            {'o', o_uyir_mei},
        };

        private int GetUyirMei(int value)
        {
            return uyir_mei.ContainsKey(value) ? uyir_mei[value] : (char)value;
        }

        private bool IsUyirMei(int value)
        {
            return uyir_mei.ContainsKey(value);
        }

        private bool HasSubUyirMei(int value)
        {
            return sub_uyir_mei.ContainsKey(value);
        }

        private bool IsSubUyirMei(int bas, int sub)
        {
            if (HasSubUyirMei(bas))
            {
                Dictionary<int, int> subbase = sub_uyir_mei[bas];

                return subbase != null && subbase.ContainsKey(sub);
            }

            return false;
        }

        private int GetSubUyirMei(int bas, int sub)
        {
            if (IsSubUyirMei(bas, sub))
            {
                Dictionary<int, int> subbase = sub_uyir_mei[bas];

                return subbase[sub];
            }

            return -1;
        }

        #endregion

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
            //using (StreamWriter output = new StreamWriter(outputFile, false, Encoding.UTF8))
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
                int read = Read(input);

                string converted = string.Empty;

                if (IsUyir(read))
                {
                    if (HasSubUyir(read))
                    {
                        int sub = Peek(input);

                        if (IsSubUyir(read, sub))
                        {
                            sub = Read(input);
                            converted = GetSubUyir(read, sub);
                        }
                        else
                        {
                            converted = GetUyir(read);
                        }
                    }
                    else
                    {
                        converted = GetUyir(read);
                    }
                    output.Write(converted);
                }
                else if (IsMei(read))
                {
                    if (HasSubMei(read))
                    {
                        int sub = Peek(input);

                        if (IsSubMei(read, sub))
                        {
                            sub = Read(input);
                            convertSubMei(input, output, read, sub);
                        }
                        else
                        {
                            convertMei(input, output, read);
                        }
                    }
                    else
                    {
                        convertMei(input, output, read);
                    }
                }
                else
                {
                    converted = char.ToString((char)read);
                    output.Write(converted);
                }
            }

            output.Flush();
        }

        private void convertMei(StreamReader input, StreamWriter output, int read)
        {
            int second = Peek(input);

            if (IsUyir(second))
            {
                second = Read(input);

                if (HasSubUyir(second))
                {
                    int sub = Peek(input);

                    if (IsSubUyirMei(second, sub))
                    {
                        sub = Read(input);

                        var mei_ = (char)GetMei(read);
                        int uyir_mei = GetSubUyirMei(second, sub);

                        output.Write((char)mei_);

                        if (uyir_mei != -1)
                            output.Write((char)uyir_mei);
                    }
                    else
                    {
                        var mei_ = (char)GetMei(read);
                        int uyir_mei = GetUyirMei(second);

                        output.Write((char)mei_);

                        if (uyir_mei != -1)
                            output.Write((char)uyir_mei);
                    }
                }
                else
                {
                    var mei_ = (char)GetMei(read);
                    int uyir_mei = GetUyirMei(second);

                    output.Write((char)mei_);

                    if (uyir_mei != -1)
                        output.Write((char)uyir_mei);
                }
            }
            else //if (!IsMei(second))
            {
                var mei_ = (char)GetMei(read);
                var join = (char)3021;
                output.Write(mei_);
                output.Write(join);
            }
        }

        private void convertSubMei(StreamReader input, StreamWriter output, int read, int mei)
        {
            int second = Peek(input);

            if (IsUyir(second))
            {
                second = Read(input);

                if (HasSubUyir(second))
                {
                    int sub = Peek(input);

                    if (IsSubUyirMei(second, sub))
                    {
                        sub = Read(input);

                        var mei_ = (char)GetSubMei(read, mei);

                        int uyir_mei = GetSubUyirMei(second, sub);

                        output.Write((char)mei_);

                        if (uyir_mei != -1)
                            output.Write((char)uyir_mei);
                    }
                    else
                    {
                        var mei_ = (char)GetSubMei(read, mei);
                        int uyir_mei = GetUyirMei(second);

                        output.Write((char)mei_);

                        if (uyir_mei != -1)
                            output.Write((char)uyir_mei);
                    }
                }
                else
                {
                    var mei_ = (char)GetSubMei(read, mei);
                    int uyir_mei = GetUyirMei(second);

                    output.Write((char)mei_);

                    if (uyir_mei != -1)
                        output.Write((char)uyir_mei);
                }
            }
            else //if (!IsMei(second))
            {
                var mei_ = (char)GetSubMei(read, mei);
                var join = (char)3021;
                output.Write(mei_);
                output.Write(join);
            }
        } 
    }
}
