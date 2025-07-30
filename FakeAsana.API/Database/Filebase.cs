using Asana.API.Enterprise;
using Asana.Library.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ToDoApplication.Persistence
{
    public class Filebase
    {
        private string _root;
        private string _projectRoot;
        private static Filebase _instance;


        public static Filebase Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = @"C:\temp";
            _projectRoot = $"{_root}\\Projects";
            Directory.CreateDirectory(_projectRoot);
        }

        public int LastToDoKey(int projectId = 0)
        {
            var projectToDos = ToDos.Where(t => t.ProjectId == projectId);
            if (projectToDos.Any())
            {
                return projectToDos.Select(x => x.Id).Max();
            }
            return 0;
        }

        public int LastProjectKey
        {
            get
            {
                if (Projects.Any())
                {
                    return Projects.Select(x => x.Id).Max();
                }
                return 0;
            }
        }
        public List<ToDo> ToDos
        {
            get
            {
                var root = new DirectoryInfo(_projectRoot);
                var _toDos = new List<ToDo>();
                foreach (var projectDirectory in root.GetDirectories())
                {
                    var toDoDirectory = new DirectoryInfo($"{projectDirectory.FullName}\\ToDos");
                    if (toDoDirectory.Exists)
                    {
                        foreach (var toDoFile in toDoDirectory.GetFiles("*.json"))
                        {
                            var toDo = JsonConvert.DeserializeObject<ToDo>(File.ReadAllText(toDoFile.FullName));
                            if (toDo != null)
                            {
                                _toDos.Add(toDo);
                            }
                        }
                    }

                }
                return _toDos;
            }
        }

        public List<Project> Projects
        {
            get
            {
                var root = new DirectoryInfo(_projectRoot);
                var _projects = new List<Project>();
                foreach (var projectDirectory in root.GetDirectories())
                {
                    var projectFile = new FileInfo($"{projectDirectory.FullName}\\project.json");
                    if (projectFile.Exists)
                    {
                        var project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(projectFile.FullName));
                        if (project != null)
                        {
                            _projects.Add(project);
                        }

                    }
                }
                return _projects;
            }
        }

        public ToDo AddOrUpdateToDo(ToDo toDo)
        {
            //set up a new Id if one doesn't already exist
            if (toDo.Id <= 0)
            {
                toDo.Id = LastToDoKey(toDo.ProjectId) + 1;
            }

            //find the project that the ToDo will belong to
            var project = Projects.FirstOrDefault(p => p.Id == toDo.ProjectId);

            string toDoName = toDo.Name ?? $"ToDo {toDo.Id}";
            string toDoPath = $"{_projectRoot}\\Project_{project.Id}\\ToDos";

            //go to the right place
            string path = $"{toDoPath}\\{toDo.Id}.json";

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(toDo));
            UpdateProjectCompletePercentage(project);


            //return the item, which now has an id
            return toDo;
        }

        public Project AddOrUpdateProject(Project project)
        {
            //set up a new Id if one doesn't already exist
            if (project.Id <= 0)
            {
                project.Id = LastProjectKey + 1;
            }
            //get the project path
            string projectPath = $"{_projectRoot}\\Project_{project.Id}";

            //check if the project directory exists, if not create it
            Directory.CreateDirectory(projectPath);
            Directory.CreateDirectory($"{projectPath}\\ToDos");

            //go to the right place
            string path = $"{projectPath}\\project.json";

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(project));

            //return the item, which now has an id
            return project;
        }


        public List<Project>? GetProjects(bool Expand = false)
        {
            if (Expand)
            {
                var projectList = new List<Project>();
                foreach (var project in Projects)
                {
                    var proj = project;
                    proj.ToDos = ToDos.Where(t => t.ProjectId == proj.Id).ToList();
                    projectList.Add(proj);
                }
                return projectList;
            }
            return Projects;
        }


        public void DeleteProject(Project? project)
        {
            if (project == null)
            {
                return;
            }
            string projectPath = $"{_projectRoot}\\Project_{project.Id}";

            if (Directory.Exists(projectPath))
            {
                Directory.Delete(projectPath, true);
            }
        }

        public void DeleteToDo(ToDo? toDo)
        {
            if (toDo == null)
            {
                return;
            }
            var project = Projects.FirstOrDefault(p => p.Id == toDo.ProjectId);

            string toDoName = toDo.Name ?? $"ToDo {toDo.Id}";
            string toDoPath = $"{_projectRoot}\\{project.Name}\\ToDos";

            string path = $"{toDoPath}\\{toDo.Id}.json";

            File.Delete(path);

                        UpdateProjectCompletePercentage(project);


        }


        public void UpdateProjectCompletePercentage(Project project)
        {
            if (project == null) return;

            var projectToDos = ToDos.Where(t => t.ProjectId == project.Id).ToList();
            var completedToDos = projectToDos.Count(t => t.IsCompleted == true);

            if (projectToDos.Count == 0)
            {
                project.CompletePercent = 0;
            }
            else
            {
                //round the percent to two decimal places
                project.CompletePercent = Math.Round(completedToDos / (double)projectToDos.Count * 100, 2);
            }

            //makes sure that the files are updated
            string projectPath = $"{_projectRoot}\\Project_{project.Id}\\project.json";
            File.WriteAllText(projectPath, JsonConvert.SerializeObject(project));
        }
    }

   
}