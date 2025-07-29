using Asana.Library.Model;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(ProjectId), "projectId")]
public partial class ProjectDetailView : ContentPage
{
	public ProjectDetailView()
	{
		InitializeComponent();
	}

    public int ProjectId { get; set; }
    private async void OkClicked(object sender, EventArgs e)
    {
        if (BindingContext is ProjectViewModel viewModel) 
        await viewModel.AddOrUpdateProject();
        await Shell.Current.GoToAsync("//MainPage");
    }
    private async void CancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var viewModel = new ProjectViewModel();
        await viewModel.LoadProjectsAsync(ProjectId);
        BindingContext = viewModel;
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {
    }

}