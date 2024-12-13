using XH0PTT_HSZF_2024251.Model;

namespace XH0PTT_HSZF_2024251.Application
{
    public interface ICharacterService
    {
        void AddCharacter(Character newCharacter);
        IEnumerable<Character> CharactersByBattleCount();
    }
}