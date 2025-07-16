using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectViewModel
    {
        public Project? Model { get; set; }
        
        
        public ProjectViewModel()
        {
            Model = new Project();
        }

        public ProjectViewModel(int id)
        {
            Model = ProjectServiceProxy.Current.GetById(id) ?? new Project();
            DeleteCommand = new Command(DoDelete);
        }

        public ProjectViewModel(Project? model)
        {
            Model = model ?? new Project();
            DeleteCommand = new Command(DoDelete);
        }

        public void DoDelete()
        {
            ProjectServiceProxy.Current.DeleteProject(Model);
        }



        public ICommand? DeleteCommand { get; set; }

        //public List<int> Priorities
        //{
        //    get
        //    {
        //        return new List<int> { 1, 2, 3, 4 };
        //    }
        //}

        //public int SelectedPriority
        //{
        //    get
        //    {
        //        return Model?.Priority ?? 4;
        //    }
        //    set
        //    {
        //        if (Model != null && Model.Priority != value)
        //        {
        //            Model.Priority = value;
        //        }
        //    }
        //}

        public void AddOrUpdateProject()
        {
            ProjectServiceProxy.Current.AddOrUpdate(Model);
        }


    }


    
}
