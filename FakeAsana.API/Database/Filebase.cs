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

        //Gets the last ToDo Id for a specific project
        public int LastToDoKey(int projectId = 0)
        {
            var projectToDos = ToDos.Where(t => t.ProjectId == projectId);
            if (projectToDos.Any())
            {
                return projectToDos.Select(x => x.Id).Max();
            }
            return 0;
        }

        //Gets the last Project Id from the file system
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

        //Gets all ToDos from the file system, reading from each project's ToDos directory
        //Compiles the list of ALL ToDos across all projects
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

        //Gets all projects from the file system, reading from each project's project.json file
        //Compiles the list of ALL projects
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
            //Find if the ToDo already exists
            //If the ToDo exists and the ProjectId has changed, delete the old file
            //Also check if the new Project already has a ToDo with this Id and if so, assign a new Id
            var existingToDo = ToDos.FirstOrDefault(t => t.Id == toDo.Id);
            if (existingToDo != null && existingToDo.ProjectId != toDo.ProjectId)
            {
                string oldToDoPath = $"{_projectRoot}\\Project_{existingToDo.ProjectId}\\ToDos\\{toDo.Id}.json";
                if (File.Exists(oldToDoPath))
                {
                    File.Delete(oldToDoPath);
                    var oldProject = Projects.FirstOrDefault(p => p.Id == existingToDo.ProjectId);
                    UpdateProjectCompletePercentage(oldProject);
                }

                //Check if the target project already has a ToDo with this Id
                string newToDoPath = $"{_projectRoot}\\Project_{toDo.ProjectId}\\ToDos\\{toDo.Id}.json";
                if (File.Exists(newToDoPath))
                {
                    //Assign a new unique Id for the target project
                    toDo.Id = LastToDoKey(toDo.ProjectId) + 1;
                }
            }




            //Set up a new Id if one doesn't already exist
            if (toDo.Id <= 0)
            {
                toDo.Id = LastToDoKey(toDo.ProjectId) + 1;
            }
            //Find the project that the ToDo will belong to
            var project = Projects.FirstOrDefault(p => p.Id == toDo.ProjectId);

            string toDoPath = $"{_projectRoot}\\Project_{project.Id}\\ToDos";
            string path = $"{toDoPath}\\{toDo.Id}.json";

            //Write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(toDo));
            UpdateProjectCompletePercentage(project);

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
            //if the user wants to expand the projects, we add the ToDos to each project
            //Done so that the ToDos are not loaded into memory unless needed
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

        //Delete the project, removing its directory and all files within it recursively
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

        //Delete the ToDo, removing its file from the file system
        //Also updates the complete percentage of the project it belonged to
        public void DeleteToDo(ToDo? toDo)
        {
            if (toDo == null)
            {
                return;
            }
            var project = Projects.FirstOrDefault(p => p.Id == toDo.ProjectId);

            string toDoPath = $"{_projectRoot}\\Project_{project.Id}\\ToDos";

            string path = $"{toDoPath}\\{toDo.Id}.json";
            
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            UpdateProjectCompletePercentage(project);


        }

        //Updates the complete percentage of the project
        //Counts completed ToDos and divides by total ToDos in the project
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

            //Updates the project file with the new complete percentage
            //Needed because the .json file is not automatically updated
            string projectPath = $"{_projectRoot}\\Project_{project.Id}\\project.json";
            File.WriteAllText(projectPath, JsonConvert.SerializeObject(project));
        }
    }

   
}