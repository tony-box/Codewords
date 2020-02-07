using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeWords.Models
{
    public class GameState
    {
        public Dictionary<String, CardColor> Words { get; set; }
        public Byte BlueWordsLeft { get; set; }
        public Byte RedWordsLeft { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
