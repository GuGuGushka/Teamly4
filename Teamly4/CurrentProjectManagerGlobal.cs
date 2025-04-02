using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamly4
{
    public static class CurrentProjectManagerGlobal
    {
        public static String Name { get; set; }
        public static String Description { get; set; }
        public static DateTime StartDateTime { get; set; }
        public static DateTime FinishDateTime { get; set; }
        public static List<Users> PerformersOfProject { get; set; }
        public static List<Tasks> TasksNames { get; set; }
    }
}
