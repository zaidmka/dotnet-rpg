using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Fight;

namespace dotnet_rpg.Services.FightService
{
    public class FightService:IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

            public async Task<ServiceResponse<List<HighScoreDto>>>GitHighScore()
            {
                var charachters = await _context.Characters
                    .Where(c => c.Fights > 0)
                    .OrderByDescending(c => c.Victories)
                    .ThenBy(c => c.Defeats)
                    .ToListAsync();
                var response = new ServiceResponse<List<HighScoreDto>>
                {
                    Data = charachters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList()
                };
                return response;
            }
           public async Task<ServiceResponse<FightResultDto>>Fight(FightRequestDto request)
           {
                var response = new ServiceResponse<FightResultDto>
                {
                    Data = new FightResultDto()
                };
                try
                {
                    var charachters = await _context.Characters
                        .Include(c => c.Weapon)
                        .Include(c => c.Skills)
                        .Where(c => request.CharachtersIds.Contains(c.Id)).ToListAsync();
                
                    bool defeated = false;

                    while(!defeated)
                    {
                        foreach(var attacker in charachters)
                        {
                            var opponents = charachters.Where(c => c.Id != attacker.Id).ToList();
                            var opponent = opponents[new Random().Next(opponents.Count)];
                            int damage = 0;
                            string attackUsed = string.Empty;
                            bool useWeapon = new Random().Next(2) == 0;
                            if (useWeapon && attacker.Weapon is not null)
                            {
                                attackUsed = attacker.Weapon.Name;
                                damage = DoWeaponAttack(attacker, opponent);
                            }
                            else if(!useWeapon && attacker.Skills is not null)
                            {
                                var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                                attackUsed = skill.Name;
                                damage = DoSkillAttack(attacker, opponent, skill);
                            }
                            else
                            {
                                response.Data.Log.Add($"{attacker.Name} has no available weapon or skill! {attacker.Name} failed to attack!");
                                continue;
                            }
                            response.Data.Log.Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage!");
                            if (opponent.HitPoints <= 0)
                            {
                                defeated = true;
                                attacker.Victories++;
                                opponent.Defeats++;
                                response.Data.Log.Add($"{opponent.Name} has been defeated!");
                                response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left!");
                                break;
                            }
                        }
                    }
                    charachters.ForEach(c =>
                    {
                        c.Fights++;
                        c.HitPoints = 100;
                    });
                    await _context.SaveChangesAsync();
                }
                
                catch(Exception ex)
                {
                    response.Sucess = false;
                    response.Message = ex.Message;
                }

                return response;


           }


           public async Task<ServiceResponse<AttackResultDto>>SkillAttack(SkillAttackDto request)
            {
                var response = new ServiceResponse<AttackResultDto>();
                try
            {
                var Attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.AttacherId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (Attacker is null || opponent is null || Attacker.Skills is null)
                {
                    throw new Exception("Attacker or Opponent or Weapon was not found");
                }
                var skill = Attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill is null)
                {
                    throw new Exception("Skill was not found");
                }
                int damage = DoSkillAttack(Attacker, opponent, skill);
                if (opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated!";
                }
                await _context.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacher = Attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = Attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };

            }
            catch (Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static int DoSkillAttack(Character Attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(Attacker.Intelligence));
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>>WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var Attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (Attacker is null || opponent is null || Attacker.Weapon is null)
                {
                    throw new Exception("Attacker or Opponent or Weapon was not found");
                }
                int damage = DoWeaponAttack(Attacker, opponent);
                if (opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated!";
                }
                await _context.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacher = Attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = Attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };

            }
            catch (Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static int DoWeaponAttack(Character Attacker, Character opponent)
        {
            if (Attacker.Weapon is null)
            {
                throw new Exception($"{Attacker.Name} has no weapon!");
            }
            int damage = Attacker.Weapon.Damage + (new Random().Next(Attacker.Strenght));
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }
    }
}