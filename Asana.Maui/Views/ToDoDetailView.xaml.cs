using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(ToDoId), "toDoId")]
public partial class ToDoDetailView : ContentPage
{
	public ToDoDetailView()
	{
		InitializeComponent();
    }

    public int ToDoId { get; set; }

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        //If we are adding a new ToDo, context will have a ToDo with Id 0
        (BindingContext as ToDoViewModel)?.AddOrUpdateAsync();
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //If we are adding a new ToDo, ToDoId will be 0 since viewModel has a ToDo with Id 0
        //Otherwise, load the ToDo with the given ToDoId
        var viewModel = new ToDoViewModel();
        await viewModel.LoadAsync(ToDoId);
        BindingContext = viewModel;
    }
}