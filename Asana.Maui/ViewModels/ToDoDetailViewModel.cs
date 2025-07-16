using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ToDoDetailViewModel : INotifyPropertyChanged
    {
        public ToDoDetailViewModel()
        {
            Model = new ToDo();
            Projects = ProjectServiceProxy.Current.Projects.ToList();
        }

        public ToDoDetailViewModel(int id)
        {
            Model = ToDoServiceProxy.Current.GetById(id) ?? new ToDo();
            Projects = ProjectServiceProxy.Current.Projects.ToList();
            DeleteCommand = new Command(DoDelete);
        }

        public ToDoDetailViewModel(ToDo? model)
        {
            Model = model ?? new ToDo();
            Projects = ProjectServiceProxy.Current.Projects.ToList();
            DeleteCommand = new Command(DoDelete);
        }

        public List<Project> Projects { get; } = new List<Project>();

        public Project? SelectedProject
        {
            get => Projects.FirstOrDefault(p => p.Id == Model?.ProjectId);

            set
            {
                if (Model != null && value != null)
                {
                    Model.ProjectId = value.Id;
                }
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        public void DoDelete()
        {
            ToDoServiceProxy.Current.DeleteToDo(Model);
        }



        public ToDo? Model { get; set; }

        public ICommand? DeleteCommand { get; set; }

        public List<int> Priorities
        {
            get
            {
                return new List<int> { 1, 2, 3, 4 };
            }
        }

        public int SelectedPriority
        {
            get
            {
                return Model?.Priority ?? 4;
            }
            set
            {
                if (Model != null && Model.Priority != value)
                {
                    Model.Priority = value;
                }
            }
        }

        public void AddOrUpdateToDo()
        {
            ToDoServiceProxy.Current.AddOrUpdate(Model);
        }

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
