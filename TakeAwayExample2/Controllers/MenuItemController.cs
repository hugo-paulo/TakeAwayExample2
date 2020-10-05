using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TakeAwayExample2.DataAccess.Repository.IRepository;

namespace TakeAwayExample2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MenuItemController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.MenuItem.GetAll(null, null, "Category,FoodType") });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var obj = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.MenuItemID == id);

                if (obj == null)
                {
                    return Json(new { success = false, message = "Error occured while deleting" });
                }

                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, obj.MenuItemImage.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _unitOfWork.MenuItem.DeleteItem(obj);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $@"Error {ex} occured while deleting" });
            }

            return Json(new { success = true, message = "Item deleted successfully" });
        }

    }
}