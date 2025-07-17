using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class ToDosInProjectViewModel : INotifyPropertyChanged
    {
        public Project? SelectedProject { get; set; }
        public ObservableCollection<ToDoDetailViewModel> ToDos
        {
            get
            {
                var allToDos = (SelectedProject?.ToDos ?? new List<ToDo>()).Select(t => new ToDoDetailViewModel(t));
                if (!IsShowCompleted)
                {
                    allToDos = allToDos.Where(t => !(t.Model?.IsCompleted ?? false));
                }
                return new ObservableCollection<ToDoDetailViewModel>(allToDos);
            }
        }

        public ToDosInProjectViewModel(int projectId)
        {
            SelectedProject = ProjectServiceProxy.Current.GetById(projectId);
        }

        public ToDoDetailViewModel? SelectedToDo { get; set; }

        public int? SelectedToDoId => SelectedToDo?.Model?.Id ?? 0;

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
                    NotifyPropertyChanged(nameof(ToDos));
                }
            }
        }

        public void DeleteToDo()
        {
            if (SelectedToDo == null)
            {
                return;
            }

            ToDoServiceProxy.Current.DeleteToDo(SelectedToDo.Model);
            NotifyPropertyChanged(nameof(ToDos));
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
