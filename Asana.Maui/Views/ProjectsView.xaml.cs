using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

public partial class ProjectsView : ContentPage
{
	public ProjectsView()
	{
		InitializeComponent();
        BindingContext = new ProjectsPageViewModel();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ProjectDetails");
    }

    private void EditClicked(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as ProjectsPageViewModel)?.SelectedProjectId ?? 0;
        Shell.Current.GoToAsync($"//ProjectDetails?projectId={selectedId}");
    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as ProjectsPageViewModel)?.DeleteProject();
    }

    private void ToDosInProjectClicked(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as ProjectsPageViewModel)?.SelectedProjectId ?? 0;
        Shell.Current.GoToAsync($"//ToDosInProject?projectId={selectedId}");
    }
    private void InLineDeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as ProjectsPageViewModel)?.RefreshPage();
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ProjectsPageViewModel)?.RefreshPage();
    }

}