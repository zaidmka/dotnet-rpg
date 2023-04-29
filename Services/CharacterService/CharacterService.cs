using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
            private static List<Character> characters=new List<Character>{
            new Character(),
            new Character{Name="Cimaa",Id=1},
            new Character{Name="Khudhur",Id=2}
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
    {
            _mapper = mapper;
        }
    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddcharacterDto newCharacter)
        {
            var ServiceResponse=new ServiceResponse<List<GetCharacterDto>>();
            var character= _mapper.Map<Character>(newCharacter);
            character.Id=characters.Max(c=>c.Id)+1;
            characters.Add(character);
            ServiceResponse.Data=characters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToList();
            return ServiceResponse;        
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            var ServiceResponse=new ServiceResponse<List<GetCharacterDto>>();
            ServiceResponse.Data=characters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToList();

            return ServiceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int seq)
        {
            var ServiceResponse=new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(p=>p.Id==seq);
            ServiceResponse.Data= _mapper.Map<GetCharacterDto>(character);
            return ServiceResponse;        

        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse=new ServiceResponse<GetCharacterDto>();
            try{
            var character=characters.FirstOrDefault(c=>c.Id==updatedCharacter.Id);
            if (character is null)
            {
                throw new Exception($"Character with Id: '{updatedCharacter.Id}' not found.");
            }
            character.Name=updatedCharacter.Name;
            character.HitPoints=updatedCharacter.HitPoints;
            character.Strenght=updatedCharacter.Strenght;
            character.Defense=updatedCharacter.Defense;
            character.Intelligence=updatedCharacter.Intelligence;
            character.Class=updatedCharacter.Class;

            serviceResponse.Data=_mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex)
            {
                serviceResponse.Sucess=false;
                serviceResponse.Message=ex.Message;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse=new ServiceResponse<List<GetCharacterDto>>();
            try{
            var character=characters.First(c=>c.Id==id);
            if (character is null)
            {
                throw new Exception($"Character with Id: '{id}' not found.");
            }
            characters.Remove(character);
            

            serviceResponse.Data=characters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch(Exception ex)
            {
                serviceResponse.Sucess=false;
                serviceResponse.Message=ex.Message;
            }
            return serviceResponse;

        }
    }
}  