using XH0PTT_HSZF_2024251.Model;
using XH0PTT_HSZF_2024251.Model.Dtos;

namespace XH0PTT_HSZF_2024251.Persistance.MsSql
{
    public interface IHouseDataProvider
    {
        event Action strongHouseAdded;

        bool AddHouse(House h);
        IEnumerable<House> GetHouses();
        void ImportXmlData();
        void ModifyHouse(House house);
        void RemoveHouse(string houseId);
        HouseWithMostAlliesDto HouseWithMostAllies();
        double AverageBattleCount(string houseName);
        List<HousesByEstablishmentYearDto> HousesByEstablishmentYear();
        List<TotalAlliesBattleCountDto> TotalAlliesBattleCount(string houseName);
        IEnumerable<DataJsonExportDto> GetHousesForSerialization();
    }
}