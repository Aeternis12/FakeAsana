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
        var selectedId = (BindingContext as ToDosInProjectViewModel)?.SelectedToDoId ?? 0;
        await Shell.Current.GoToAsync($"//ToDoDetails?toDoId={selectedId}");
    }
    private async void DeleteClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as ToDosInProjectViewModel;
        if (viewModel?.SelectedToDo != null)
        {
            await viewModel.DeleteToDoAsync(viewModel.SelectedToDo);
        }
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ToDosInProjectViewModel(ProjectId);
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void InLineDeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as ToDosInProjectViewModel)?.RefreshPage();
    }
}
