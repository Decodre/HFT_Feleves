using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XH0PTT_HSZF_2024251.Model
{
    public class House
    {
        public House(string name, string ruler, int armySize, int establishedYear, string seat)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Ruler = ruler;
            ArmySize = armySize;
            EstablishedYear = establishedYear;
            Seat = seat;
            Allies = new List<string>();
            Characters = new HashSet<Character>();
        }

        public House()
        {
            Allies = new List<string>();
            Characters = new HashSet<Character>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } // Primary key
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        [StringLength(100)]
        [Required]
        public string Ruler { get; set; }
        [Required]
        public int ArmySize { get; set; }
        [Required]
        public int EstablishedYear { get; set; }
        [StringLength(100)]
        [Required]
        public string Seat { get; set; }
        public List<string> Allies { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
