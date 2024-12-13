using XH0PTT_HSZF_2024251.Model;
using XH0PTT_HSZF_2024251.Model.Dtos;

namespace XH0PTT_HSZF_2024251.Application
{
    public interface IHouseService
    {
        bool AddHouse(House newHouse);
        double AverageBattleCount(string houseName);
        void CreateArmyCountReport();
        void CreateHouseFolders();
        void CreateJson();
        List<HousesByEstablishmentYearDto> HousesByEstablishmentYear();
        HouseWithMostAlliesDto HouseWithMostAllies();
        List<TotalAlliesBattleCountDto> TotalAlliesBattleCount(string houseName);
        void UpdateHouseFolders(House h);
    }
}