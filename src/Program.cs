using Skclusive.Tanglish.Core;

var transliterator = Transliteration.GetTransliterator(TransliterationType.Tamil);

var result = transliterator.Convert("விரதம் பழக்கமாகாது");

Console.WriteLine($"{result}"); // prints: "viratham pazakkamAkAthu"