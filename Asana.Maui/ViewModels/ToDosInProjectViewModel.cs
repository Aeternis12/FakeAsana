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

        public ToDoViewModel? SelectedToDo { get; set; }
        public int? SelectedToDoId  
        {
            get
            {
                return SelectedToDo?.Model?.Id ?? 0;
            }

        }

        private bool isShowCompleted;
        public bool IsShowCompleted
        {
            get
            {
                return isShowCompleted;
            }
                set
            {
                if (isShowCompleted != value)
                {
                    isShowCompleted = value;
                    _ = LoadToDosAsync();
                }
            }
        }

        public int ProjectId { get; set; }

        public ToDosInProjectViewModel(int projectId)
        {
            _toDoSvc = ToDoServiceProxy.Current;
            ProjectId = projectId;
            ToDos = new ObservableCollection<ToDoViewModel>();
        }

        public async Task LoadToDosAsync()
        {
            var toDos = await _toDoSvc.GetByProjectId(ProjectId);
            var viewModels = toDos.Where(t => IsShowCompleted || !(t.IsCompleted ?? false)).Select(t => new ToDoViewModel(t));

            //Do this because todos are filtered vs the projects async load which is not filterd
            ToDos.Clear();
            foreach (var vm in viewModels)
            {
                ToDos.Add(vm);
            }
            NotifyPropertyChanged(nameof(ToDos));
        }

        public async Task DeleteToDoAsync(ToDoViewModel toDo)
        {
            await _toDoSvc.DeleteToDo(toDo.Model.Id);
            ToDos.Remove(toDo);
            NotifyPropertyChanged(nameof(ToDos));
        }

        public async Task AddOrUpdateToDoAsync(ToDoViewModel toDo)
        {
            await toDo.AddOrUpdateAsync();
            await LoadToDosAsync();
        }

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
