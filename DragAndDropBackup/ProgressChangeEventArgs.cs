using System;

namespace DragAndDropBackup {

    public class ProgressChangeEventArgs : EventArgs {

        /// <summary>
        /// Current percentage completed.
        /// </summary>
        public int Percent { get; private set; }

        /// <summary>
        /// Time remaining in seconds.
        /// </summary>
        public int TimeRemaining { get; private set; }

        /// <summary>
        /// Event data for when the progress changes.
        /// </summary>
        /// <param name="percent">Current percentage completed.</param>
        /// <param name="timeRemaining">Time remaining in seconds.</param>
        public ProgressChangeEventArgs(int percent, int timeRemaining) {
            Percent = percent;
            TimeRemaining = timeRemaining;
        }

    }

}
