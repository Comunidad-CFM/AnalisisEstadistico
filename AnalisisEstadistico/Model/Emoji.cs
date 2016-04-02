using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalisisEstadistico.Model
{
    public class Emoji
    {
        public string emoji;
        public int score;

        public Emoji(string e, int s)
        {
            this.emoji = e;
            this.score = s;
        }
    }
}