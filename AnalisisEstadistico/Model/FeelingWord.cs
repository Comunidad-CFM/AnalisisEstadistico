using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalisisEstadistico.Model
{
    public class FeelingWord
    {
        public string word;
        public int score;

        public FeelingWord(string w, int s)
        {
            this.word = w;
            this.score = s;
        }
    }
}