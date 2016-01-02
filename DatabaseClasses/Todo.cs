using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClasses
{
    public class ToDo
    {
        public ToDo() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Finnished { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeadLine { get; set; }
        public int EstimationTime { get; set; }

        /// <summary>
        /// Sorterar efter deadline.
        /// </summary>
        public class SortByDeadLineClass : IComparer<ToDo>
        {
            public int Compare(ToDo t1, ToDo t2)
            {
                return (DateTime.Compare(t1.DeadLine, t2.DeadLine));
            }
        }

        /// <summary>
        /// Overrider basklassen ToString metod med egen.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = String.Format("Id: {0} | Name: {1} | Description: {2} | Finished: {3} | CreateDate: {4} | DeadLine: {5} | EstimationTime: {6} min",
                Id, Name, Description, Finnished, CreatedDate, DeadLine, EstimationTime);
            return s;
        }
    }
}
