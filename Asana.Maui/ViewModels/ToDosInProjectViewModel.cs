using Asana.Library.Model;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class ToDosInProjectViewModel : INotifyPropertyChanged
    {
        private ToDoServiceProxy _toDoSvc;
        public ObservableCollection<ToDoViewModel> ToDos { get; private set; } = new();

        //Private backing field 
        private ToDoViewModel? _selectedToDo;

        //Public property to get/set the selected ToDo
        public ToDoViewModel? SelectedToDo 
        {
            get
            {
                return _selectedToDo;
            }
            set
            {
                if(SelectedToDo != value)
                {
                    _selectedToDo = value;
                    NotifyPropertyChanged(nameof(SelectedToDo));
                    NotifyPropertyChanged(nameof(IsToDoSelected));
                    NotifyPropertyChanged(nameof(SelectedToDoId));
                }
            }
        }
        public int? SelectedToDoId  
        {
            get
            {
                return SelectedToDo?.Model?.Id ?? 0;
            }

        }

        //Checks to see if ToDo is selected for button enabling
        public bool IsToDoSelected
        {
            get
            {
                return SelectedToDo != null;
            }
        }

        private bool _isShowCompleted;
        public bool IsShowCompleted
        {
            get
            {
                return _isShowCompleted;
            }
            set
            {
                if (_isShowCompleted != value)
                {
                    _isShowCompleted = value;
                    _ = LoadToDosAsync(); 
                }
            }
        }

        public int ProjectId { get; set; }

        //Constructor that sets up the service. Used when navigating to page (default constructor is not used)
        public ToDosInProjectViewModel(int projectId)
        {
            _toDoSvc = ToDoServiceProxy.Current;
            ProjectId = projectId;
            ToDos = new ObservableCollection<ToDoViewModel>();
        }


        //Loads ToDos based on the ProjectId and filters them based on IsShowCompleted
        public async Task LoadToDosAsync()
        {
            var toDos = await _toDoSvc.GetByProjectId(ProjectId);

            //Uses ternary operator, evaluates if IsShowCompleted is true or false then doing the appropriate filtering
            var viewModels = IsShowCompleted ? toDos.Select(t => new ToDoViewModel(t)) : toDos.Where(t => !(t.IsCompleted ?? false)).Select(t => new ToDoViewModel(t));

            ToDos.Clear();
            foreach (var vm in viewModels)
            {
                ToDos.Add(vm);
            }
            NotifyPropertyChanged(nameof(ToDos));
        }

        //Deletes the ToDo by calling the service proxy which calls to API
        public async Task DeleteToDoAsync(ToDoViewModel toDo)
        {
            await _toDoSvc.DeleteToDo(toDo.Model.Id);
            ToDos.Remove(toDo);
            SelectedToDo = null;
            NotifyPropertyChanged(nameof(SelectedToDo));
            NotifyPropertyChanged(nameof(ToDos));
        }

        //Adds or updates the ToDo by calling the service proxy which calls to API
        public async Task AddOrUpdateToDoAsync(ToDoViewModel toDo)
        {
            await toDo.AddOrUpdateAsync();
            await LoadToDosAsync();
        }

        //Refreshes the page by notifying the ToDos property, which will trigger the UI to update
        //Notably only refreshes ToDos collection, since filtering is done in in the load method
        public void RefreshPage()
        {
            NotifyPropertyChanged(nameof(ToDos));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
