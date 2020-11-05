using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TakeAwayExample2.DataAccess.Repository.IRepository;

namespace TakeAwayExample2.Pages.Admin.FoodType
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Models.FoodType FoodTypeObj { get; set; }

        public IActionResult OnGet(int? id)
        {
            FoodTypeObj = new Models.FoodType();

            if (id != null)
            {
                FoodTypeObj = _unitOfWork.FoodType.GetFirstOrDefault(f => f.FoodTypeID == id);

                if (FoodTypeObj == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost(Models.FoodType FoodTypeObj)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (FoodTypeObj.FoodTypeID == 0)
            {
                _unitOfWork.FoodType.Add(FoodTypeObj);
            }
            else
            {
                _unitOfWork.FoodType.UpdateItem(FoodTypeObj);
            }

            _unitOfWork.Save();

            return RedirectToPage("./Index");
        }

    }
}