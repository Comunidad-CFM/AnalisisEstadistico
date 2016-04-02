using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalisisEstadistico.Modulos
{
    public class Language
    {
        public string text;
        public Dictionary<char, double> dictionarySpanish = new Dictionary<char, double>();
        public Dictionary<char, double> dictionaryEnglish = new Dictionary<char, double>();
        public Dictionary<char, double> dictionaryGerman = new Dictionary<char, double>();
        public Dictionary<char, double> dictionaryPortuguese = new Dictionary<char, double>();
        public Dictionary<char, double> percents = new Dictionary<char, double>();
        public double spanishDeviation = 0,
                      englishDeviation = 0,
                      germanDeviation = 0,
                      portugueseDeviation = 0;

        public Language(string text)
        {
            this.text = text;
        }

        public void initializeDictionaries()
        {
            this.dictionarySpanish.Add('a', 0.11525);
            this.dictionarySpanish.Add('b', 0.02215);
            this.dictionarySpanish.Add('c', 0.04019);
            this.dictionarySpanish.Add('d', 0.0501);
            this.dictionarySpanish.Add('e', 0.12181);
            this.dictionarySpanish.Add('f', 0.00692);
            this.dictionarySpanish.Add('g', 0.01768);
            this.dictionarySpanish.Add('h', 0.00703);
            this.dictionarySpanish.Add('i', 0.06247);
            this.dictionarySpanish.Add('j', 0.00493);
            this.dictionarySpanish.Add('k', 0.00011);
            this.dictionarySpanish.Add('l', 0.04967);
            this.dictionarySpanish.Add('m', 0.03157);
            this.dictionarySpanish.Add('n', 0.06712);
            this.dictionarySpanish.Add('o', 0.08683);
            this.dictionarySpanish.Add('p', 0.0251);
            this.dictionarySpanish.Add('q', 0.00877);
            this.dictionarySpanish.Add('r', 0.06871);
            this.dictionarySpanish.Add('s', 0.07977);
            this.dictionarySpanish.Add('t', 0.04632);
            this.dictionarySpanish.Add('u', 0.02927);
            this.dictionarySpanish.Add('v', 0.01138);
            this.dictionarySpanish.Add('w', 0.00017);
            this.dictionarySpanish.Add('x', 0.00215);
            this.dictionarySpanish.Add('y', 0.01008);
            this.dictionarySpanish.Add('z', 0.00467);
            this.dictionarySpanish.Add('à', 0);
            this.dictionarySpanish.Add('â', 0);
            this.dictionarySpanish.Add('á', 0.00502);
            this.dictionarySpanish.Add('ä', 0);
            this.dictionarySpanish.Add('ã', 0);
            this.dictionarySpanish.Add('ç', 0);
            this.dictionarySpanish.Add('é', 0.00433);
            this.dictionarySpanish.Add('ê', 0);
            this.dictionarySpanish.Add('í', 0.00725);
            this.dictionarySpanish.Add('ñ', 0.00311);
            this.dictionarySpanish.Add('ö', 0);
            this.dictionarySpanish.Add('ô', 0);
            this.dictionarySpanish.Add('ó', 0.00827);
            this.dictionarySpanish.Add('ß', 0);
            this.dictionarySpanish.Add('ú', 0.00168);
            this.dictionarySpanish.Add('ü', 0.00012);

            this.dictionaryEnglish.Add('a', 0.08167);
            this.dictionaryEnglish.Add('b', 0.01492);
            this.dictionaryEnglish.Add('c', 0.02782);
            this.dictionaryEnglish.Add('d', 0.04253);
            this.dictionaryEnglish.Add('e', 0.12702);
            this.dictionaryEnglish.Add('f', 0.0228);
            this.dictionaryEnglish.Add('g', 0.02015);
            this.dictionaryEnglish.Add('h', 0.06094);
            this.dictionaryEnglish.Add('i', 0.06966);
            this.dictionaryEnglish.Add('j', 0.00153);
            this.dictionaryEnglish.Add('k', 0.00772);
            this.dictionaryEnglish.Add('l', 0.04025);
            this.dictionaryEnglish.Add('m', 0.02406);
            this.dictionaryEnglish.Add('n', 0.06749);
            this.dictionaryEnglish.Add('o', 0.07507);
            this.dictionaryEnglish.Add('p', 0.01929);
            this.dictionaryEnglish.Add('q', 0.00095);
            this.dictionaryEnglish.Add('r', 0.05987);
            this.dictionaryEnglish.Add('s', 0.06327);
            this.dictionaryEnglish.Add('t', 0.09056);
            this.dictionaryEnglish.Add('u', 0.2758);
            this.dictionaryEnglish.Add('v', 0.00978);
            this.dictionaryEnglish.Add('w', 0.02361);
            this.dictionaryEnglish.Add('x', 0.0015);
            this.dictionaryEnglish.Add('y', 0.01974);
            this.dictionaryEnglish.Add('z', 0.00074);
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

            this.dictionaryGerman.Add('a', 0.06516);
            this.dictionaryGerman.Add('b', 0.1886);
            this.dictionaryGerman.Add('c', 0.02732);
            this.dictionaryGerman.Add('d', 0.05076);
            this.dictionaryGerman.Add('e', 0.16396);
            this.dictionaryGerman.Add('f', 0.01656);
            this.dictionaryGerman.Add('g', 0.03009);
            this.dictionaryGerman.Add('h', 0.04577);
            this.dictionaryGerman.Add('i', 0.0655);
            this.dictionaryGerman.Add('j', 0.00268);
            this.dictionaryGerman.Add('k', 0.01417);
            this.dictionaryGerman.Add('l', 0.03437);
            this.dictionaryGerman.Add('m', 0.02534);
            this.dictionaryGerman.Add('n', 0.09776);
            this.dictionaryGerman.Add('o', 0.02594);
            this.dictionaryGerman.Add('p', 0.0067);
            this.dictionaryGerman.Add('q', 0.0018);
            this.dictionaryGerman.Add('r', 0.07003);
            this.dictionaryGerman.Add('s', 0.0727);
            this.dictionaryGerman.Add('t', 0.06154);
            this.dictionaryGerman.Add('u', 0.04166);
            this.dictionaryGerman.Add('v', 0.00846);
            this.dictionaryGerman.Add('w', 0.01921);
            this.dictionaryGerman.Add('x', 0.00034);
            this.dictionaryGerman.Add('y', 0.00039);
            this.dictionaryGerman.Add('z', 0.01134);
            this.dictionaryGerman.Add('à', 0);
            this.dictionaryGerman.Add('â', 0);
            this.dictionaryGerman.Add('á', 0);
            this.dictionaryGerman.Add('ä', 0.00578);
            this.dictionaryGerman.Add('ã', 0);
            this.dictionaryGerman.Add('ç', 0);
            this.dictionaryGerman.Add('é', 0);
            this.dictionaryGerman.Add('ê', 0);
            this.dictionaryGerman.Add('í', 0);
            this.dictionaryGerman.Add('ñ', 0);
            this.dictionaryGerman.Add('ö', 0.00443);
            this.dictionaryGerman.Add('ô', 0);
            this.dictionaryGerman.Add('ó', 0);
            this.dictionaryGerman.Add('ß', 0.00307);
            this.dictionaryGerman.Add('ú', 0);
            this.dictionaryGerman.Add('ü', 0.00995);

            this.dictionaryPortuguese.Add('a', 0.14634);
            this.dictionaryPortuguese.Add('b', 0.01043);
            this.dictionaryPortuguese.Add('c', 0.03882);
            this.dictionaryPortuguese.Add('d', 0.04992);
            this.dictionaryPortuguese.Add('e', 0.1257);
            this.dictionaryPortuguese.Add('f', 0.01023);
            this.dictionaryPortuguese.Add('g', 0.01303);
            this.dictionaryPortuguese.Add('h', 0.00781);
            this.dictionaryPortuguese.Add('i', 0.06186);
            this.dictionaryPortuguese.Add('j', 0.00397);
            this.dictionaryPortuguese.Add('k', 0.00015);
            this.dictionaryPortuguese.Add('l', 0.02779);
            this.dictionaryPortuguese.Add('m', 0.04738);
            this.dictionaryPortuguese.Add('n', 0.04446);
            this.dictionaryPortuguese.Add('o', 0.09735);
            this.dictionaryPortuguese.Add('p', 0.02523);
            this.dictionaryPortuguese.Add('q', 0.01204);
            this.dictionaryPortuguese.Add('r', 0.0653);
            this.dictionaryPortuguese.Add('s', 0.06805);
            this.dictionaryPortuguese.Add('t', 0.04336);
            this.dictionaryPortuguese.Add('u', 0.03639);
            this.dictionaryPortuguese.Add('v', 0.01575);
            this.dictionaryPortuguese.Add('w', 0.00037);
            this.dictionaryPortuguese.Add('x', 0.00253);
            this.dictionaryPortuguese.Add('y', 0.00006);
            this.dictionaryPortuguese.Add('z', 0.0047);
            this.dictionaryPortuguese.Add('à', 0.00072);
            this.dictionaryPortuguese.Add('â', 0.00562);
            this.dictionaryPortuguese.Add('á', 0.00118);
            this.dictionaryPortuguese.Add('ä', 0);
            this.dictionaryPortuguese.Add('ã', 0.00733);
            this.dictionaryPortuguese.Add('ç', 0.0053);
            this.dictionaryPortuguese.Add('é', 0.00337);
            this.dictionaryPortuguese.Add('ê', 0.0045);
            this.dictionaryPortuguese.Add('í', 0.00132);
            this.dictionaryPortuguese.Add('ñ', 0);
            this.dictionaryPortuguese.Add('ö', 0);
            this.dictionaryPortuguese.Add('ô', 0.00635);
            this.dictionaryPortuguese.Add('ó', 0.00296);
            this.dictionaryPortuguese.Add('ß', 0);
            this.dictionaryPortuguese.Add('ú', 0.00207);
            this.dictionaryPortuguese.Add('ü', 0.00026);
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

        public Dictionary<char, double> getPercent(string cleanedText, int totalLetters)
        {
            Dictionary<char, double> occurs = new Dictionary<char, double>();

            foreach (char letter in cleanedText)
            {
                if (!searchLetter(letter, occurs))
                {
                    occurs.Add(letter, ((cleanedText.LongCount(letra => letra.ToString().Equals(letter))) / totalLetters) * 100);
                }
            }

            return occurs;
        }

        public void calculateDeviationAndShowResults(Dictionary<char, double> percents, int totalLetters)
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

            if (spanishDeviation > englishDeviation && spanishDeviation > germanDeviation && spanishDeviation > portugueseDeviation)
            { 
                // Spanish
            }
            else if (englishDeviation > spanishDeviation && englishDeviation > germanDeviation && englishDeviation > portugueseDeviation)
            { 
                // English
            }
            else if (germanDeviation > spanishDeviation && germanDeviation > englishDeviation && germanDeviation > portugueseDeviation)
            {
                // German
            }
            else if (portugueseDeviation > spanishDeviation && portugueseDeviation > englishDeviation && portugueseDeviation > germanDeviation)
            {
                // English
            }
            else 
            { 
                // Unknown
            }
        }

        public void languageAnalisys() 
        {    
            string cleanedText = cleanText(); // Obtener las letras que realmente importan para el analisis.
            int totalLetters = text.Count(); //Total de letras del texto original.
            this.percents = getPercent(cleanedText, totalLetters); // Diccionario con la letra y el porcentaje de ocurrencias en el texto limpio.
            calculateDeviationAndShowResults(this.percents, totalLetters); // Calcula la desviacion y muestra los resultados.
        }
    }
}