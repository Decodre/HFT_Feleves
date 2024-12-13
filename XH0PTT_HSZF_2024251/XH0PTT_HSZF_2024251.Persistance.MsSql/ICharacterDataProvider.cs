using XH0PTT_HSZF_2024251.Model;

namespace XH0PTT_HSZF_2024251.Persistance.MsSql
{
    public interface ICharacterDataProvider
    {
        void AddCharacter(Character c);
        IEnumerable<Character> GetCharacters();
        void ModifyCharacter(Character character);
        void RemoveCharacter(string characterId);
        IEnumerable<Character> CharactersByBattleCount();
    }
}