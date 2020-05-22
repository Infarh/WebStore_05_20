using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();

        Task<IEnumerable<Employee>> GetAllAsync(CancellationToken Cancel = default);

        Employee GetById(int id);

        Task<Employee> GetByIdAsync(int id, CancellationToken Cancel = default);

        int Add(Employee Employee);

        Task<int> AddAsync(Employee Employee, CancellationToken Cancel = default);

        void Edit(Employee Employee);

        Task EditAsync(Employee Employee, CancellationToken Cancel = default);

        bool Delete(int id);

        Task<bool> DeleteAsync(int id, CancellationToken Cancel = default);

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken Cancel = default);
    }
}
