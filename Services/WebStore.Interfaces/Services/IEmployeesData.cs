using System.Collections.Generic;
using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();

        Employee GetById(int id);

        void Add(Employee Employee);

        void Edit(int id, Employee Employee);

        bool Delete(int id);

        void SaveChanges();
    }
}
