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
        private Byte _WordsToGuess = 8;
        private Random _Random = new Random();

        public String CreateNewGame(Int32 size)
        {
            StringBuilder builder = new StringBuilder();
            Char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _Random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();            
        }

        [Route("generate"), HttpGet]
        public NewGame GenerateGameBoard(String sessionId)
        {
            if (sessionId != null && _ActiveGames.ContainsKey(sessionId))
            {
                return null;
            }

            var newSessionId = CreateNewGame(4);
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

            var assignedWords = new Dictionary<String, CardColor>();
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

            var newGame = new GameState
            {
                Words = assignedWords,
                BlueWordsLeft = blueWords,
                RedWordsLeft = redWords
            };

            _ActiveGames.Add(newSessionId, newGame);
            
            words.Shuffle();
            return new NewGame()
            {
                SessionId = newSessionId,
                Words = words,
                RedWordCount = redWords,
                BlueWordCount = blueWords
            };
        }

        [Route("checkword"), HttpGet]
        public CardColor CheckWordColor(String sessionId, String word)
        {
            return _ActiveGames[sessionId].Words[word];
        }


        [Route("codemaster"), HttpGet]
        public WordKey GetCodeMasterKey(String sessionId)
        {
            if(_ActiveGames.TryGetValue(sessionId, out var wordKey))
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

        private String[] RetriveWords()
        {
            var wordArray = new String[25];
            var wordCount = Words.WordCount();
            var usedWords = new HashSet<Int32>();
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
            Int32 n = list.Length;
            while (n > 1)
            {
                n--;
                Int32 k = _Random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
