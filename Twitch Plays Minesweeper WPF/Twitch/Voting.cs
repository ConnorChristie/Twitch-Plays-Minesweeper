using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using Twitch_Plays_Minesweeper.Game;

namespace Twitch_Plays_Minesweeper_WPF.Twitch
{
    public class Voting
    {
        private Timer timer;
        private MainGame game;

        private int countdownTime = 30;
        private int countdownTimer = 30;

        public bool GameOver { get; set; }

        private Panel votingPanel;
        private Label countdownLabel;

        private Dictionary<MainGame.Action, Votes> votes = new Dictionary<MainGame.Action, Votes>();

        public Voting(MainGame game, Panel votingPanel, Label countdownLabel)
        {
            timer = new Timer();

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            this.game = game;
            this.votingPanel = votingPanel;
            this.countdownLabel = countdownLabel;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MainWindow.synchronizationContext.Send(_ => UpdateCountdownLabel(), null);
            
            if (countdownTimer <= 0)
            {
                MainWindow.synchronizationContext.Send(_ => CommitAction(), null);

                countdownTimer = countdownTime;
            } else
            {
                countdownTimer--;
            }
        }

        private void UpdateCountdownLabel()
        {
            countdownLabel.Content = string.Format("{0} second{1}", countdownTimer, countdownTimer == 1 ? "" : "s");
        }

        public void AddVote(MainGame.Action action)
        {
            var vote = from v in votes
                       where v.Key == action
                       select v;

            if (vote.Count() > 0)
            {
                KeyValuePair<MainGame.Action, Votes> actVote = vote.First();

                actVote.Value.AddVote();

                DisplaySortedVotes();
            } else
            {
                Votes aVote = new Votes(votes.Count(), action);

                votes.Add(action, aVote);

                aVote.AddToPanel(votingPanel);
            }
        }

        private void DisplaySortedVotes()
        {
            var sorted = GetSortedVotes();

            votingPanel.Children.Clear();

            int index = 0;

            foreach (KeyValuePair<MainGame.Action, Votes> vote in sorted)
            {
                vote.Value.AddToPanel(votingPanel, index++);
            }
        }

        private IOrderedEnumerable<KeyValuePair<MainGame.Action, Votes>> GetSortedVotes()
        {
            var sorted = from v in votes
                         orderby v.Value._Votes descending
                         select v;

            return sorted;
        }

        private void CommitAction()
        {
            if (votes.Count > 0)
            {
                game.CommitAction(GetSortedVotes().First().Key);

                votingPanel.Children.Clear();
                votes.Clear();
            }
        }
    }
}
