using Asana.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Asana.Library.Services
{
    public class ProjectServiceProxy
    {
        private List<Project> _ProjectList;
        public List<Project> Projects
        {
            get
            {
                return _ProjectList.Take(100).ToList();
            }
            private set
            {
                if (value != _ProjectList)
                {
                    _ProjectList = value;
                }
            }
        }

        private ProjectServiceProxy()
        {
            Projects = new List<Project>();
        }

        private static ProjectServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (Projects.Any())
                {
                    return Projects.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static ProjectServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectServiceProxy();
                }

                return instance;
            }
        }

        public void UpdateCompletePercent(Project project)
        {
            if(project == null || project.ToDos == null || !project.ToDos.Any())
            {
                project.CompletePercent = 0;
                return;
            }
            var completed = project.ToDos.Count(t => t.IsCompleted == true);
            project.CompletePercent = completed / (float)project.ToDos.Count * 100;

        }

        public Project? GetById(int id)
        {
            return Projects.FirstOrDefault(t => t.Id == id);
        }

        public Project AddOrUpdate(Project? project)
        {
            if (project != null && project.Id == 1)
            {
                project.Id = nextKey;
                _ProjectList.Add(project);
            }

            return project;
        }

        public void DeleteProject(Project? project)
        {

            if (project == null)
            {
                return;
            }
            _ProjectList.Remove(project);
        }
    }
}

