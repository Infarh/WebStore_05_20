using WebStore.WPF.ViewModels.Base;

namespace WebStore.WPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _Title = "Test WebAPI";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion
    }
}
