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

        private static List<Character> characters=new List<Character>{
            new Character(),
            new Character{Name="Cimaa",id=1},
            new Character{Name="Khudhur",id=2}
        };

        [HttpGet("GetAll")]
        public ActionResult<List<Character>> Get(){
            return Ok(characters);
        }

        [HttpGet("{seq}")]
        public ActionResult<Character> GetSingle(int seq){
            return Ok(characters.FirstOrDefault(p=>p.id==seq));
        }
    }
}