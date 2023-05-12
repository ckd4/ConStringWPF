using Lab8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8
{
    public class TeachersF
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool IsStaff { get; set; }
        public int Hours { get; set; }
    }
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Load
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int Hours { get; set; }
    }

    public class LoadBalancer
    {
        private readonly List<TeachersF> _teachers;
        private readonly List<Load> _loads;

        public LoadBalancer(List<TeachersF> teachers, List<Load> loads)
        {
            _teachers = teachers;
            _loads = loads;
        }

        public void RedistributeLoad(int totalHours, bool staffOnly)
        {
            var teachersToDistribute = _teachers
                .Where(t => !staffOnly || t.IsStaff)
                .OrderBy(t => t.Hours)
                .ToList();

            int hoursLeftToDistribute = totalHours;

            foreach (var teacher in teachersToDistribute)
            {
                int hoursToDistribute = Math.Min(hoursLeftToDistribute, teacher.Hours);
                teacher.Hours -= hoursToDistribute;
                hoursLeftToDistribute -= hoursToDistribute;

                if (hoursLeftToDistribute == 0)
                {
                    break;
                }
            }

            if (hoursLeftToDistribute > 0)
            {
                throw new Exception("Not enough teachers to distribute the load.");
            }
        }
    }
}
/* Пример использования:
var teachers = new List<TeachersF>
{
    new Teacher { Id = 1, FullName = "Иванов Иван Иванович", IsStaff = true, Hours = 20 }
}
*/
