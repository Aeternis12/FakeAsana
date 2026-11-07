using Asana.Library.Model;
using Asana.Library.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FakeAsana.API.Database
{
    public class DatabaseService : IDataBaseService
    {
        private readonly string _connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=AsanaDb;Trusted_Connection=True;TrustServerCertificate=True;";

        private static DatabaseService? _instance;
        public static DatabaseService Current => _instance ??= new DatabaseService();

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<List<Project>> GetProjectsAsync(bool expand = false)
        {
            using var db = CreateConnection();
            var projectList = (await db.QueryAsync<Project>("SELECT * FROM Projects")).ToList();

            if (expand)
            {
                var allToDos = (await db.QueryAsync<ToDo>("SELECT * FROM ToDos")).ToList();
                foreach (var project in projectList)
                {
                    project.ToDos = allToDos.Where(t => t.ProjectId == project.Id).ToList();
                }
            }

            return projectList;
        }

        public async Task<List<ToDo>> GetToDosAsync()
        {
            using var db = CreateConnection();
            var ToDoList = (await db.QueryAsync<ToDo>("SELECT * FROM ToDos")).ToList();
            return ToDoList;
        }

        public async Task<Project> AddOrUpdateProjectAsync(Project project)
        {
            using var db = CreateConnection();

            if (project.Id <= 0)
            {
                const string sql = @"
                    INSERT INTO Projects (Name, Description, CompletePercent)
                    VALUES (@Name, @Description, @CompletePercent);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                project.Id = await db.ExecuteScalarAsync<int>(sql, project);
            }
            else
            {
                const string sql = @"
                    UPDATE Projects
                    SET Name = @Name, Description = @Description, CompletePercent = @CompletePercent
                    WHERE Id = @Id;";

                await db.ExecuteAsync(sql, project);
            }

            return project;
        }

        public async Task<ToDo> AddOrUpdateToDoAsync(ToDo toDo)
        {
            using var db = CreateConnection();

            if (toDo.Id <= 0)
            {
                const string sql = @"
                    INSERT INTO ToDos (ProjectId, Name, Description, Priority, IsCompleted, DueDate)
                    VALUES (@ProjectId, @Name, @Description, @Priority, @IsCompleted, @DueDate);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                toDo.Id = await db.ExecuteScalarAsync<int>(sql, toDo);
            }
            else
            {
                const string sql = @"
                    UPDATE ToDos
                    SET ProjectId = @ProjectId, Name = @Name, Description = @Description,
                        Priority = @Priority, IsCompleted = @IsCompleted, DueDate = @DueDate
                    WHERE Id = @Id;";

                await db.ExecuteAsync(sql, toDo);
            }

            await UpdateProjectCompletePercentageAsync(toDo.ProjectId);

            return toDo;
        }

        public async Task DeleteProjectAsync(Project project)
        {
            if (project == null || project.Id <= 0) return;

            using var db = CreateConnection();
            await db.ExecuteAsync("DELETE FROM Projects WHERE Id = @Id", new { Id = project.Id });
        }

        public async Task DeleteToDoAsync(ToDo toDo)
        {
            if (toDo == null || toDo.Id <= 0) return;

            using var db = CreateConnection();
            await db.ExecuteAsync("DELETE FROM ToDos WHERE Id = @Id", new { Id = toDo.Id });

            await UpdateProjectCompletePercentageAsync(toDo.ProjectId);
        }

        public async Task UpdateProjectCompletePercentageAsync(int projectId)
        {
            using var db = CreateConnection();

            var projectToDos = (await db.QueryAsync<ToDo>("SELECT * FROM ToDos WHERE ProjectId = @ProjectId", new { ProjectId = projectId })).ToList();

            double percent = 0;
            if (projectToDos.Count > 0)
            {
                int completedCount = projectToDos.Count(t => t.IsCompleted == true);
                percent = Math.Round((double)completedCount / projectToDos.Count * 100, 2);
            }

            await db.ExecuteAsync("UPDATE Projects SET CompletePercent = @Percent WHERE Id = @Id", new { Percent = percent, Id = projectId });
        }
    }
}