using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeWords.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeWords.Controllers.ApiControllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        private static Dictionary<String, GameState> _ActiveGames = new Dictionary<String, GameState>();

        [Route("generate"), HttpGet]
        public NewGame GenerateGameBoard()
        {
            return GameManager.CreateNewGame();
        }

        [Route("checkword"), HttpGet]
        public CardColor CheckWordColor(String sessionId, String word)
        {
            if (sessionId != null && word != null)
            {
                return GameManager.GetCardColor(sessionId, word);
            }

            throw new Exception("Invalid Session or Word");
        }


        [Route("codemaster"), HttpGet]
        public WordKey GetCodeMasterKey(String sessionId)
        {
            if(_ActiveGames.TryGetValue(sessionId.ToUpper(), out var wordKey))
            {
                var words = wordKey.Words;
                return new WordKey
                {
                    BlueWords = words.Where(x => x.Value == CardColor.Blue).Select(x => x.Key),
                    RedWords = words.Where(x => x.Value == CardColor.Red).Select(x => x.Key),
                    BlackWord = words.Where(x => x.Value == CardColor.Black).Select(x => x.Key).Single()
                };
            }
            return null;
        }
    }
}
