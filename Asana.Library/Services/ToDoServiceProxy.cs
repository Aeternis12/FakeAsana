using Asana.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ToDoServiceProxy
    {

       
        public List<ToDo> ToDos
        {
            get
            {
                return ProjectServiceProxy.Current.Projects.SelectMany(p => p.ToDos).Take(100).ToList();
            }
            
        }

        private ToDoServiceProxy() 
        {
            var projects = ProjectServiceProxy.Current.Projects;
        }

        private static ToDoServiceProxy? instance;

        private int nextKey
        {
            get
            {
                var allToDos = ToDos;
                if (allToDos.Any())
                {
                    return allToDos.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static ToDoServiceProxy Current
        {
            get
            {
                if(instance == null)
                {
                    instance = new ToDoServiceProxy();
                }

                return instance;
            }
        }

        public ToDo? GetById(int id)
        {
            return ToDos.FirstOrDefault(t => t.Id == id);
        }

        public List<ToDo> GetByProjectId(int projectId)
        {
            return ToDos.Where(t => t.ProjectId == projectId).ToList();
        }

        public ToDo AddOrUpdate(ToDo? toDo)
        {
            if (toDo == null)
            {
                return toDo;
            }

            var targetProject = ProjectServiceProxy.Current.GetById(toDo.ProjectId);
            foreach (var project in ProjectServiceProxy.Current.Projects)
            {
                var existing = project.ToDos.FirstOrDefault(t => t.Id == toDo.Id);
                if (existing != null)
                {
                    project.ToDos.Remove(existing);
                    ProjectServiceProxy.Current.UpdateCompletePercent(targetProject);
                    break;
                }
            }

            if (toDo.Id == 0)
            {
                toDo.Id = nextKey;
                targetProject.ToDos.Add(toDo);
                ProjectServiceProxy.Current.UpdateCompletePercent(targetProject);
            }
            else
            {
                var existingToDo = targetProject.ToDos.FirstOrDefault(t => t.Id == toDo.Id);
                if (existingToDo != null)
                {
                    existingToDo.Description = toDo.Description;
                    existingToDo.IsCompleted = toDo.IsCompleted;
                    existingToDo.DueDate = toDo.DueDate;
                    existingToDo.Priority = toDo.Priority;
                }
                else
                {
                    targetProject.ToDos.Add(toDo);
                    ProjectServiceProxy.Current.UpdateCompletePercent(targetProject);
                }
            }
            return toDo;
        }

        public void DeleteToDo(ToDo? toDo)
        {
            var project = ProjectServiceProxy.Current.GetById(toDo.ProjectId);
            if (project == null)
            {
                return;
            }
            if (toDo == null)
            {
                return;
            }

            project.ToDos.Remove(toDo);
        }
    }
}
