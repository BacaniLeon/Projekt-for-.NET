using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public Game Game { get; set; }
    }

}
