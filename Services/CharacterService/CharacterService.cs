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
        
        public List<Character> AddCharacter(Character newCharacter)
        {
            characters.Add(newCharacter);
            return characters;        
        }

        public List<Character> GetAllCharacter()
        {
            return characters;
        }

        public Character GetCharacterById(int seq)
        {
            return characters.FirstOrDefault(p=>p.Id==seq);
        }
    }
} 