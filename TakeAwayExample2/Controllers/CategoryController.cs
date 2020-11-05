using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TakeAwayExample2.DataAccess.Repository.IRepository;

namespace TakeAwayExample2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.Category.GetAll() });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(c => c.CategoryID == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error occured while deleting" });
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Item deleted successfully" });
        }

    }
}
