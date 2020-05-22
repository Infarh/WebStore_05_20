using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration Configuration) : base(Configuration, WebAPI.Employees) { }

        public IEnumerable<Employee> GetAll() => Get<List<Employee>>(_ServiceAddress);

        public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken Cancel = default) =>
            await GetAsync<IEnumerable<Employee>>(_ServiceAddress, Cancel)
               .ConfigureAwait(false);

        public Employee GetById(int id) => Get<Employee>($"{_ServiceAddress}/{id}");

        public async Task<Employee> GetByIdAsync(int id, CancellationToken Cancel = default) =>
            await GetAsync<Employee>($"{_ServiceAddress}/{id}", Cancel)
               .ConfigureAwait(false);

        public int Add(Employee Employee) => Post(_ServiceAddress, Employee).Content.ReadAsAsync<int>().Result;

        public async Task<int> AddAsync(Employee Employee, CancellationToken Cancel = default) =>
            await (await PostAsync(_ServiceAddress, Employee, Cancel).ConfigureAwait(false))
               .EnsureSuccessStatusCode()
               .Content.ReadAsAsync<int>(Cancel)
               .ConfigureAwait(false);

        public void Edit(Employee Employee) => Put($"{_ServiceAddress}", Employee);

        public async Task EditAsync(Employee Employee, CancellationToken Cancel = default) =>
            await PutAsync($"{_ServiceAddress}", Employee, Cancel)
               .ConfigureAwait(false);

        public bool Delete(int id) => Delete($"{_ServiceAddress}/{id}").IsSuccessStatusCode;

        public async Task<bool> DeleteAsync(int id, CancellationToken Cancel = default) =>
            (await DeleteAsync($"{_ServiceAddress}/{id}", Cancel).ConfigureAwait(false)).IsSuccessStatusCode;

        public void SaveChanges() { }

        public Task SaveChangesAsync(CancellationToken Cancel = default) => Cancel.IsCancellationRequested ? Task.FromCanceled(Cancel) : Task.CompletedTask;
    }
}
