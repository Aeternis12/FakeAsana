using Asana.Library.Model;
using Asana.Library.Util;
using Newtonsoft.Json;
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
        private readonly WebRequestHandler _webRequestHandler = new WebRequestHandler();


        //Singleton instance for ProjectServiceProxy
        private static object _lock = new object();
        private static ProjectServiceProxy? instance;
        public static ProjectServiceProxy Current
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new ProjectServiceProxy();
                    }
                }

                return instance;
            }
        }

        //Public property to access the list of projects
        public List<Project> Projects
        {
            get
            {
                return _projects;
            }
        }

        //Private backing field for the list of projects
        private List<Project> _projects;

        public ProjectServiceProxy()
        {
            _projects = new List<Project>();
        }

        //Method to retrieve all projects from the API
        public async Task<List<Project>> GetProjects()
        {
            var projectData = await _webRequestHandler.Get("/Project/Expand");
            _projects = JsonConvert.DeserializeObject<List<Project>>(projectData) ?? new List<Project>();
            return _projects;
        }

        //Method to retrieve a project by its ID so we can view or edit it in the MainPage seleciton
        public async Task<Project?> GetById(int id)
        {
            var data = await _webRequestHandler.Get($"/Project/{id}");
            return JsonConvert.DeserializeObject<Project>(data);
        }

        //Method that calls to API, adding in a new project or updating an existing one from the parameter
        public async Task<Project?> AddOrUpdate(Project? project)
        {
            var data = await _webRequestHandler.Post("/Project", project);
            return JsonConvert.DeserializeObject<Project>(data);
        }

        //Calls to API to delete a project by its ID
        public async Task DeleteProject(int id)
        {
            await _webRequestHandler.Delete($"/Project/{id}");
        }
    }
}

