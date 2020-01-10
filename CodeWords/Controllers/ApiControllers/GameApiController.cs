using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeWords.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;

namespace CodeWords.Controllers.ApiControllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        private static Dictionary<String, Dictionary<String, CardColor>> _ActiveGames = new Dictionary<string, Dictionary<string, CardColor>>();
        private Byte _WordsToGuess = 8;
        private Random _Random = new Random();

        public String CreateNewGame()
        {
            return "aaaa";
        }

        [Route("generate"), HttpGet]
        public NewGame GenerateGameBoard(String sessionId)
        {
            if (_ActiveGames.TryGetValue(sessionId, out var existingGame))
            {
                return null;
            }

            var blueGoesFirst = _Random.Next(0, 2) == 0 ? true : false;
            var blueWords = _WordsToGuess;
            var redWords = _WordsToGuess;
            if (blueGoesFirst)
            {
                blueWords += 1;
            }
            else
            {
                redWords += 1;
            }

            var assignedWords = new Dictionary<string, CardColor>();
            var words = RetriveWords();
            var index = 0;
            for (var i = 0; i < blueWords; i++, index++)
            {
                assignedWords.Add(words[index], CardColor.Blue);
            }

            for (var i = 0; i < redWords; i++, index++)
            {
                assignedWords.Add(words[index], CardColor.Red);
            }

            assignedWords.Add(words[index++], CardColor.Black);

            for (var i = 0; i < 7; i++, index++)
            {
                assignedWords.Add(words[index], CardColor.Neutral);
            }

            _ActiveGames.Add(sessionId, assignedWords);
            
            words.Shuffle();
            return new NewGame()
            {
                SessionId = CreateNewGame(),
                Words = words
            };
        }

        [Route("checkword"), HttpGet]
        public CardColor CheckWordColor(String sessionId, String word)
        {
            return _ActiveGames[sessionId][word];
        }


        [Route("codemaster"), HttpGet]
        public WordKey GetCodeMasterKey(String sessionId)
        {
            if(_ActiveGames.TryGetValue(sessionId, out var wordKey))
            {
                return new WordKey
                {
                    BlueWords = wordKey.Where(x => x.Value == CardColor.Blue).Select(x => x.Key),
                    RedWords = wordKey.Where(x => x.Value == CardColor.Red).Select(x => x.Key),
                    BlackWord = wordKey.Where(x => x.Value == CardColor.Black).Select(x => x.Key).Single()
                };
            }
            return null;
        }

        private String[] RetriveWords()
        {
            var wordArray = new String[25];
            var wordCount = Words.WordCount();
            var usedWords = new HashSet<int>();
            var wordIndex = 0;
            for (var i = 0; i < wordArray.Length; i++)
            {
                while (true)
                {
                    wordIndex = _Random.Next(0, wordCount);
                    if (!usedWords.Contains(wordIndex))
                    {
                        usedWords.Add(wordIndex);
                        break;
                    }
                }
                wordArray[i] = Words.GetWord(wordIndex);
            }

            return wordArray;
        }
    }
    public static class Extensions
    {
        private static Random _Random = new Random();
        public static void Shuffle(this String[] list)
        {
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = _Random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
