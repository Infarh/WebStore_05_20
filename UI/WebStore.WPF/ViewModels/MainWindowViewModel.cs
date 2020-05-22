using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.WPF.Infrastructure.Commands;
using WebStore.WPF.ViewModels.Base;
using WebStore.WPF.Views.Windows;

namespace WebStore.WPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IEmployeesData _EmployeesData;

        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _Title = "Test WebAPI";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion

        #region Employees : ObservableCollection<Employee> - Сотрудники

        /// <summary>Сотрудники</summary>
        private ObservableCollection<Employee> _Employees;

        /// <summary>Сотрудники</summary>
        public ObservableCollection<Employee> Employees
        {
            get => _Employees;
            private set => Set(ref _Employees, value);
        }

        #endregion

        #region SelectedEmployee : Employee - Выбранный сотрудник

        /// <summary>Выбранный сотрудник</summary>
        private Employee _SelectedEmployee;

        /// <summary>Выбранный сотрудник</summary>
        public Employee SelectedEmployee { get => _SelectedEmployee; set => Set(ref _SelectedEmployee, value); }

        #endregion

        #region Команды

        #region Command RefreshEmployeesCommand - Загрузить данные по сотрудникам

        /// <summary>Загрузить данные по сотрудникам</summary>
        public ICommand RefreshEmployeesCommand { get; }

        /// <summary>Проверка возможности выполнения - Загрузить данные по сотрудникам</summary>
        private static bool CanRefreshEmployeesCommandExecute(object p) => true;

        /// <summary>Логика выполнения - Загрузить данные по сотрудникам</summary>
        private async void OnRefreshEmployeesCommandExecuted(object p) =>
            Employees = new ObservableCollection<Employee>(await Task.Run(() => _EmployeesData.GetAll()));

        #endregion

        #region Command CreateEmployeeCommand - Создание нового сотрудника

        /// <summary>Создание нового сотрудника</summary>
        public ICommand CreateEmployeeCommand { get; }

        /// <summary>Проверка возможности выполнения - Создание нового сотрудника</summary>
        private static bool CanCreateEmployeeCommandExecute(object p) => true;

        /// <summary>Логика выполнения - Создание нового сотрудника</summary>
        private async void OnCreateEmployeeCommandExecuted(object p)
        {
            var id = (_Employees?.DefaultIfEmpty().Max(e => e.Id) ?? 0) + 1;

            var employee = new Employee
            {
                SurName = $"Surname {id}",
                FirstName = $"Name {id}",
                Patronymic = $"Patronymic {id}",
                Age = 25
            };

            var editor = new EmployeeEditorWindow { DataContext = employee, Owner = Application.Current.MainWindow };
            if (editor.ShowDialog() != true) return;

            employee.Id = await _EmployeesData.AddAsync(employee);
            Employees.Add(employee);
        }

        #endregion

        #region Command DeleteEmployeeCommand - Удаление сотрудника

        /// <summary>Удаление сотрудника</summary>
        public ICommand DeleteEmployeeCommand { get; }

        /// <summary>Проверка возможности выполнения - Удаление сотрудника</summary>
        private bool CanDeleteEmployeeCommandExecute(object p) => p is Employee employee && (Employees?.Contains(employee) ?? false);

        /// <summary>Логика выполнения - Удаление сотрудника</summary>
        private async void OnDeleteEmployeeCommandExecuted(object p)
        {
            if (!(p is Employee employee)) return;
            await _EmployeesData.DeleteAsync(employee.Id);
            Employees?.Remove(employee);
        }

        #endregion

        #region Command EditEmployeeCommand - Редактирование сотрудника

        /// <summary>Редактирование сотрудника</summary>
        public ICommand EditEmployeeCommand { get; }

        /// <summary>Проверка возможности выполнения - Редактирование сотрудника</summary>
        private static bool CanEditEmployeeCommandExecute(object p) => p is Employee;

        /// <summary>Логика выполнения - Редактирование сотрудника</summary>
        private async void OnEditEmployeeCommandExecuted(object p)
        {
            if (!(p is Employee employee)) return;

            var editor = new EmployeeEditorWindow { DataContext = employee, Owner = Application.Current.MainWindow };
            if (editor.ShowDialog() == true)
                await _EmployeesData.EditAsync(employee);
            else
            {
                var service_employee = await _EmployeesData.GetByIdAsync(employee.Id);
                Employees[Employees.IndexOf(employee)] = service_employee;
            }
        }

        #endregion

        #endregion

        public MainWindowViewModel(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;

            #region Команды

            RefreshEmployeesCommand = new LambdaCommand(OnRefreshEmployeesCommandExecuted, CanRefreshEmployeesCommandExecute);

            CreateEmployeeCommand = new LambdaCommand(OnCreateEmployeeCommandExecuted, CanCreateEmployeeCommandExecute);
            EditEmployeeCommand = new LambdaCommand(OnEditEmployeeCommandExecuted, CanEditEmployeeCommandExecute);
            DeleteEmployeeCommand = new LambdaCommand(OnDeleteEmployeeCommandExecuted, CanDeleteEmployeeCommandExecute);

            #endregion
        }
    }
}
