using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Utils.Threading
{
    public static class QueueAction
    {
        internal static Queue<Action> actions = new();

        public static void Sleep(int milliseconds, Action action)
        {
            Task.Run(() =>
            {
                Sleep(milliseconds, action, true);
            });
        }
        internal async static Task Sleep(int milliseconds, Action action, bool interno)
        {
            await Task.Delay(milliseconds);

            actions.Enqueue(action);
        }

    }
}
