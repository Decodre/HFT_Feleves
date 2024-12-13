using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XH0PTT_HSZF_2024251.Model.Dtos
{
    public class DataJsonExportDto
    {
        public string Name { get; set; }
        public string Ruler { get; set; }
        public int ArmySize { get; set; }
        public int EstablishedYear { get; set; }
        public string Seat { get; set; }
        public List<string> Allies { get; set; }
        public List<CharacterDto> Characters { get; set; }
    }

    public class CharacterDto
    {
        public string Name { get; set; }
        public int BattleCount { get; set; }
    }
}
