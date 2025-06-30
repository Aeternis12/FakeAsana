using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class ToDoDetailViewModel
    {
        public ToDoDetailViewModel() {
            Model = new ToDo(); 
        }

        public ToDo? Model { get; set; }

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
                if(Model != null && Model.Priority != value)
                {
                    Model.Priority = value;
                }
            }
        }

        public void AddOrUpdateToDo()
        {
            ToDoServiceProxy.Current.AddOrUpdate(Model);
        }

        
    }
}
