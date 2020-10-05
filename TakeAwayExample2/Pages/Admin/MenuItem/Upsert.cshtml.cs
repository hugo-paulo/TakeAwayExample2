using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models.ViewModels;

namespace TakeAwayExample2.Pages.Admin.MenuItem
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        //Used to move images onto the server
        private readonly IWebHostEnvironment _hostEnvironment;

        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public MenuItemVM MenuItemObj { get; set; }
        //A view model is needed for drop downlist
        //Can use full path, this better (same as other pages)

        public IActionResult OnGet(int? id)
        {
            //Instantiating and Initializing the object (thus assigning the properties)
            MenuItemObj = new MenuItemVM
            {
                MenuItem = new Models.MenuItem(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown(),
                FoodTypeList = _unitOfWork.FoodType.GetFoodTypeListForDropDown()                
            };

            if (id != null)
            {
                MenuItemObj.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(i => i.MenuItemID == id);

                if (MenuItemObj.MenuItem == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            //Gets the path to the wwwroot folder
            string webRootPath = _hostEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (MenuItemObj.MenuItem.MenuItemID == 0)
            {
                //seems this block can be extracted? its duplicated on line 93
                string fileName = Guid.NewGuid().ToString();

                var uploads = Path.Combine(webRootPath, @"images\menuItems");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                MenuItemObj.MenuItem.MenuItemImage = @"\images\menuItems\" + fileName + extension;

                _unitOfWork.MenuItem.AddItem(MenuItemObj.MenuItem);
            }
            else
            {
                //Editing the Menu Item

                var obj = _unitOfWork.MenuItem.GetSingle(MenuItemObj.MenuItem.MenuItemID);

                //The file has been uploaded
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var uploads = Path.Combine(webRootPath, @"images\menuItems");
                    var extension = Path.GetExtension(files[0].FileName);

                    var imagePath = Path.Combine(webRootPath, obj.MenuItemImage.TrimStart('\\'));

                    //Remove the original 
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    //Adds the new file
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    MenuItemObj.MenuItem.MenuItemImage = @"\images\menuItems\" + fileName + extension;

                }
                else
                {
                    //When a update happens without an image update, eg update the name or price

                    MenuItemObj.MenuItem.MenuItemImage = obj.MenuItemImage;
                }

                //Sanitize the user input (we should use this for all input)
                MenuItemObj.MenuItem.MenuItemDescription = InputSanitize(MenuItemObj.MenuItem.MenuItemDescription);

                _unitOfWork.MenuItem.UpdateItem(MenuItemObj.MenuItem);
            }

            _unitOfWork.Save();

            return RedirectToPage("./Index");
        }

        //This should be extracted and place centrally in file heirachy and visibity changed to public
        private string InputSanitize(string userInput)
        {
            return Regex.Replace(userInput, "<.*?>", string.Empty);
        }

    }
}
