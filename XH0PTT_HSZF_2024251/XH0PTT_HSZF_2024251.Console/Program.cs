using XH0PTT_HSZF_2024251.Persistance.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XH0PTT_HSZF_2024251.Model;
using XH0PTT_HSZF_2024251.Application;
using XH0PTT_HSZF_2024251.Model.Dtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace XH0PTT_HSZF_2024251.Console
{
    public class Program
    {
        static GotDbContext ctx;
        static IHouseDataProvider hDataProvider;
        static IHouseService hService;
        static ICharacterDataProvider cDataProvider;
        static ICharacterService cService;
        static void StrongHouseAdded()
        {
            System.Console.WriteLine("A strong house has been added.");
        }

        static void AddHouse()
        {
            string name;
            string ruler;
            int armySize;
            int establishedYear;
            string seat;
            do
            {
                System.Console.WriteLine("House name:");
                name = System.Console.ReadLine();
            } while (name == "");
            do
            {
                System.Console.WriteLine("House ruler:");
                ruler = System.Console.ReadLine();
            } while (ruler == "");
            do
            {
                System.Console.WriteLine("House armySize:");
                armySize = int.Parse(System.Console.ReadLine());
            } while (armySize == 0);
            do
            {
                System.Console.WriteLine("House established year:");
                establishedYear = int.Parse(System.Console.ReadLine());
            } while (establishedYear == 0);
            do
            {
                System.Console.WriteLine("House seat:");
                seat = System.Console.ReadLine();
            } while (name == "");

            House newHouse = new House(name, ruler, armySize, establishedYear, seat);

            if (hService.AddHouse(newHouse))
            {
                System.Console.WriteLine($"House {newHouse.Name} added.");
            }
            else
            {
                System.Console.WriteLine($"There is already a house named {newHouse.Name}.");
            }
        }
        static void AddCharacter()
        {
            string name;
            int battleCount;
            string houseName;
            do
            {
                System.Console.WriteLine("Character name:");
                name = System.Console.ReadLine();
            } while (name == "");
            do
            {
                System.Console.WriteLine("Character battle count:");
                battleCount = int.Parse(System.Console.ReadLine());
            } while (battleCount == 0);
            do
            {
                System.Console.WriteLine("Character house name:");
                houseName = System.Console.ReadLine();
            } while (houseName == "");

            Character newCharacter = new Character(name, battleCount);
            var charHouse = hDataProvider.GetHouses()
                .FirstOrDefault(x => x.Name == houseName);
            if (charHouse == null)
            {
                AddHouse();
                charHouse = hDataProvider.GetHouses()
                .FirstOrDefault(x => x.Name == houseName);
            }
            newCharacter.House = charHouse;
            newCharacter.HouseId = charHouse.Id;
        }


        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) => {
                services.AddScoped<GotDbContext>();

                services.AddSingleton<ICharacterDataProvider, CharacterDataProvider>();
                services.AddSingleton<IHouseDataProvider, HouseDataProvider>();

                services.AddSingleton<ICharacterService, CharacterService>();
                services.AddSingleton<IHouseService, HouseService>();
            }).Build();

            host.Start();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            cDataProvider = serviceProvider.GetRequiredService<ICharacterDataProvider>();
            hDataProvider = serviceProvider.GetRequiredService<IHouseDataProvider>();
            cService = serviceProvider.GetRequiredService<ICharacterService>();
            hService = serviceProvider.GetRequiredService<IHouseService>();

            hDataProvider.strongHouseAdded += StrongHouseAdded;

            int option = -1;
            do
            {
                string optionText = "1. Import Data\n2. Add house\n3. Add Character\n4. Create army count\n5. List characters by battle count\n6. House with most allies\n" +
                    "7. Average battle count in a house\n8. Houses ordered by establish year\n9. Count of allies battle count\n10. Save datas into Json files\n0. Exit";
                System.Console.WriteLine(optionText);
                string? input = System.Console.ReadLine();
                if (input == "" || !input.All(char.IsDigit))
                    continue;

                option = int.Parse(input);

                switch (option)
                {
                    case 1:
                        hDataProvider.ImportXmlData();
                        System.Console.WriteLine("Import done.");
                        hService.CreateHouseFolders();
                        break;    
                    case 2:
                        //Add house
                        AddHouse();
                        break;
                    case 3:
                        //Add character
                        AddCharacter();
                        break;
                    case 4:
                        //Create army count files
                        hService.CreateArmyCountReport();
                        System.Console.WriteLine("Reports created.");
                        break;
                    case 5:
                        //List characters
                        var charactersByBattleCount = cService.CharactersByBattleCount();
                        System.Console.WriteLine("Characters sorted by battle count (descending):");
                        foreach (var character in charactersByBattleCount)
                        {
                            System.Console.WriteLine($"{character.Name}: {character.BattleCount} battles");
                        }
                        break;
                    case 6:
                        //House with most allies
                        var houseWithMostAllies = hService.HouseWithMostAllies();
                        System.Console.WriteLine($"House with the most allies: {houseWithMostAllies.Name} ({houseWithMostAllies.AlliesCount} allies)");
                        break;
                    case 7:
                        //Avg battle count of charachters in houses
                        System.Console.WriteLine("Name of house:");
                        string? houseName7 = System.Console.ReadLine();
                        if (houseName7 != null)
                        {
                            var averageBattleCount = hService.AverageBattleCount(houseName7);
                            System.Console.WriteLine($"Average battle participation for house '{houseName7}': {averageBattleCount}");
                        }
                        break;
                    case 8:
                        //Houses ordered by establish year 
                        List<HousesByEstablishmentYearDto> housesByEstablishmentYear = hService.HousesByEstablishmentYear();
                        System.Console.WriteLine("Houses ranked by year of establishment:");
                        foreach (var house in housesByEstablishmentYear)
                        {
                            System.Console.WriteLine($"{house.Name} - Established: {house.EstablishedYear}, Seat: {house.Seat}");
                        }
                        break;
                    case 9:
                        //Count of allies battle count
                        System.Console.WriteLine("Name of house:");
                        string? houseName9 = System.Console.ReadLine();
                        if (houseName9 != null)
                        {
                            List<TotalAlliesBattleCountDto> totalAlliesBattleCount = hService.TotalAlliesBattleCount(houseName9);
                            System.Console.WriteLine($"Total battle participation of allies for house '{houseName9}':");
                            foreach (var ally in totalAlliesBattleCount)
                            {
                                System.Console.WriteLine($"{ally.HouseName}: {ally.TotalBattleCount} battles");
                            }
                        }
                        break;
                    case 10:
                        //Save data in Json 
                        hService.CreateJson();
                        System.Console.WriteLine("Data saved to Json.");
                        break;
                    default:
                        continue;
                }
                System.Console.WriteLine("\nPress any key to continue...");
                System.Console.ReadKey();
                System.Console.Clear();

            } while (option != 0);
        }
    }
}
