using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XH0PTT_HSZF_2024251.Model
{
    public class Character
    {
        public Character(string name, int battleCount)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            BattleCount = battleCount;
        }
        public Character()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } // Primary key
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int BattleCount { get; set; }

        // Foreign key
        public string HouseId { get; set; }
        public virtual House House { get; set; }
    }
}
