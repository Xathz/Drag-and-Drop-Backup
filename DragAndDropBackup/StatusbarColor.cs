using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DragAndDropBackup {

    /// <remarks>https://stackoverflow.com/a/9753302</remarks>
    public static class StatusbarState {

        public enum TaskbarStates {
            Normal = 1,
            Error = 2,
            Paused = 3
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        public static void SetState(this ProgressBar pBar, TaskbarStates state) => SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);

    }

}
