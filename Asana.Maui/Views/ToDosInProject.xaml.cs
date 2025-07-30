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

    private async void CancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
    private async void AddNewClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ToDoDetails");
    }
    private async void EditClicked(object sender, EventArgs e)
    {
        //Gets the ID of the selected ToDo from viewModel so we can get the details of it
        var selectedId = (BindingContext as ToDosInProjectViewModel)?.SelectedToDoId ?? 0;
        await Shell.Current.GoToAsync($"//ToDoDetails?toDoId={selectedId}");
    }
    private async void DeleteClicked(object sender, EventArgs e)
    {
        //Gets the selected ToDo from viewModel so we can delete it
        var selectedToDo = (BindingContext as ToDosInProjectViewModel)?.SelectedToDo;
        await (BindingContext as ToDosInProjectViewModel)?.DeleteToDoAsync(selectedToDo);
    }

    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //Gets the Project ID from the query property so we can load the ToDos in that project
        BindingContext = new ToDosInProjectViewModel(ProjectId);
        await (BindingContext as ToDosInProjectViewModel)?.LoadToDosAsync();
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void InLineDeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as ToDosInProjectViewModel)?.RefreshPage();
    }
}
