﻿using CodeWords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWords
{
    public static class GameManager
    {
        private static Dictionary<String, GameState> _ActiveGames = new Dictionary<String, GameState>();
        private static readonly Random _Random = new Random();
        private static Byte _WordsToGuess = 8;

        public static NewGame CreateNewGame()
        {
            var newSessionId = GenerateSessionId();
            var blueGoesFirst = _Random.Next(0, 2) == 0 ? true : false;

            var assignedWords = new Dictionary<String, CardColor>();
            var words = RetriveWords();
            var index = 0;
            for (var i = 0; i < _WordsToGuess; i++, index++)
            {
                assignedWords.Add(words[index], CardColor.Blue);
            }

            for (var i = 0; i < _WordsToGuess; i++, index++)
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
                BlueWordsLeft = _WordsToGuess,
                RedWordsLeft = _WordsToGuess,
                CreatedDateTime = DateTime.Now
            };

            if (blueGoesFirst)
            {
                newGame.BlueWordsLeft++;
                assignedWords.Add(words[index++], CardColor.Blue);
            }
            else
            {
                newGame.RedWordsLeft++;
                assignedWords.Add(words[index++], CardColor.Red);
            }

            _ActiveGames.Add(newSessionId, newGame);
            Shuffle(words);

            return new NewGame
            {
                SessionId = newSessionId,
                Words = words,
                RedWordCount = newGame.RedWordsLeft,
                BlueWordCount = newGame.BlueWordsLeft
            };
        }

        public static CardSelection ProcessCardSelection(String sessionId, String word)
        {
            if (!_ActiveGames.TryGetValue(sessionId, out var game))
            {
                throw new Exception("Could not find game session");
            }
            
            var cardColor = game.Words[word];
            var cardSelection = new CardSelection
            {
                CardColor = cardColor
            };
            if (game.GameOver)
            {
                return cardSelection;
            }
            
            switch (cardColor)
            {
                case CardColor.Blue:
                {
                    game.BlueWordsLeft--;
                    break;
                }
                case CardColor.Red:
                {
                    game.RedWordsLeft--;
                    break;
                }
                case CardColor.Black:
                {
                    game.GameOver = true;
                    cardSelection.GameOver = true;
                    cardSelection.VictoryMessage = "You picked the black word better luck next time!";
                    break;
                }
            }

            if (game.BlueWordsLeft == 0)
            {
                game.GameOver = true;
                cardSelection.GameOver = true;
                cardSelection.VictoryMessage = "Blue team wins!";
                
                return cardSelection;
            }
            
            if (game.RedWordsLeft == 0)
            {
                game.GameOver = true;
                cardSelection.GameOver = true;
                cardSelection.VictoryMessage = "Red team wins!";
                
                return cardSelection;
            }

            return cardSelection;
        }

        public static WordKey CreateWordKey(String sessionId)
        {
            if (!_ActiveGames.TryGetValue(sessionId.ToUpper(), out var wordKey))
            {
                throw new Exception("Invalid SessionID");
            }

            var words = wordKey.Words;
            return new WordKey
            {
                BlueWords = words.Where(x => x.Value == CardColor.Blue).Select(x => x.Key),
                RedWords = words.Where(x => x.Value == CardColor.Red).Select(x => x.Key),
                NeutralWords = words.Where(x => x.Value == CardColor.Neutral).Select(x => x.Key),
                BlackWord = words.Where(x => x.Value == CardColor.Black).Select(x => x.Key).Single()
            };
        }

        private static String GenerateSessionId()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _Random.NextDouble() + 65))));
            }

            return builder.ToString();
        }

        private static String[] RetriveWords()
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

        private static void Shuffle(String[] list)
        {
            var n = list.Length;
            while (n > 1)
            {
                n--;
                var k = _Random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
