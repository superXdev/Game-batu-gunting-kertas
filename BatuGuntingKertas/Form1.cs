using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;
using Microsoft.Win32;

namespace BatuGuntingKertas
{
    public partial class Form1 : MetroForm
    {
        // Game log registry
        RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"Software", true);
        // others
        string[] computerHand = { "batu", "gunting", "kertas" };
        Random rnd = new Random();
        int winStreak, loseStreak = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // check if subkey is exist or not
            if (reg.OpenSubKey("BGK") == null)
            {
                // Create new subkey
                reg.CreateSubKey("BGK");
                // initiation value
                reg.OpenSubKey("BGK", true).SetValue("total_win", 0);
                reg.OpenSubKey("BGK", true).SetValue("total_lose", 0);
                reg.OpenSubKey("BGK", true).SetValue("win_streak", 0);
                reg.OpenSubKey("BGK", true).SetValue("lose_streak", 0);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Checking game result
        /// </summary>
        /// <param name="computerHand">generated string from Array</param>
        /// <param name="playerHand">input string for player</param>
        /// <returns>Win or Lose</returns>
        internal bool isPlayerWin(string computerHand, string playerHand)
        {
            // check the winner
            if (computerHand == "batu")
            {
                return (playerHand == "kertas") ? true : false;
            }
            else if (computerHand == "gunting")
            {
                return (playerHand == "batu") ? true : false;
            }
            else if(computerHand=="kertas")
            {
                return (playerHand == "gunting") ? true : false;
            }
            return false;
        }

        /// <summary>
        /// Get bitmap from game resources
        /// </summary>
        /// <param name="hand">input string</param>
        /// <returns>Bitmap icon</returns>
        internal Bitmap HandIcon(string hand)
        {
            if (hand == "batu")
                return GameResources.icons8_punch_100_n;
            else if (hand == "gunting")
                return GameResources.icons8_hand_peace_100_n;
            else
                return GameResources.icons8_hand_64_n;
        }

        /// <summary>
        /// Showing game result and game log process
        /// </summary>
        /// <param name="computer"></param>
        /// <param name="player"></param>
        internal void ShowTheWinner(string computer, string player)
        {
            // Disable button
            BatuBtn.Enabled = false;
            GuntingBtn.Enabled = false;
            KertasBtn.Enabled = false;

            // show result label
            ResultLbl.Visible = true;
            if (computer == player)
            {
                ResultLbl.Text = "Draw";
                TextPanel.BackColor = Color.DarkGray;
            }
            else  if (isPlayerWin(computer, player))
            {
                ResultLbl.Text = "You Win!";
                TextPanel.BackColor = Color.MediumSeaGreen;
                winStreak++;
                loseStreak = 0;
                // Game Log
                Log.TotalWin += 1;
                if (winStreak > Log.WinStreak)
                    Log.WinStreak += 1;
            }
            else
            {
                ResultLbl.Text = "You Lose!";
                TextPanel.BackColor = Color.IndianRed;
                loseStreak++;
                winStreak = 0;
                // Game Log
                Log.TotalLose += 1;
                if (loseStreak > Log.LoseStreak)
                    Log.LoseStreak += 1;
            }
            // Winstreak & losestreak
            winLbl.Text = Log.WinStreak.ToString();
            loseLbl.Text = Log.LoseStreak.ToString();

            // Show hands icon
            pictureBox1.BackgroundImage = HandIcon(computer);
            pictureBox2.BackgroundImage = HandIcon(player);
        }

        internal void GameLog(bool show = false)
        {
            RegistryKey gameRegistry = reg.OpenSubKey("BGK", true);

            // get all value
            int total_win = Int32.Parse(gameRegistry.GetValue("total_win").ToString());
            int total_lose = Int32.Parse(gameRegistry.GetValue("total_lose").ToString());
            int win_streak = Int32.Parse(gameRegistry.GetValue("win_streak").ToString());
            int lose_streak = Int32.Parse(gameRegistry.GetValue("lose_streak").ToString());

            // just show game log
            if (show)
            {
                MessageBox.Show("Total Win : " + total_win + "\nTotal Lose : " + total_lose + "\n\nWin Streak : " + win_streak + "\nLose Streak : " + lose_streak);
                return;
            }

            // save game log
            gameRegistry.SetValue("total_win", total_win + Log.TotalWin);
            gameRegistry.SetValue("total_lose", total_win + Log.TotalLose);

            if (Log.WinStreak > win_streak)
                gameRegistry.SetValue("win_streak", Log.WinStreak);
            if (Log.LoseStreak > lose_streak)
                gameRegistry.SetValue("lose_streak", Log.LoseStreak);
        }

        private void BatuBtn_Click(object sender, EventArgs e)
        {
            ShowTheWinner(computerHand[rnd.Next(0, 2)], "batu");
        }

        private void GuntingBtn_Click(object sender, EventArgs e)
        {
            ShowTheWinner(computerHand[rnd.Next(0, computerHand.Length)], "gunting"); // aneh emang
        }

        private void KertasBtn_Click(object sender, EventArgs e)
        {
            ShowTheWinner(computerHand[rnd.Next(0, 2)], "kertas");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            // Enable button
            BatuBtn.Enabled = true;
            GuntingBtn.Enabled = true;
            KertasBtn.Enabled = true;

            // Clear image
            pictureBox1.BackgroundImage = null;
            pictureBox2.BackgroundImage = null;

            ResultLbl.Visible = false;
            TextPanel.BackColor = Color.RoyalBlue;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            GameLog(true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameLog();
        }

        private void KertasBtn_MouseHover(object sender, EventArgs e)
        {
        }

        private void KertasBtn_MouseLeave(object sender, EventArgs e)
        {
            KertasBtn.BackColor = Color.White;
        }

        private void KertasBtn_MouseEnter(object sender, EventArgs e)
        {
            KertasBtn.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void GuntingBtn_MouseEnter(object sender, EventArgs e)
        {
            GuntingBtn.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void GuntingBtn_MouseLeave(object sender, EventArgs e)
        {
            GuntingBtn.BackColor = Color.White;
        }

        private void BatuBtn_MouseEnter(object sender, EventArgs e)
        {
            BatuBtn.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void BatuBtn_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void BatuBtn_MouseLeave(object sender, EventArgs e)
        {
            BatuBtn.BackColor = Color.White;
        }
    }
}
