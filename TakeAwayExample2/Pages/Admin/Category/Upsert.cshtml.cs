using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TakeAwayExample2.DataAccess.Repository.IRepository;

namespace TakeAwayExample2.Pages.Admin.Category
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Models.Category CategoryObj { get; set; }

        public IActionResult OnGet(int? id)
        {
            CategoryObj = new Models.Category();

            if (id != null)
            {
                CategoryObj = _unitOfWork.Category.GetFirstOrDefault(c => c.CategoryID == id);

                if (CategoryObj == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost(Models.Category CategoryObj)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if(CategoryObj.CategoryID == 0)
            {
                _unitOfWork.Category.CreateItem(CategoryObj);
            }
            else
            {
                _unitOfWork.Category.UpdateItem(CategoryObj);
            }

            _unitOfWork.Save();

            return RedirectToPage("./Index");
        }

    }
}
