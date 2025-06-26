using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class MainPageViewModel
    {
        private ToDoServiceProxy _toDoSvc;

        public MainPageViewModel()
        {
            _toDoSvc = ToDoServiceProxy.Current;
        }

        public ObservableCollection<ToDo> ToDos
        {
            get
            {
                return new ObservableCollection<ToDo>(_toDoSvc.ToDos);
            }
        } 


        public bool IsShowCompleted { get; set; } 
    }
}
