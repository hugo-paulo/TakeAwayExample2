using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Stripe;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Utility;

namespace TakeAwayExample2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = SD.ManagerRole)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.User.GetAll() });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var obj = _unitOfWork.User.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            //Method that checks if user has not been locked out
            _unitOfWork.User.LoginGracePeriod(obj);

            return Json(new { success = true, message = "Operation successful" });
        }

    }
}
