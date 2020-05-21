using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.WPF.Infrastructure.Commands;
using WebStore.WPF.ViewModels.Base;

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

        #region Employees : IEnumerable<Employee> - Сотрудники

        /// <summary>Сотрудники</summary>
        private IEnumerable<Employee> _Employees;

        /// <summary>Сотрудники</summary>
        public IEnumerable<Employee> Employees
        {
            get => _Employees;
            private set => Set(ref _Employees, value);
        }

        #endregion

        #region Команды

        #region Command RefreshEmployeesCommand - Загрузить данные по сотрудникам

        /// <summary>Загрузить данные по сотрудникам</summary>
        public ICommand RefreshEmployeesCommand { get; }

        /// <summary>Проверка возможности выполнения - Загрузить данные по сотрудникам</summary>
        private static bool CanRefreshEmployeesCommandExecute(object p) => true;

        /// <summary>Логика выполнения - Загрузить данные по сотрудникам</summary>
        private async void OnRefreshEmployeesCommandExecuted(object p)
        {
            Employees = await Task.Run(() => _EmployeesData.GetAll());
        }

        #endregion

        #endregion

        public MainWindowViewModel(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;

            #region Команды

            RefreshEmployeesCommand = new LambdaCommand(OnRefreshEmployeesCommandExecuted, CanRefreshEmployeesCommandExecute);

            #endregion
        }
    }
}
