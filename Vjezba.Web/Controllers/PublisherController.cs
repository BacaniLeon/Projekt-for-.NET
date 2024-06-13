using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL; // Assuming this is the namespace where your DbContext is located
using Vjezba.Model;
using Vjezba.Web.Models; // Assuming this is the namespace where your ViewModels are located

namespace Vjezba.Web.Controllers
{
    [Route("publisher")]
    public class PublisherController(GamesDbContext _dbContext) : Controller
    {
        [Route("all-publishers")]
        public IActionResult Index()
        {
            var publishers = _dbContext.Publishers.ToList();
            return View(publishers);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddPublisher()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public IActionResult AddPublisher(PublisherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var publisher = new Publisher
                {
                    Name = model.Name
                };

                _dbContext.Publishers.Add(publisher);
                _dbContext.SaveChanges();

                return RedirectToAction("Games", "Game");
            }

            return View(model);
        }
        [Route("publisher/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }
        [HttpPost]
        [ActionName("Edit")]
        [Route("publisher/edit/{id}")]
        public async Task<IActionResult> EditCategory(int id)
        {
            var publisher = _dbContext.Publishers.Single(p => p.Id == id);
            var ok = await this.TryUpdateModelAsync(publisher);


            _dbContext.SaveChanges();
            return RedirectToAction("Games", "Game");


        }

        [Route("publisher/delete/{id}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }
            _dbContext.Publishers.Remove(publisher);
            _dbContext.SaveChanges();

            return RedirectToAction("Games", "Game");
        }

        [HttpDelete("publisher/delete/{id}")]
        public IActionResult DeleteAjax(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound("Publisher not found.");
            }

            _dbContext.Publishers.Remove(publisher);
            _dbContext.SaveChanges();

            return Ok(new { message = "Publisher deleted successfully" });
        }

        [HttpGet]
        public IActionResult SearchPublishers(string searchQuery)
        {
            var publishers = _dbContext.Publishers
                .Where(p => p.Name.Contains(searchQuery))
                .ToList();

            return PartialView("_PublisherListPartial", publishers);
        }
    }
}
