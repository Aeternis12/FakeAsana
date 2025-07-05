using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;


[QueryProperty(nameof(ProjectId), "projectId")]
public partial class ToDosInProject : ContentPage
{
	public int ProjectId { get; set; }
    public ToDosInProject()
	{
		InitializeComponent();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//ProjectsPage");
    }
    private void AddNewClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ToDoDetails");
    }
    private void EditClicked(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as MainPageViewModel)?.SelectedToDoId ?? 0;
        Shell.Current.GoToAsync($"//ToDoDetails?toDoId={selectedId}");
    }
    private void DeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as MainPageViewModel)?.DeleteToDo();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ToDosInProjectViewModel(ProjectId);
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

}