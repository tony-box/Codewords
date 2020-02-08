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
            return GameManager.CreateWordKey(sessionId);
        }
    }
}