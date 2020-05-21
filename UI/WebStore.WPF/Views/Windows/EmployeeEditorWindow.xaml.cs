using System.Windows;
using System.Windows.Controls;

namespace WebStore.WPF.Views.Windows
{
    public partial class EmployeeEditorWindow
    {
        public EmployeeEditorWindow() => InitializeComponent();

        private void OnDialogButtonClick(object Sender, RoutedEventArgs E)
        {
            if(!(Sender is Button button)) return;

            DialogResult = !button.IsCancel;
            Close();
        }
    }
}
