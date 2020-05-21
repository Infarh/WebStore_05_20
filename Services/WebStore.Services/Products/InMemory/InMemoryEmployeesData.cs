using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Products.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ILogger<InMemoryEmployeesData> _Logger;
        private readonly List<Employee> _Employees = TestData.Employees;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
        {
            _Logger = Logger;
            _Logger.LogInformation("Инициализация сервиса сотрудников завершена. В списке {0} сотрудников", _Employees.Count);
        }

        public IEnumerable<Employee> GetAll()
        {
            _Logger.LogInformation("Запрос списка сотрудников. Возращено {0} сотрудников", _Employees.Count);
            return _Employees;
        }

        public Employee GetById(int id)
        {
            _Logger.LogInformation("Запрошен сотрудник с id {0}", id);
            var employee = _Employees.FirstOrDefault(e => e.Id == id);
            if (employee is null)
                _Logger.LogInformation("Сотрудник с id:{0} не найден!", id);
            else
                _Logger.LogInformation("Найден сотрудник {0}", employee);

            return employee;
        }

        public void Add(Employee Employee)
        {
            _Logger.LogInformation("Попытка добавления сотрудника {0}", Employee);
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            if (_Employees.Contains(Employee))
            {
                _Logger.LogWarning("Сотрудник {0} уже существует", Employee);
                return;
            }
            Employee.Id = _Employees.Count == 0 ? 1 : _Employees.Max(e => e.Id) + 1;
            _Employees.Add(Employee);
            _Logger.LogInformation("Сотрудник {0} добавлен", Employee);
        }

        public void Edit(int id, Employee Employee)
        {
            _Logger.LogInformation("Попытка редактирования сотрудника id:{0} = {1}", id, Employee);
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            if (_Employees.Contains(Employee))
            {
                _Logger.LogWarning("Сотрудник {0} уже существует в памяти списка", Employee);
                return;
            }

            var db_employee = GetById(id);
            if (db_employee is null)
            {
                _Logger.LogWarning("Сотрудник {0} не найден", Employee);
                return;
            }

            db_employee.SurName = Employee.SurName;
            db_employee.FirstName = Employee.FirstName;
            db_employee.Patronymic = Employee.Patronymic;
            db_employee.Age = Employee.Age;

            _Logger.LogInformation("Сотрудник {0} отредактирован", Employee);
        }

        public bool Delete(int id)
        {
            _Logger.LogInformation("Попытка удаления сотрудника id:{0}", id);

            var db_employee = GetById(id);
            if (db_employee is null)
            {
                _Logger.LogWarning("Сотрудник id:{0} не найден", id);
                return false;
            }

            var remove_result = _Employees.Remove(db_employee);
            if(remove_result)
                _Logger.LogInformation("Сотрудник {0} удалён", db_employee);
            else
                _Logger.LogWarning("Сотрудник {0} отсутствовал в списке!", db_employee);

            return remove_result;
        }

        public void SaveChanges() { }
    }
}
