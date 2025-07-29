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
        public ICommand? DeleteCommand { get; set; }

        
        public ProjectViewModel()
        {
            Model = new Project();
        }

        public ProjectViewModel(Project? model)
        {
            Model = model ?? new Project();
            DeleteCommand = new Command(async () => await DeleteProjectAsync());
        }

        public async Task LoadProjectsAsync(int id)
        {
            Model = await ProjectServiceProxy.Current.GetById(id) ?? new Project();
            DeleteCommand = new Command(async () => await DeleteProjectAsync());
        }
 
       

        public async Task DeleteProjectAsync()
        {
            if (Model == null) return;
            await ProjectServiceProxy.Current.DeleteProject(Model.Id);
        }

        public async Task AddOrUpdateProject()
        {
            await ProjectServiceProxy.Current.AddOrUpdate(Model);
        }


    }


    
}
