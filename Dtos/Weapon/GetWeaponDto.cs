using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Dtos.Weapon
{
    public class GetWeaponDto
    {
        public String Name { get; set; }=string.Empty;
        public int Damage { get; set; }
    }
}