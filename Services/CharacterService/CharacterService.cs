using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
    {
            _context = context;
            _mapper = mapper;
        }
    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddcharacterDto newCharacter)
        {
            var ServiceResponse=new ServiceResponse<List<GetCharacterDto>>();
            var character= _mapper.Map<Character>(newCharacter);
            
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            ServiceResponse.Data=
            await _context.Characters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return ServiceResponse;        
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            var ServiceResponse=new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            ServiceResponse.Data=dbCharacters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToList();

            return ServiceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int seq)
        {
            var ServiceResponse=new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(p=>p.Id==seq);
            ServiceResponse.Data= _mapper.Map<GetCharacterDto>(dbCharacter);
            return ServiceResponse;        

        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse=new ServiceResponse<GetCharacterDto>();
            try{
            var character=
            await _context.Characters.FirstOrDefaultAsync(c=>c.Id==updatedCharacter.Id);
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
            await _context.SaveChangesAsync();
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
            var character= await _context.Characters.FirstOrDefaultAsync(c=>c.Id==id);
            if (character is null)
            {
                throw new Exception($"Character with Id: '{id}' not found.");
            }
            _context.Characters.Remove(character);
            
            await _context.SaveChangesAsync();
            serviceResponse.Data=
            await _context.Characters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToListAsync();
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