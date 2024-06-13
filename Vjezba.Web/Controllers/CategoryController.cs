using Microsoft.AspNetCore.Mvc;
using Vjezba.Web.Models;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Web.Controllers
{
    [Route("category")]
    public class CategoryController(GamesDbContext _dbContext) : Controller
    {
        [Route("all-categories")]
        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        [Route("add")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = model.Name
                };

                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();

                return RedirectToAction("Games", "Game");
            }

            return View(model);
        }

        [ActionName(nameof(Edit))]
        [Route("category/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
         
            if(category == null) { return NotFound(); }

            return View(category);
        }

        [HttpPost]
        [ActionName("Edit")]
        [Route("category/edit/{id}")]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = _dbContext.Categories.Single(c => c.Id == id);
            var ok = await this.TryUpdateModelAsync(category);

            
                _dbContext.SaveChanges();
                return RedirectToAction("Games", "Game");
            
            
        }

        [HttpGet]
        [Route("category/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return RedirectToAction("Games", "Game");
        }


        [HttpDelete("category/delete/{id}")]
        public IActionResult DeleteAjax(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return Ok(new { message = "Category deleted successfully" });
        }


    }
}
