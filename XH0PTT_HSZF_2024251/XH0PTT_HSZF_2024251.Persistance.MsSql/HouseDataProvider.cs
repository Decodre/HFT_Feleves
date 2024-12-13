using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using XH0PTT_HSZF_2024251.Model;
using XH0PTT_HSZF_2024251.Model.Dtos;

namespace XH0PTT_HSZF_2024251.Persistance.MsSql
{
    public class HouseDataProvider : IHouseDataProvider
    {
        GotDbContext ctx;

        public event Action strongHouseAdded;

        public HouseDataProvider(GotDbContext ctx)
        {
            this.ctx = ctx;
        }

        public void ImportXmlData()
        {
            string path = "GoT.xml";
            if (!File.Exists(path))
            {
                return;
            }
            XDocument xmlDoc = XDocument.Load(path);

            var houses = xmlDoc.Descendants("House").Select(house => new House
            {
                Name = house.Element("Name")?.Value,
                Ruler = house.Element("Ruler")?.Value,
                ArmySize = int.Parse(house.Element("ArmySize")?.Value ?? "0"),
                EstablishedYear = int.Parse(house.Element("EstablishedYear")?.Value ?? "0"),
                Seat = house.Element("Seat")?.Value,
                Allies = house.Element("Allies")?.Elements("Ally").Select(a => a.Value).ToList(),
                Characters = house.Element("Characters")?.Elements("Character").Select(character => new Character(
                    character.Element("Name")?.Value,
                    int.Parse(character.Element("BattleCount")?.Value ?? "0"))
                ).ToList()
            }).ToList();

            ctx.Houses.AddRange(houses);

            ctx.SaveChanges();
        }

        public bool AddHouse(House h)
        {
            if (ctx.Houses.FirstOrDefault(x => x.Name == h.Name) != null)
            {
                return false;
            }
            ctx.Houses.Add(h);
            if (h.ArmySize > 15000)
            {
                strongHouseAdded?.Invoke();
            }
            ctx.SaveChanges();
            return true;
        }

        public void RemoveHouse(string houseId)
        {
            var houseToRemove = ctx.Houses.FirstOrDefault(x => x.Id == houseId);
            if (houseToRemove != null)
            {
                ctx.Houses.Remove(houseToRemove);
                ctx.SaveChanges();
            }
        }

        public void ModifyHouse(House house)
        {
            var houseToEdit = ctx.Houses.FirstOrDefault(x => x.Id == house.Id);
            if (houseToEdit != null)
            {
                houseToEdit.ArmySize = house.ArmySize;
                houseToEdit.Name = house.Name;
                houseToEdit.Ruler = house.Ruler;
                houseToEdit.EstablishedYear = house.EstablishedYear;
                houseToEdit.Seat = house.Seat;
                houseToEdit.Allies = house.Allies;

                ctx.SaveChanges();
            }
        }

        public IEnumerable<House> GetHouses()
        {
            return ctx.Houses;
        }

        public HouseWithMostAlliesDto HouseWithMostAllies()
        {
            return ctx.Houses
            .OrderByDescending(h => h.Allies.Count)
            .Select(h => new HouseWithMostAlliesDto
            {
                Name = h.Name,
                AlliesCount = h.Allies.Count
            })
            .FirstOrDefault();
        }

        public double AverageBattleCount(string houseName)
        {
            return ctx.Houses
            .Where(h => h.Name == houseName)
            .SelectMany(h => h.Characters)
            .Average(c => c.BattleCount);
        }

        public List<HousesByEstablishmentYearDto> HousesByEstablishmentYear()
        {
            return ctx.Houses
            .OrderBy(h => h.EstablishedYear)
            .Select(h => new HousesByEstablishmentYearDto
            {
                Name = h.Name,
                EstablishedYear = h.EstablishedYear,
                Seat = h.Seat
            })
            .ToList();
        }

        public List<TotalAlliesBattleCountDto> TotalAlliesBattleCount(string houseName)
        {
            if (!ctx.Houses.Any(x => x.Name == houseName))
            {
                throw new Exception($"There is no house named {houseName}");
            }
            var houseAllies = ctx.Houses
                .Where(h => h.Name == houseName)
                .SelectMany(h => h.Allies)
                .ToList();
            return ctx.Houses
                .Where(h => houseAllies.Contains(h.Name))
                .SelectMany(h => h.Characters)
                .GroupBy(c => c.House.Name)
                .OrderBy(g => g.Key)
                .Select(g => new TotalAlliesBattleCountDto { HouseName = g.Key, TotalBattleCount = g.Sum(c => c.BattleCount) })
                .ToList();
        }

        public IEnumerable<DataJsonExportDto> GetHousesForSerialization()
        {
            return GetHouses()
            .Select(h => new DataJsonExportDto
            {
                Name = h.Name,
                Ruler = h.Ruler,
                ArmySize = h.ArmySize,
                EstablishedYear = h.EstablishedYear,
                Seat = h.Seat,
                Allies = h.Allies,
                Characters = h.Characters.Select(c => new CharacterDto
                {
                    Name = c.Name,
                    BattleCount = c.BattleCount
                }).ToList()
            });
        }
    }
}
