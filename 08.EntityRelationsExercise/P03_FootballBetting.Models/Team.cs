using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class Team
    {  

        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LogoUrl { get; set; }

        [Required]
        
        public string Initials { get; set; }

        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }
        public virtual Color PrimaryKitColor { get; set; }

        public int SecondaryKitColorId { get; set; }
        public  virtual Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }
        public  virtual Town Town { get; set; }

        public virtual ICollection<Player> Players { get; set; } = new HashSet<Player>();

        public virtual ICollection<Game> HomeGames { get; set; } = new HashSet<Game>();
        public virtual ICollection<Game> AwayGames { get; set; } = new HashSet<Game>();
    }
}
