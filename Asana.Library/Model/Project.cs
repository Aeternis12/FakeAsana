using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Model
{
    public class Project
    {
        public List<ToDo> ToDos { get; set; } = new List<ToDo>();
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? CompletePercent
        {
            get
            {
                if (ToDos.Count == 0.0 || ToDos == null)
                    return null;

                double completedAmount = ToDos.Count(t => t.IsCompleted == true);
                return completedAmount / ToDos.Count * 100.0;
            }
        }
        public int? Id { get; set; }

        public override string ToString()
        {
            return $"Project [#{Id}] {Name} - {Description} -- {CompletePercent ?? 0.0}% Complete";
        }
    }
}
