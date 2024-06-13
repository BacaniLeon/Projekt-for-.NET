using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1,5)]
        public double Grade { get; set; }
        public string PictureURL { get; set; }
       
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
       
        public int? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }

}
