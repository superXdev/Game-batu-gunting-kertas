using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BatuGuntingKertas
{
    class Log
    {
        private static int total_win, total_lose, win_streak, lose_streak = 0;

        public static int TotalWin
        {
            get { return total_win; }
            set { total_win = value; }
        }

        public static int TotalLose
        {
            get { return total_lose; }
            set { total_lose = value; }
        }

        public static int WinStreak
        {
            get { return win_streak; }
            set { win_streak = value; }
        }

        public static int LoseStreak
        {
            get { return lose_streak; }
            set { lose_streak = value; }
        }
    }
}
