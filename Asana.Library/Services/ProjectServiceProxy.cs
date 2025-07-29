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

        public List<Project> Projects
        {
            get
            {
                return _projects;
            }
        }

        private List<Project> _projects;

        public ProjectServiceProxy()
        {
            _projects = new List<Project>();
        }

        public async Task<List<Project>> GetProjects()
        {
            var projectData = await _webRequestHandler.Get("/Project/Expand");
            if (string.IsNullOrEmpty(projectData))
            {
                Console.WriteLine("No project data received.");
                return new List<Project>();
            }
            _projects = JsonConvert.DeserializeObject<List<Project>>(projectData) ?? new List<Project>();
            return _projects;
        }

        public async Task<Project?> GetById(int id)
        {
            var data = await _webRequestHandler.Get($"/Project/{id}");
            return JsonConvert.DeserializeObject<Project>(data);
        }

        public async Task<Project?> AddOrUpdate(Project? project)
        {
            var data = await _webRequestHandler.Post("/Project", project);
            return JsonConvert.DeserializeObject<Project>(data);
        }

        public async Task DeleteProject(int id)
        {
            await _webRequestHandler.Delete($"/Project/{id}");
        }
    }
}

