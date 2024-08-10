using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Utils
{
    public class Debug
    {
        public static void PrintLine(params object[] args)
        {
            string message = "";
            foreach (object arg in args)
            {
                if (arg is string)
                {
                    message += (string)arg + " ";
                }
                else if (arg is int)
                {
                    message += arg.ToString() + " ";
                }
                else
                {
                    message += arg.ToString() + " ";
                }
            }
            Console.WriteLine(message);
        }
    }
}
