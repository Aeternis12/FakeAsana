using Asana.Maui.ViewModels;

namespace Asana.Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        private void AddNewClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ToDoDetails");
        }

    }

}
