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

        //Simple set up of a viewmodel
        public ProjectViewModel()
        {
            Model = new Project();
        }

        //Used when editing or viewing a project
        public ProjectViewModel(Project? model)
        {
            Model = model ?? new Project();
            //Dont think this works
            DeleteCommand = new Command(async () => await DeleteProjectAsync());
        }

        //Loads the project based on the ID and sets up the delete command
        public async Task LoadProjectAsync(int id)
        {
            Model = await ProjectServiceProxy.Current.GetById(id) ?? new Project();
            //Dont think this works
            DeleteCommand = new Command(async () => await DeleteProjectAsync());
        }


        // Deletes the project by calling the service proxy which calls to API
        public async Task DeleteProjectAsync()
        {
            if (Model == null) return;
            await ProjectServiceProxy.Current.DeleteProject(Model.Id);
        }

        //Adds or updates the project by calling the service proxy which calls to API
        public async Task AddOrUpdateProject()
        {
            await ProjectServiceProxy.Current.AddOrUpdate(Model);
        }


    }


    
}
