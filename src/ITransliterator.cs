using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skclusive.Tanglish.Core
{
    public interface ITransliterator
    {
        string Convert(string text);

        void Convert(string inputFile, string outputFile);

        void Convert(StreamReader input, StreamWriter output);
    }
}
