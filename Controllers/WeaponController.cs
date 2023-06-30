using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController:ControllerBase
    {
        private readonly IWeaponeService _weaponService;
        public WeaponController(IWeaponeService weaponService)
        {
            _weaponService = weaponService;
            
        }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> addWeapon(AddWeaponDto newWeapon)
    {
        return Ok(await _weaponService.AddWeapon(newWeapon));
    }
  
    }
}