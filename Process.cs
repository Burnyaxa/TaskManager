using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFO
{
    public class Process : IComparable, IPrioritizable
    {
        public int Id { get; set; }
        public int ExecutionTime { get; set; }
        public int WaitingTime { get; set; }
        public int Priority { get; set; }

        public int CompareTo(object obj)
        {
            Process process = obj as Process;
            return this.Priority - process.Priority;
        }
    }
}
