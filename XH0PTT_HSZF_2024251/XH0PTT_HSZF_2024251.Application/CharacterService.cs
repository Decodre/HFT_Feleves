using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XH0PTT_HSZF_2024251.Model;
using XH0PTT_HSZF_2024251.Persistance.MsSql;

namespace XH0PTT_HSZF_2024251.Application
{
    public class CharacterService : ICharacterService
    {
        ICharacterDataProvider dp;

        public CharacterService(ICharacterDataProvider dp)
        {
            this.dp = dp;
        }

        public void AddCharacter(Character newCharacter)
        {
            dp.AddCharacter(newCharacter);
        }

        public IEnumerable<Character> CharactersByBattleCount()
        {
            return dp.CharactersByBattleCount();
        }
    }
}
