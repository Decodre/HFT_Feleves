using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using XH0PTT_HSZF_2024251.Model;

namespace XH0PTT_HSZF_2024251.Persistance.MsSql
{
    public class CharacterDataProvider : ICharacterDataProvider
    {
        GotDbContext ctx;

        public CharacterDataProvider(GotDbContext ctx)
        {
            this.ctx = ctx;
        }

        public void AddCharacter(Character c)
        {
            ctx.Characters.Add(c);
            ctx.SaveChanges();
        }

        public void RemoveCharacter(string characterId)
        {
            var characterToRemove = ctx.Characters.FirstOrDefault(x => x.Id == characterId);
            if (characterToRemove != null)
            {
                ctx.Characters.Remove(characterToRemove);
                ctx.SaveChanges();
            }
        }

        public void ModifyCharacter(Character character)
        {
            var CharacterToEdit = ctx.Characters.FirstOrDefault(x => x.Id == character.Id);
            if (CharacterToEdit != null)
            {
                CharacterToEdit.BattleCount = character.BattleCount;
                CharacterToEdit.Name = character.Name;

                ctx.SaveChanges();
            }
        }

        public IEnumerable<Character> GetCharacters()
        {
            return ctx.Characters;
        }

        public IEnumerable<Character> CharactersByBattleCount()
        {
            return ctx.Characters
            .OrderByDescending(c => c.BattleCount)
            .ToList();
        }
    }
}
