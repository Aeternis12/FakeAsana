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
    private  void OkClicked(object sender, EventArgs e)
    {
        //If we are adding a new projects, contexty will have a project with Id 0
        (BindingContext as ProjectViewModel)?.AddOrUpdateProject();
        Shell.Current.GoToAsync("//MainPage");
    }
    private async void CancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //If we are adding a new project, ProjectId will be 0 since viewModel has a project with Id 0
        //Otherwise, load the project with the given ProjectId
        var viewModel = new ProjectViewModel();
        await viewModel.LoadProjectAsync(ProjectId);
        BindingContext = viewModel;
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

}