using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skclusive.Tanglish.Core
{
    public enum TransliterationType
    {
        Tamil,
        English
    }

    public class Transliteration
    {
        public static ITransliterator GetTransliterator(TransliterationType type, bool granda = true)
        {
            ITransliterator transliterator = null;

            if (type == TransliterationType.English)
            {
                transliterator = new EnglishTransliterator(granda);

            }else if (type == TransliterationType.Tamil)
            {
                transliterator = new TamilTransliterator(granda);
            }

            return transliterator;
        }
    }
}
