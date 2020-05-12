using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Mapping;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    //[Route("users")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData;

        //[Route("employees")]
        public IActionResult Index() => View(_EmployeesData.GetAll().Select(e => e.ToView()));

        //[Route("employee/{Id}")]
        public IActionResult Details(int Id)
        {
            var employee = _EmployeesData.GetById(Id);
            if (employee is null)
                return NotFound();

            return View(employee.ToView());
        }

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Create() => View(new EmployeeViewModel());

        [Authorize(Roles = Role.Administrator)]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel Employee)
        {
            if(Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            if (!ModelState.IsValid)
                return View(Employee);

            _EmployeesData.Add(Employee.FromView());
            _EmployeesData.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(int? Id)
        {
            if (Id is null) return View(new EmployeeViewModel());

            if (Id < 0)
                return BadRequest();

            var employee = _EmployeesData.GetById((int) Id);
            if (employee is null)
                return NotFound();

            return View(employee.ToView());
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel Employee)
        {
            if(Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            if(Employee.Name == "123" && Employee.SecondName == "QWE")
                ModelState.AddModelError(string.Empty, "Странные имя и фамилия...");

            if (!ModelState.IsValid)
                return View(Employee);

            var id = Employee.Id;
            if(id == 0)
                _EmployeesData.Add(Employee.FromView());
            else
                _EmployeesData.Edit(id, Employee.FromView());

            _EmployeesData.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return View(employee.ToView());
        }

        [Authorize(Roles = Role.Administrator)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            _EmployeesData.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}