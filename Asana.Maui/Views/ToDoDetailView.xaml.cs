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
        (BindingContext as ToDoViewModel)?.AddOrUpdateAsync();
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var viewModel = new ToDoViewModel();
        await viewModel.LoadAsync(ToDoId);
        BindingContext = viewModel;
    }
}