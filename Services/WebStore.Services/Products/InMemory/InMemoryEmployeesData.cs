using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public Task<IEnumerable<Employee>> GetAllAsync(CancellationToken Cancel = default)
        {
            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<IEnumerable<Employee>>(Cancel);

            _Logger.LogInformation("Запрос списка сотрудников. Возращено {0} сотрудников", _Employees.Count);
            return Task.FromResult<IEnumerable<Employee>>(_Employees);
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

        public Task<Employee> GetByIdAsync(int id, CancellationToken Cancel = default)
        {
            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<Employee>(Cancel);

            var employee = _Employees.FirstOrDefault(e => e.Id == id);

            if (employee is null)
                _Logger.LogInformation("Сотрудник с id:{0} не найден!", id);
            else
                _Logger.LogInformation("Найден сотрудник {0}", employee);

            return Task.FromResult(employee);
        }

        public int Add(Employee Employee)
        {
            _Logger.LogInformation("Попытка добавления сотрудника {0}", Employee);
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            if (_Employees.Contains(Employee))
            {
                _Logger.LogWarning("Сотрудник {0} уже существует", Employee);
                return Employee.Id;
            }

            Employee.Id = _Employees.Count == 0 ? 1 : _Employees.Max(e => e.Id) + 1;
            _Employees.Add(Employee);
            _Logger.LogInformation("Сотрудник {0} добавлен", Employee);

            return Employee.Id;
        }

        public Task<int> AddAsync(Employee Employee, CancellationToken Cancel = default)
        {
            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<int>(Cancel);

            if (_Employees.Contains(Employee))
            {
                _Logger.LogWarning("Сотрудник {0} уже существует", Employee);
                return Task.FromResult(Employee.Id);
            }

            Employee.Id = _Employees.Count == 0 ? 1 : _Employees.Max(e => e.Id) + 1;
            _Employees.Add(Employee);
            _Logger.LogInformation("Сотрудник {0} добавлен", Employee);

            return Task.FromResult(Employee.Id);
        }

        public void Edit(Employee Employee)
        {
            _Logger.LogInformation("Попытка редактирования сотрудника id:{0} = {1}", Employee.Id, Employee);
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            if (_Employees.Contains(Employee))
            {
                _Logger.LogWarning("Сотрудник {0} уже существует в памяти списка", Employee);
                return;
            }

            var db_employee = GetById(Employee.Id);
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

        public Task EditAsync(Employee Employee, CancellationToken Cancel = default)
        {
            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled(Cancel);

            _Logger.LogInformation("Попытка редактирования сотрудника id:{0} = {1}", Employee.Id, Employee);
            if (_Employees.Contains(Employee))
            {
                _Logger.LogWarning("Сотрудник {0} уже существует в памяти списка", Employee);
                return Task.CompletedTask;
            }

            var db_employee = GetById(Employee.Id);
            if (db_employee is null)
            {
                _Logger.LogWarning("Сотрудник {0} не найден", Employee);
                return Task.CompletedTask;
            }

            db_employee.SurName = Employee.SurName;
            db_employee.FirstName = Employee.FirstName;
            db_employee.Patronymic = Employee.Patronymic;
            db_employee.Age = Employee.Age;

            return Task.CompletedTask;
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
            if (remove_result)
                _Logger.LogInformation("Сотрудник {0} удалён", db_employee);
            else
                _Logger.LogWarning("Сотрудник {0} отсутствовал в списке!", db_employee);

            return remove_result;
        }

        public Task<bool> DeleteAsync(int id, CancellationToken Cancel = default)
        {
            if (Cancel.IsCancellationRequested)
                return Task.FromCanceled<bool>(Cancel);

            var db_employee = GetById(id);
            if (db_employee is null)
            {
                _Logger.LogWarning("Сотрудник id:{0} не найден", id);
                return Task.FromResult(false);
            }

            var remove_result = _Employees.Remove(db_employee);
            if (remove_result)
                _Logger.LogInformation("Сотрудник {0} удалён", db_employee);
            else
                _Logger.LogWarning("Сотрудник {0} отсутствовал в списке!", db_employee);

            return Task.FromResult(remove_result);
        }

        public void SaveChanges() { }

        public Task SaveChangesAsync(CancellationToken Cancel = default) => Cancel.IsCancellationRequested ? Task.FromCanceled(Cancel) : Task.CompletedTask;
    }
}
