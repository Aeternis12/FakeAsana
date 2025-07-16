using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class ProjectsPageViewModel : INotifyPropertyChanged
    {
        private ProjectServiceProxy _projectSvc;

        public ProjectsPageViewModel()
        {
            _projectSvc = ProjectServiceProxy.Current;
        }

        

        public ProjectViewModel? SelectedProject { get; set;}

        public int? SelectedProjectId => SelectedProject?.Model?.Id ?? 0;


        public ObservableCollection<ProjectViewModel> Projects
        {
            get
            {
                var projects = _projectSvc.Projects.Select(t => new ProjectViewModel(t));
                return new ObservableCollection<ProjectViewModel>(projects);
            }
        }

        public void DeleteProject()
        {
            if (SelectedProject == null)
            {
                return;
            }

            ProjectServiceProxy.Current.DeleteProject(SelectedProject.Model);
            NotifyPropertyChanged(nameof(Projects));
        }

        public void RefreshPage()
        {
            NotifyPropertyChanged(nameof(Projects));
        }





        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
