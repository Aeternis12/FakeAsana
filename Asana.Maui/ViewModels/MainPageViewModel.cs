using Asana.Library.Model;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        //Private backing field for the project service, called to in the constructor
        private ProjectServiceProxy _projectSvc;

        //ObservableCollection which notifies the user interface of changes
        public ObservableCollection<ProjectViewModel> Projects { get; private set; } = new();

        //Private backing field for the selected project
        private ProjectViewModel? _selectedProject;

        //Public property to get/set the selected project
        public ProjectViewModel? SelectedProject 
        {
            get
            {
                return _selectedProject;
            } 
            set
            {
                if (_selectedProject != value)
                {
                    //Updates UI of selected project
                    _selectedProject = value;
                    NotifyPropertyChanged(nameof(SelectedProject));
                    NotifyPropertyChanged(nameof(SelectedProjectId));
                    NotifyPropertyChanged(nameof(IsProjectSelected));
                }
            }
        }

        public int? SelectedProjectId
        {
            get
            {
                return SelectedProject?.Model?.Id ?? 0;
            }

        }

        //Checks to see if project is selected for button enabling
        public bool IsProjectSelected 
        {
            get
            {
                return SelectedProject != null;
            } 
        }

        //Constructor initializes the project service and loads the projects into the ObservableCollection
        public MainPageViewModel()
        {
            _projectSvc = ProjectServiceProxy.Current;
            Projects = new ObservableCollection<ProjectViewModel>(_projectSvc.Projects.Select(t => new ProjectViewModel(t)));
        }

        //Loads up the projects from the project service (which calls the API)
        public async Task LoadProjectsAsync()
        {
            var projects = await _projectSvc.GetProjects();
            Projects.Clear();
            foreach (var project in projects)
            {
                Projects.Add(new ProjectViewModel(project));
            }
            SelectedProject = null; //Clear the selected project after loading
            NotifyPropertyChanged(nameof(SelectedProject)); 
            //Updates observable colletion
            NotifyPropertyChanged(nameof(Projects));
        }

        //Calls to projectviewmodel which calls to service which calls to API to add or update the project
        //Also reloads projects to reflect any changes made
        public async Task AddOrUpdateProjectAsync(ProjectViewModel project)
        {
            await project.AddOrUpdateProject();
            await LoadProjectsAsync();
        }

        //Calls to projectviewmodel which calls to service which calls to API to delete the project
        public async Task DeleteProjectAsync(ProjectViewModel project)
        {
            if (project == null) return;
            await _projectSvc.DeleteProject(project.Model.Id);
            Projects.Remove(project);
            SelectedProject = null; //Clear the selected project after deletion
            //Updates observable collection
            NotifyPropertyChanged(nameof(SelectedProject));
            NotifyPropertyChanged(nameof(Projects));
        }

        //Refrshes page by clearing observable collection and reloading projects
        public void RefreshPage()
        {
            //Can add this to refresh because the projects are not filtered (compared to ToDosInProjectViewModel)
            Projects.Clear();
            foreach (var project in _projectSvc.Projects)
            {
                Projects.Add(new ProjectViewModel(project));
            }
            NotifyPropertyChanged(nameof(Projects));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
