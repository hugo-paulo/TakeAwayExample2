using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Utility;

namespace TakeAwayExample2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = SD.ManagerRole)]
    public class FoodTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.FoodType.GetAll() });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.FoodType.GetFirstOrDefault(f => f.FoodTypeID == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error occured while deleting" });
            }

            _unitOfWork.FoodType.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Item deleted successfully" });
        }

    }
}
