using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Infrastructure
{
    public static class CloseExcel
    {
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static bool TryKillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0) return false;
            try
            {
                Process.GetProcessById((int)processID).Kill();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
