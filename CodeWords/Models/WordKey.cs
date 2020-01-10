using System;
using System.Collections.Generic;

namespace CodeWords.Models
{
    public class WordKey
    {
        public IEnumerable<String> BlueWords { get; set; }
        public IEnumerable<String> RedWords { get; set; }
        public String BlackWord { get; set; }
    }
}
