using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TakeAwayExample2.DataAccess.Repository.IRepository;

namespace TakeAwayExample2.Pages.Admin.MenuItemBatch
{
    public class BatchUpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnviroment;

        public BatchUpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnviroment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnviroment = hostingEnviroment;
        }

        [BindProperty]
        public Models.MenuItem MenuItemObj { get; set; }

        /*
        *This will hold all category entries in the DB.
        *This is done so that the GetAll is called only once, rather than calling it in loop when checking for the keys
        */
        private List<Models.Category> categoryList = new List<Models.Category>();
        private List<Models.FoodType> foodTypeList = new List<Models.FoodType>();
        //This must be called before we execute the excel reader

        public void OnGet()
        {
            
        }

        public void OnPost(IFormFile excelFile)
        {
            //documents the name of the folder that will store the excell uploads
            //?file name may need to be changed?
            string fileName = $"{_hostingEnviroment.WebRootPath}\\documents\\{excelFile.FileName}";

            //This creates a copy of the excel file that user uploads on the local directory
            using (FileStream fileStream = System.IO.File.Create(fileName)) 
            {
                excelFile.CopyTo(fileStream);
                fileStream.Flush();
            }

            addToCategoriesList();
            addToFoodTypeList();
            executeExcelReader(fileName);
        }
                
        //Adds all the entries in the DB to the category list
        private void addToCategoriesList()
        {
            foreach (var item in _unitOfWork.Category.GetAll())
            {
                categoryList.Add(item);
            }
        }

        private void addToFoodTypeList()
        {
            foreach (var item in _unitOfWork.FoodType.GetAll())
            {
                foodTypeList.Add(item);
            }
        }

        private void executeExcelReader(string fileName)
        {
            MenuItemObj = new Models.MenuItem();

            var documentPath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\documents"}" + "\\" + fileName;

            using (var stream = System.IO.File.Open(documentPath, FileMode.Open, FileAccess.Read))
            {
                using (var r = ExcelReaderFactory.CreateReader(stream))
                {
                    while (r.Read())
                    {
                        MenuItemObj.MenuItemName = r.GetValue(0).ToString();
                        MenuItemObj.MenuItemPrice = (Decimal) r.GetValue(1); //need to check 4 decimal (convert to if needed)

                        //The r.GetValue is the column position in the excel sheet
                        //?temp needs to pass an argument in this function (it works because we hav default arg)
                        MenuItemObj.CategoryID = getCategoryKey(r.GetValue(3).ToString());
                        MenuItemObj.FoodTypeID = getFoodTypeKey(r.GetValue(4).ToString());

                        //then save to the db
                        _unitOfWork.MenuItem.Add(MenuItemObj);
                        //do i need a save changes?
                    }
                }
            }
        }

        //This works similar to getCategoryKey method
        private int getFoodTypeKey(string foodTypeName = "Meat")
        {
            foodTypeName = nameSanitizer(foodTypeName);

            var F = foodTypeList.Where(f => f.FoodTypeName == foodTypeName);

            if (F.Count() == 0)
            {
                createFoodType(foodTypeName);
                getFoodTypeKey(foodTypeName);
            }

            int pk = F.Single().FoodTypeID;
            return pk;
        }

        private int getCategoryKey(string cateroryName = "test 2") //this is the correct signature
        {
            cateroryName = nameSanitizer(cateroryName);
            //Gets the category object for the list
            var C = categoryList.Where(c => c.CategoryName == cateroryName);
            
            //Need to create category because there are none in the DB     
            if (C.Count() == 0)
            {
                createCategory(cateroryName);
                //Calls this function again to get the category PK
                getCategoryKey(cateroryName);
            }

            int pk = C.Single().CategoryID;
            return pk;

        }

        //call the create when category not in the db (in the list)
        private void createCategory(string categoryName)
        {
            //aleter data to write to the db (assigns the values from the excel reader)
            var catObj = new Models.Category();
            catObj.CategoryName = categoryName;
            catObj.DisplayOrder = 1;//need to figure a way to dynamicallly adjust this
            _unitOfWork.Category.Add(catObj);
        }

        private void createFoodType(string foodTypeName)
        {
            var cartObj = new Models.FoodType();
            cartObj.FoodTypeName = foodTypeName;
            _unitOfWork.FoodType.Add(cartObj);
        }

        //do sanitize check that makes the first letter capital
        private string nameSanitizer(string name)
        {
            //Checks if the first letter of the string is capital
            if (!char.IsUpper(name[0]))
            {
                //Changes the first char in the string chanes it to upper, 
                //then concat this char to the string minus its first char(the one we changed) 
                name = name.First().ToString().ToUpper() + name.Substring(1).ToLower();
            }
            else
            {
                //Excludes the first char because it's already capitalised and the rest is lowercased
                name = name.Substring(0, 1) + name.Substring(1).ToLower();
            }            

            return name;
        }
    }
}
