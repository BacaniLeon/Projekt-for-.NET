using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Web.Models;



namespace Vjezba.Web.Controllers
{
    [Route("games")]
    public class GameController(GamesDbContext _dbContext) : Controller
    {
        [Route("all-games")]
        public IActionResult Games()
        {
            var games = _dbContext.Games
                .Include(g => g.Publisher)
                .Include(g => g.Reviews)
                .Include(g => g.Category)
                .Select(g => new GameViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    Grade = g.Grade,
                    PictureURL = g.PictureURL,
                    PublisherName = g.Publisher.Name,
                    CategoryName = g.Category.Name,
                    AverageRating = g.Reviews.Any() ? g.Reviews.Average(r => r.Rating) : 0
                }).ToList();

            return View(games);
        }


        [HttpGet]
        [Route("all-games/{id}")]
        public IActionResult GameDetail(int id)
        {
            var game = _dbContext.Games
                .Include(g => g.Publisher)
                .Include(g => g.Reviews)
                .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            var model = new GameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Grade = game.Grade,
                PictureURL = game.PictureURL,
                PublisherId = game.PublisherId,
                PublisherName = game.Publisher?.Name,
                AverageRating = game.Reviews.Any() ? game.Reviews.Average(r => r.Rating) : 0,
                Reviews = game.Reviews.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Content = r.Content,
                    Rating = r.Rating,
                    GameId = r.GameId,
                   
                }).ToList()
            };

            return View(model);
        }

        private void FillDropdownValues()
        {
            var selectItems = new List<SelectListItem>();

            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            foreach (var publisher in _dbContext.Publishers)
            {
                listItem = new SelectListItem(publisher.Name, publisher.Id.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.Publishers = selectItems;
        }

        private void FillDropdownValuesCategory()
        {
            var selectItems = new List<SelectListItem>();

            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            foreach (var category in _dbContext.Categories)
            {
                listItem = new SelectListItem(category.Name, category.Id.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.Categories = selectItems;
        }


        public IActionResult AddGame()
        {
            this.FillDropdownValues();
            this.FillDropdownValuesCategory();
            return View();
        }

        [HttpPost]
        public IActionResult AddGame(Game model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Games.Add(model);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Games));
            }
            else
            {
                this.FillDropdownValues();
                return View();
            }
        }

        [ActionName(nameof(Edit))]
        [Route("games/edit-game/{id}")]
        public IActionResult Edit(int id)
        {
            var category = _dbContext.Games.FirstOrDefault(g => g.Id == id);
            this.FillDropdownValues();
            this.FillDropdownValuesCategory();

            if (category == null) { return NotFound(); }

            return View(category);
        }

        [HttpPost]
        [ActionName("Edit")]
        [Route("games/edit-game/{id}")]
        public async Task<IActionResult> EditGame(int id)
        {
            var category = _dbContext.Games.Single(g => g.Id == id);
            var ok = await this.TryUpdateModelAsync(category);


            _dbContext.SaveChanges();
            return RedirectToAction("Games", "Game");


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Game/Delete/{id}")]
        public IActionResult DeleteGame(int id)
        {
            var game = _dbContext.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            _dbContext.Games.Remove(game);
            _dbContext.SaveChanges();

            return RedirectToAction("Games");
        }


        [HttpGet]
        [Route("add-review/{id}")]
        public IActionResult AddReview(int id)
        {
            var model = new ReviewViewModel
            {
                GameId = id
            };
            return View(model);
        }

        [HttpPost]
        [Route("add-review/{id}")]
        public IActionResult AddReview(ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var review = new Review
                {
                    Content = model.Content,
                    Rating = model.Rating,
                    GameId = model.GameId,
                    
                };

                _dbContext.Reviews.Add(review);
                _dbContext.SaveChanges();

                return RedirectToAction("Games");
            }
            return View(model);
        }

    }
}
