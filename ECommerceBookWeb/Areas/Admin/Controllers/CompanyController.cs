using ECommerceBook.DataAccess.Repository.IRepository;
using ECommerceBook.Models;
using ECommerceBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        
        private IUnitOfWork unitOfWork;
        public CompanyController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Company> objCompanyList = unitOfWork.CompanyRepository.GetAll();
            return View(objCompanyList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Company comp)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CompanyRepository.Add(comp);
                unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            return View(comp);

        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            Company? companyFromDb = unitOfWork.CompanyRepository.Get(u => u.Id == id);
            if (companyFromDb == null)
            {
                return NotFound();
            }
            return View(companyFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Company company) {
            if (ModelState.IsValid)
            {
                unitOfWork.CompanyRepository.Update(company);
                unitOfWork.Save();
                TempData["success"] = "Company updated successfully";
                return RedirectToAction("Index");
            }
            return View(company);
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company? companyFromDb = unitOfWork.CompanyRepository.Get(u => u.Id == id);
            if (companyFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            unitOfWork.CompanyRepository.Remove(companyFromDb);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var CompanyList = unitOfWork.CompanyRepository.GetAll()
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    streetAddress = p.StreetAddress,
                    city = p.City,
                    state = p.State,
                    postalCode = p.PostalCode,
                    phoneNumber=p.PhoneNumber
                }).ToList();

            return Json(new { data = CompanyList });
        }
        #endregion
    }
}
