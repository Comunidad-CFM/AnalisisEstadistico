using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalisisEstadistico.Modulos
{
    public class Language
    {
        public string text;
        public Dictionary<char, double> dictionarySpanish;
        public Dictionary<char, double> dictionaryEnglish;
        public Dictionary<char, double> dictionaryGerman;
        public Dictionary<char, double> dictionaryPortuguese;
        public Dictionary<char, double> percents;
        public double spanishDeviation,
                      englishDeviation,
                      germanDeviation,
                      portugueseDeviation;

        public Language()
        {
            this.dictionarySpanish = new Dictionary<char, double>();
            this.dictionaryEnglish = new Dictionary<char, double>();
            this.dictionaryGerman = new Dictionary<char, double>();
            this.dictionaryPortuguese = new Dictionary<char, double>();

            this.initializeDictionaries();
        }

        public void initializeDictionaries()
        {
            this.dictionarySpanish.Add('a', 11.525);
            this.dictionarySpanish.Add('b', 2.215);
            this.dictionarySpanish.Add('c', 4.019);
            this.dictionarySpanish.Add('d', 5.01);
            this.dictionarySpanish.Add('e', 12.181);
            this.dictionarySpanish.Add('f', 0.692);
            this.dictionarySpanish.Add('g', 1.768);
            this.dictionarySpanish.Add('h', 0.703);
            this.dictionarySpanish.Add('i', 6.247);
            this.dictionarySpanish.Add('j', 0.493);
            this.dictionarySpanish.Add('k', 0.011);
            this.dictionarySpanish.Add('l', 4.967);
            this.dictionarySpanish.Add('m', 3.157);
            this.dictionarySpanish.Add('n', 6.712);
            this.dictionarySpanish.Add('o', 8.683);
            this.dictionarySpanish.Add('p', 2.51);
            this.dictionarySpanish.Add('q', 0.877);
            this.dictionarySpanish.Add('r', 6.871);
            this.dictionarySpanish.Add('s', 7.977);
            this.dictionarySpanish.Add('t', 4.632);
            this.dictionarySpanish.Add('u', 2.927);
            this.dictionarySpanish.Add('v', 1.138);
            this.dictionarySpanish.Add('w', 0.017);
            this.dictionarySpanish.Add('x', 0.215);
            this.dictionarySpanish.Add('y', 1.008);
            this.dictionarySpanish.Add('z', 0.467);
            this.dictionarySpanish.Add('à', 0);
            this.dictionarySpanish.Add('â', 0);
            this.dictionarySpanish.Add('á', 0.502);
            this.dictionarySpanish.Add('ä', 0);
            this.dictionarySpanish.Add('ã', 0);
            this.dictionarySpanish.Add('ç', 0);
            this.dictionarySpanish.Add('é', 0.433);
            this.dictionarySpanish.Add('ê', 0);
            this.dictionarySpanish.Add('í', 0.725);
            this.dictionarySpanish.Add('ñ', 0.311);
            this.dictionarySpanish.Add('ö', 0);
            this.dictionarySpanish.Add('ô', 0);
            this.dictionarySpanish.Add('ó', 0.827);
            this.dictionarySpanish.Add('ß', 0);
            this.dictionarySpanish.Add('ú', 0.168);
            this.dictionarySpanish.Add('ü', 0.012);

            this.dictionaryEnglish.Add('a', 8.167);
            this.dictionaryEnglish.Add('b', 1.492);
            this.dictionaryEnglish.Add('c', 2.782);
            this.dictionaryEnglish.Add('d', 4.253);
            this.dictionaryEnglish.Add('e', 12.702);
            this.dictionaryEnglish.Add('f', 2.228);
            this.dictionaryEnglish.Add('g', 2.015);
            this.dictionaryEnglish.Add('h', 6.094);
            this.dictionaryEnglish.Add('i', 6.966);
            this.dictionaryEnglish.Add('j', 0.153);
            this.dictionaryEnglish.Add('k', 0.772);
            this.dictionaryEnglish.Add('l', 4.025);
            this.dictionaryEnglish.Add('m', 2.406);
            this.dictionaryEnglish.Add('n', 6.749);
            this.dictionaryEnglish.Add('o', 7.507);
            this.dictionaryEnglish.Add('p', 1.929);
            this.dictionaryEnglish.Add('q', 0.095);
            this.dictionaryEnglish.Add('r', 5.987);
            this.dictionaryEnglish.Add('s', 6.327);
            this.dictionaryEnglish.Add('t', 9.056);
            this.dictionaryEnglish.Add('u', 2.758);
            this.dictionaryEnglish.Add('v', 0.978);
            this.dictionaryEnglish.Add('w', 2.361);
            this.dictionaryEnglish.Add('x', 0.15);
            this.dictionaryEnglish.Add('y', 1.974);
            this.dictionaryEnglish.Add('z', 0.074);
            this.dictionaryEnglish.Add('à', 0);
            this.dictionaryEnglish.Add('â', 0);
            this.dictionaryEnglish.Add('á', 0);
            this.dictionaryEnglish.Add('ä', 0);
            this.dictionaryEnglish.Add('ã', 0);
            this.dictionaryEnglish.Add('ç', 0);
            this.dictionaryEnglish.Add('é', 0);
            this.dictionaryEnglish.Add('ê', 0);
            this.dictionaryEnglish.Add('í', 0);
            this.dictionaryEnglish.Add('ñ', 0);
            this.dictionaryEnglish.Add('ö', 0);
            this.dictionaryEnglish.Add('ô', 0);
            this.dictionaryEnglish.Add('ó', 0);
            this.dictionaryEnglish.Add('ß', 0);
            this.dictionaryEnglish.Add('ú', 0);
            this.dictionaryEnglish.Add('ü', 0);

            this.dictionaryGerman.Add('a', 6.516);
            this.dictionaryGerman.Add('b', 1.886);
            this.dictionaryGerman.Add('c', 2.732);
            this.dictionaryGerman.Add('d', 5.076);
            this.dictionaryGerman.Add('e', 16.396);
            this.dictionaryGerman.Add('f', 1.656);
            this.dictionaryGerman.Add('g', 3.009);
            this.dictionaryGerman.Add('h', 4.577);
            this.dictionaryGerman.Add('i', 6.55);
            this.dictionaryGerman.Add('j', 0.268);
            this.dictionaryGerman.Add('k', 1.417);
            this.dictionaryGerman.Add('l', 3.437);
            this.dictionaryGerman.Add('m', 2.534);
            this.dictionaryGerman.Add('n', 9.776);
            this.dictionaryGerman.Add('o', 2.594);
            this.dictionaryGerman.Add('p', 0.067);
            this.dictionaryGerman.Add('q', 0.18);
            this.dictionaryGerman.Add('r', 7.003);
            this.dictionaryGerman.Add('s', 7.27);
            this.dictionaryGerman.Add('t', 6.154);
            this.dictionaryGerman.Add('u', 4.166);
            this.dictionaryGerman.Add('v', 0.846);
            this.dictionaryGerman.Add('w', 1.921);
            this.dictionaryGerman.Add('x', 0.034);
            this.dictionaryGerman.Add('y', 0.039);
            this.dictionaryGerman.Add('z', 1.134);
            this.dictionaryGerman.Add('à', 0);
            this.dictionaryGerman.Add('â', 0);
            this.dictionaryGerman.Add('á', 0);
            this.dictionaryGerman.Add('ä', 0.578);
            this.dictionaryGerman.Add('ã', 0);
            this.dictionaryGerman.Add('ç', 0);
            this.dictionaryGerman.Add('é', 0);
            this.dictionaryGerman.Add('ê', 0);
            this.dictionaryGerman.Add('í', 0);
            this.dictionaryGerman.Add('ñ', 0);
            this.dictionaryGerman.Add('ö', 0.443);
            this.dictionaryGerman.Add('ô', 0);
            this.dictionaryGerman.Add('ó', 0);
            this.dictionaryGerman.Add('ß', 0.307);
            this.dictionaryGerman.Add('ú', 0);
            this.dictionaryGerman.Add('ü', 0.995);

            this.dictionaryPortuguese.Add('a', 14.634);
            this.dictionaryPortuguese.Add('b', 1.043);
            this.dictionaryPortuguese.Add('c', 3.882);
            this.dictionaryPortuguese.Add('d', 4.992);
            this.dictionaryPortuguese.Add('e', 12.57);
            this.dictionaryPortuguese.Add('f', 1.023);
            this.dictionaryPortuguese.Add('g', 1.303);
            this.dictionaryPortuguese.Add('h', 0.781);
            this.dictionaryPortuguese.Add('i', 6.186);
            this.dictionaryPortuguese.Add('j', 0.397);
            this.dictionaryPortuguese.Add('k', 0.015);
            this.dictionaryPortuguese.Add('l', 2.779);
            this.dictionaryPortuguese.Add('m', 4.738);
            this.dictionaryPortuguese.Add('n', 4.446);
            this.dictionaryPortuguese.Add('o', 9.735);
            this.dictionaryPortuguese.Add('p', 2.523);
            this.dictionaryPortuguese.Add('q', 1.204);
            this.dictionaryPortuguese.Add('r', 6.53);
            this.dictionaryPortuguese.Add('s', 6.805);
            this.dictionaryPortuguese.Add('t', 4.336);
            this.dictionaryPortuguese.Add('u', 3.639);
            this.dictionaryPortuguese.Add('v', 1.575);
            this.dictionaryPortuguese.Add('w', 0.037);
            this.dictionaryPortuguese.Add('x', 0.253);
            this.dictionaryPortuguese.Add('y', 0.006);
            this.dictionaryPortuguese.Add('z', 0.47);
            this.dictionaryPortuguese.Add('à', 0.072);
            this.dictionaryPortuguese.Add('â', 0.562);
            this.dictionaryPortuguese.Add('á', 0.118);
            this.dictionaryPortuguese.Add('ä', 0);
            this.dictionaryPortuguese.Add('ã', 0.733);
            this.dictionaryPortuguese.Add('ç', 0.53);
            this.dictionaryPortuguese.Add('é', 0.337);
            this.dictionaryPortuguese.Add('ê', 0.45);
            this.dictionaryPortuguese.Add('í', 0.00132);
            this.dictionaryPortuguese.Add('ñ', 0);
            this.dictionaryPortuguese.Add('ö', 0);
            this.dictionaryPortuguese.Add('ô', 0.635);
            this.dictionaryPortuguese.Add('ó', 0.296);
            this.dictionaryPortuguese.Add('ß', 0);
            this.dictionaryPortuguese.Add('ú', 0.207);
            this.dictionaryPortuguese.Add('ü', 0.026);
        }

        public string cleanText()
        {
            string cleanedText = "",
                   letters = "abcdefghijklmnopqrstuvwxzàâáäãçéêíñöôóßúü";
            this.text.ToLower();

            foreach (char letter in this.text)
            {
                if (letters.Contains(letter))
                {
                    cleanedText += letter;
                }
            }

            return cleanedText;
        }

        public bool searchLetter(char letter, Dictionary<char, double> occurs)
        {
            foreach (KeyValuePair<char, double> item in occurs) 
            {
                if (item.Key == letter) {
                    return true;
                }
            }
            return false;
        }

        public int getOccurs(char letter, string cleanedText)
        {
            int count = 0;

            foreach (char character in cleanedText)
            {
                if (character.Equals(letter))
                {
                    count++;
                }
            }

            return count;
        }

        public Dictionary<char, double> getPercent(string cleanedText, int totalLetters)
        {
            Dictionary<char, double> occurs = new Dictionary<char, double>();

            foreach (char letter in cleanedText)
            {
                if (!searchLetter(letter, occurs))
                {
                    occurs.Add(letter, ((double)getOccurs(letter, cleanedText) / totalLetters) * 100);
                }
            }

            return occurs;
        }

        public string calculateDeviationAndShowResults(Dictionary<char, double> percents, int totalLetters)
        {
            double spanishDeviation = 0,
                   englishDeviation = 0,
                   germanDeviation = 0,
                   portugueseDeviation = 0;

            foreach (KeyValuePair<char, double> item in percents)
            {
                spanishDeviation += Math.Pow((dictionarySpanish[item.Key] - item.Value), 2);
                englishDeviation += Math.Pow((dictionaryEnglish[item.Key] - item.Value), 2);
                germanDeviation += Math.Pow((dictionaryGerman[item.Key] - item.Value), 2);
                portugueseDeviation += Math.Pow((dictionaryPortuguese[item.Key] - item.Value), 2);
            }

            spanishDeviation /= totalLetters;
            englishDeviation /= totalLetters;
            germanDeviation /= totalLetters;
            portugueseDeviation /= totalLetters;

            if (spanishDeviation < englishDeviation && spanishDeviation < germanDeviation && spanishDeviation < portugueseDeviation)
            {
                return "Spanish";
            }
            else if (englishDeviation < spanishDeviation && englishDeviation < germanDeviation && englishDeviation < portugueseDeviation)
            { 
                return "English";
            }
            else if (germanDeviation < spanishDeviation && germanDeviation < englishDeviation && germanDeviation < portugueseDeviation)
            {
                return "German";
            }
            else if (portugueseDeviation < spanishDeviation && portugueseDeviation < englishDeviation && portugueseDeviation < germanDeviation)
            {
                return "Portuguese";
            }
            else 
            {
                return "Unknown";
            }
        }

        public string languageAnalisys() 
        {
            this.percents = new Dictionary<char, double>();
            this.spanishDeviation = 0;
            this.englishDeviation = 0;
            this.germanDeviation = 0;
            this.portugueseDeviation = 0;

            string cleanedText = cleanText(); // Obtener las letras que realmente importan para el analisis.
            int totalLetters = text.Count(); //Total de letras del texto original.
            this.percents = getPercent(cleanedText, totalLetters); // Diccionario con la letra y el porcentaje de ocurrencias en el texto limpio.
            return calculateDeviationAndShowResults(this.percents, totalLetters); // Calcula la desviacion y muestra los resultados.
        }
    }
}