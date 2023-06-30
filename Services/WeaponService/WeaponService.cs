using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Weapon;

namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService : IWeaponeService
    {
        private readonly DataContext _Context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WeaponService(DataContext Context,IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _Context = Context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _Context.Characters
                .FirstOrDefaultAsync(c=>c.Id == newWeapon.CharacterId &&
                c.User!.Id==int.Parse(_httpContextAccessor.HttpContext!.User
                .FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(character is null){
                response.Sucess=false;
                response.Message="Character NOT Found!";
                return response;
            }
            var weapon = new Weapon
            {
                Name = newWeapon.Name,
                Damage=newWeapon.Damage,
                Character=character
            };
            _Context.Weapons.Add(weapon);
            await _Context.SaveChangesAsync();
            response.Data=_mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex) 
            {
                response.Sucess=false;
                response.Message=ex.Message;
            }
            return response;
        }
    }
}