using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFO
{
    class Program
    {
        private const int _minPriority = 5;
        private const int _maxPriority = 1;

        private const int _minExecutionTime = 10;
        private const int _maxExecutionTime = 50;

        public static int TotalTime = 0;
        public static int AverageWaitingTime = 0;
        public static int Interval = 0;
        public static int Idle = 0;
        static void Main(string[] args)
        {
            Console.Write("Amount of processes: ");
            int quantity = Convert.ToInt32(Console.ReadLine());
            List<Process> processes = GenerateProcesses(quantity);

            for (int i = 0; i <= 50; i += 5)
            {
                PriorityQueue<Process> queue = GenerateQueue(processes);
                ExecuteProcesses(queue, processes, i);
                int sumWaitingTime = processes.Sum(x => x.WaitingTime);
                double averageWaitingTime = (double)sumWaitingTime / (double)processes.Count;
                double idlePercent = ((double)Idle / ((double)TotalTime + (double)Idle)) * 100;
                Console.WriteLine("Average waiting time with queue interval {0}: {1}, IDLE: {2}%",i, averageWaitingTime, idlePercent);

                var averagePriorityWaitingTime = processes.GroupBy(x => x.Priority)
                    .Select(x => new { 
                    Priority = x.Key,
                    WaitingTime = x.Average(x => x.WaitingTime)
                    });

                foreach(var item in averagePriorityWaitingTime)
                {
                    Console.WriteLine("Average waiting time for priority {0}: {1}", item.Priority.ToString(), item.WaitingTime);
                }

                Interval = 0;
                Idle = 0;
                TotalTime = 0;
            }

        }

        static List<Process> GenerateProcesses(int quantity)
        {
            List<Process> processes = new List<Process>();
            Random random = new Random();
            int priority, executionTime;
            for (int i = 0; i < quantity; i++)
            {
                priority = random.Next(_maxPriority, _minPriority);
                executionTime = random.Next(_minExecutionTime, _maxExecutionTime);
                processes.Add(new Process { Id = i, Priority = priority, ExecutionTime = executionTime, WaitingTime = 0 });
            }
            return processes;
        }

        static PriorityQueue<Process> GenerateQueue(List<Process> processes)
        {
            PriorityQueue<Process> queue = new PriorityQueue<Process>();
            for(int i = 0; i < processes.Count; i++)
            {
                queue.Enqueue(processes[i]);
            }
            return queue;
        }

        static void ExecuteProcesses(PriorityQueue<Process> queue, List<Process> processes, int difference)
        {
            Process process;
            while(queue.Count() != 0)
            {
                process = queue.Dequeue();
         
                processes.Where(x => x.Id == process.Id)
                    .First()
                    .WaitingTime = TotalTime - Interval < 0 ? 0 : TotalTime - Interval;
                Idle += TotalTime - Interval < 0 ? Interval - TotalTime : 0;
                Interval += difference;

                TotalTime += process.ExecutionTime;

                Console.WriteLine("Process id{0} Priority {1} Execution time {2} Waiting time {3}", process.Id,
                    process.Priority,
                    process.ExecutionTime,
                    process.WaitingTime);

                if (queue.Count() == 0)
                {
                    Console.WriteLine("Execution ended. Total time : {0}", TotalTime);
                }
            }
        }
    }
}
