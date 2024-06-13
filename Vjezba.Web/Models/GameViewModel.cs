using Vjezba.Model;

namespace Vjezba.Web.Models
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Grade { get; set; }
        public string PictureURL { get; set; }
        public int? PublisherId { get; set; }
        public string PublisherName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double AverageRating { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }

        public static GameViewModel FromGame(Game game)
        {
            return new GameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Grade = game.Grade,
                PictureURL = game.PictureURL,
                PublisherId = game.PublisherId,
                CategoryId = game.CategoryId,

            };
        }
    }
}
