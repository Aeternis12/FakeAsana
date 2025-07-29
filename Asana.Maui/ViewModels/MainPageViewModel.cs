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
        private ProjectServiceProxy _projectSvc;
        public ObservableCollection<ProjectViewModel> Projects { get; private set; } = new();

        public ProjectViewModel? SelectedProject { get; set; }
        public int? SelectedProjectId
        {
            get
            {
                return SelectedProject?.Model?.Id ?? 0;
            }

        }

        public MainPageViewModel()
        {
            _projectSvc = ProjectServiceProxy.Current;
            Projects = new ObservableCollection<ProjectViewModel>(_projectSvc.Projects.Select(t => new ProjectViewModel(t)));
        }

        public async Task LoadProjectsAsync()
        {
            var projects = await _projectSvc.GetProjects();
            Projects.Clear();
            foreach (var project in projects)
                Projects.Add(new ProjectViewModel(project));
            NotifyPropertyChanged(nameof(Projects));
        }

        public async Task AddOrUpdateProjectAsync(ProjectViewModel project)
        {
            await project.AddOrUpdateProject();
            await LoadProjectsAsync();
        }

        public async Task DeleteProjectAsync(ProjectViewModel project)
        {
            if (project == null) return;
            await ProjectServiceProxy.Current.DeleteProject(project.Model.Id);
            Projects.Remove(project);
            NotifyPropertyChanged(nameof(Projects));
        }

        public void RefreshPage()
        {
            //Can add this to refresh because the projects are not filtered
            Projects.Clear();
            foreach (var project in _projectSvc.Projects)
                Projects.Add(new ProjectViewModel(project));
            NotifyPropertyChanged(nameof(Projects));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
