using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessingBigData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("同步中……");
            TaskAsyncParallelBD task = new TaskAsyncParallelBD();
            task.SynchCourseware();
            Console.WriteLine("同步结束");
        }
    }
}
