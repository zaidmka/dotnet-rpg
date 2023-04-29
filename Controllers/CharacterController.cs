global using dotnet_rpg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {

        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
{
            _characterService = characterService;
        }
        [HttpGet("GetAll")]
        public ActionResult<List<Character>> Get(){
            return Ok(_characterService.GetAllCharacter());
        }

        [HttpGet("{seq}")]
        public ActionResult<Character> GetSingle(int seq){
            return Ok(_characterService.GetCharacterById(seq));
        }

        [HttpPost]
        public ActionResult<List<Character>> AddCharacter(Character newCharacter){
            return Ok(_characterService.AddCharacter(newCharacter));
        }
    }
}