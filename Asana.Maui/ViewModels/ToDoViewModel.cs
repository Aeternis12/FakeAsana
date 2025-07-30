using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ToDoViewModel : INotifyPropertyChanged
    {
        public ToDo? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }
        public List<int> Priorities
        {
            get
            {
               return new List<int> { 1, 2, 3, 4 };
            }
        }

        //Projects set up so we can assign a ToDo to a Project.
        public List<Project> Projects { get; private set; } = new List<Project>();

        //Simple constructor for a new ToDo when adding a new one
        public ToDoViewModel()
        {
            Model = new ToDo();
            DeleteCommand = new Command(async () => await DeleteAsync());
        }


        //Constructor used when editing or viewing a ToDo
        public ToDoViewModel(ToDo? model)
        {
            Model = model ?? new ToDo();
            Projects = ProjectServiceProxy.Current.Projects.ToList();
            DeleteCommand = new Command(async () => await DeleteAsync());
        }

        //Loads ToDos and Projects based on the ID
        //Notifies the UI of changes to the Model, Projects, and SelectedProject
        public async Task LoadAsync(int id)
        {
            Model = await ToDoServiceProxy.Current.GetById(id) ?? new ToDo();
            Projects = await ProjectServiceProxy.Current.GetProjects();
            OnPropertyChanged(nameof(Model));
            OnPropertyChanged(nameof(Projects));
            OnPropertyChanged(nameof(SelectedProject));
        }

        //Adds or updates the ToDo by calling the service proxy which calls to API
        public async Task AddOrUpdateAsync()
        {
            await ToDoServiceProxy.Current.AddOrUpdate(Model);
        }


        //Deletes the ToDo by calling the service proxy which calls to API
        public async Task DeleteAsync()
        {
            if (Model == null) return;
            await ToDoServiceProxy.Current.DeleteToDo(Model.Id);
        }

        public int SelectedPriority
        {
            get => Model?.Priority ?? 4;
            set
            {
                if (Model != null && Model.Priority != value)
                {
                    Model.Priority = value;
                    OnPropertyChanged(nameof(SelectedPriority));
                }
            }
        }

        public Project? SelectedProject
        {
            get => Projects.FirstOrDefault(p => p.Id == Model?.ProjectId);
            set
            {
                if (Model != null && value != null)
                {
                    Model.ProjectId = value.Id;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        //When due date and time are set, they are combined into a single DateTime property.
        public DateTime DueDate
        {
            get => Model?.DueDate?.Date ?? DateTime.Today;
            set
            {
                if (Model != null)
                {
                    var time = Model.DueDate?.TimeOfDay ?? TimeSpan.Zero;
                    Model.DueDate = value.Date + time;
                    OnPropertyChanged(nameof(DueDate));
                    OnPropertyChanged(nameof(DueTime));
                }
            }
        }

        // When due time is set, it is combined with the existing date to form a DateTime property.
        public TimeSpan DueTime
        {
            get => Model?.DueDate?.TimeOfDay ?? TimeSpan.Zero;
            set
            {
                if (Model != null)
                {
                    var date = Model.DueDate?.Date ?? DateTime.Today;
                    Model.DueDate = date + value;
                    OnPropertyChanged(nameof(DueTime));
                    OnPropertyChanged(nameof(DueDate));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
