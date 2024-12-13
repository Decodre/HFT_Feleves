using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XH0PTT_HSZF_2024251.Model;
using XH0PTT_HSZF_2024251.Model.Dtos;
using XH0PTT_HSZF_2024251.Persistance.MsSql;

namespace XH0PTT_HSZF_2024251.Application
{
    public class HouseService : IHouseService
    {
        IHouseDataProvider dp;

        const string mainFolder = "Westeros";

        public HouseService(IHouseDataProvider dp)
        {
            this.dp = dp;
        }

        public void CreateHouseFolders()
        {
            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }

            foreach (var item in dp.GetHouses())
            {
                string folderPath = Path.Combine(mainFolder, item.Name);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
        }

        public void UpdateHouseFolders(House h)
        {
            string folderPath = Path.Combine(mainFolder, h.Name);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public HouseWithMostAlliesDto HouseWithMostAllies()
        {
            var houseWithMostAllies = dp.HouseWithMostAllies();

            if (houseWithMostAllies != null)
            {
                return houseWithMostAllies;
            }
            return new HouseWithMostAlliesDto();
        }

        public double AverageBattleCount(string houseName)
        {
            var averageBattleCount = dp.AverageBattleCount(houseName);
            averageBattleCount = Math.Round(averageBattleCount);

            return averageBattleCount;
        }

        public List<HousesByEstablishmentYearDto> HousesByEstablishmentYear()
        {
            return dp.HousesByEstablishmentYear();
        }

        public List<TotalAlliesBattleCountDto> TotalAlliesBattleCount(string houseName)
        {
            return dp.TotalAlliesBattleCount(houseName);
        }

        public bool AddHouse(House newHouse)
        {
            bool added = dp.AddHouse(newHouse);
            if (added)
            {
                UpdateHouseFolders(newHouse);
            }
            return added;
        }

        public void CreateArmyCountReport()
        {
            var houseSummaries = dp.GetHouses();

            foreach (var house in houseSummaries)
            {
                string houseDirectory = Path.Combine(mainFolder, house.Name);
                string fileName = $"allies_armyCount_{house.Name}.txt";
                string filePath = Path.Combine(houseDirectory, fileName);

                string reportContent = $"House Name: {house.Name}\n" +
                                       $"Total Allies: {house.Allies.Count}\n" +
                                       $"Army Strength: {house.ArmySize}";

                if (!Directory.Exists(houseDirectory))
                    continue;

                File.WriteAllText(filePath, reportContent);

            }
        }

        public void CreateJson()
        {
            var houses = dp.GetHousesForSerialization();

            foreach (var house in houses)
            {
                string houseDirectory = Path.Combine(mainFolder, house.Name);

                string fileName = $"house_{house.Name}.json";
                string filePath = Path.Combine(houseDirectory, fileName);

                var jsonContent = JsonConvert.SerializeObject(house, Formatting.Indented);

                File.WriteAllText(filePath, jsonContent);
            }
        }
    }
}
