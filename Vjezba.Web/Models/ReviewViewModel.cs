using System.ComponentModel.DataAnnotations;

namespace Vjezba.Web.Models
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public int GameId { get; set; }
   
    }


}
