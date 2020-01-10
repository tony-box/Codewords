using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeWords.Models
{
    public class NewGame
    {
        public String SessionId { get; set; }
        public IEnumerable<String> Words { get; set; }
    }
}
