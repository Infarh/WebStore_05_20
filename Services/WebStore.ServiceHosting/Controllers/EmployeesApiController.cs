using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesApiController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData;

        [HttpGet]
        public IEnumerable<Employee> GetAll() => _EmployeesData.GetAll();

        async Task<IEnumerable<Employee>> IEmployeesData.GetAllAsync(CancellationToken Cancel) => await _EmployeesData.GetAllAsync(Cancel);

        [HttpGet("{id}")]
        public Employee GetById(int id) => _EmployeesData.GetById(id);

        async Task<Employee> IEmployeesData.GetByIdAsync(int id, CancellationToken Cancel) => await _EmployeesData.GetByIdAsync(id, Cancel);

        [HttpPost]
        public void Add([FromBody] Employee Employee) => _EmployeesData.Add(Employee);

        async Task IEmployeesData.AddAsync(Employee Employee, CancellationToken Cancel) => await _EmployeesData.AddAsync(Employee, Cancel);

        [HttpPut("{id}")]
        public void Edit(int id, [FromBody] Employee Employee) => _EmployeesData.Edit(id, Employee);

        async Task IEmployeesData.EditAsync(int id, Employee Employee, CancellationToken Cancel) => await _EmployeesData.EditAsync(id, Employee, Cancel);

        [HttpDelete("{id}")]
        public bool Delete(int id) => _EmployeesData.Delete(id);

        async Task<bool> IEmployeesData.DeleteAsync(int id, CancellationToken Cancel) => await _EmployeesData.DeleteAsync(id, Cancel);

        [NonAction]
        public void SaveChanges() => _EmployeesData.SaveChanges();

        async Task IEmployeesData.SaveChangesAsync(CancellationToken Cancel) => await _EmployeesData.SaveChangesAsync(Cancel);
    }
}