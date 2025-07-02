﻿using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ToDoDetailViewModel
    {
        public ToDoDetailViewModel() {
            Model = new ToDo(); 
        }

        public ToDoDetailViewModel(int id)
        {
            Model = ToDoServiceProxy.Current.GetById(id) ?? new ToDo();
            DeleteCommand = new Command(DoDelete);
        }

        public ToDoDetailViewModel(ToDo? model)
        {
            Model = model ?? new ToDo();
            DeleteCommand = new Command(DoDelete);
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
