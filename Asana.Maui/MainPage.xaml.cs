using Asana.Maui.ViewModels;

namespace Asana.Maui;
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ProjectDetails");
    }

    private async void EditClicked(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as MainPageViewModel)?.SelectedProjectId ?? 0;
        await Shell.Current.GoToAsync($"//ProjectDetails?projectId={selectedId}");
    }

    private async void DeleteClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as MainPageViewModel;
        if (viewModel?.SelectedProject != null)
        {
            await viewModel.DeleteProjectAsync(viewModel.SelectedProject);
        }
    }

    private void ToDosInProjectClicked(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as MainPageViewModel)?.SelectedProjectId ?? 0;
        Shell.Current.GoToAsync($"//ToDosInProject?projectId={selectedId}");
    }
    private void InLineDeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as MainPageViewModel)?.RefreshPage();
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        await (BindingContext as MainPageViewModel)?.LoadProjectsAsync();
    }

}