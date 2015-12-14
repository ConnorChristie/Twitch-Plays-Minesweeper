using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Controls;
using TPM.Logic.Game;

namespace TPM.Logic.Twitch
{
    public class Voting
    {
        private System.Timers.Timer timer;
        private MainGame game;

        private int countdownTime = 30;
        private int countdownTimer = 30;

        public bool GameOver { get; set; }

        private Panel votingPanel;
        private Label countdownLabel;

        private Dictionary<VotingAction, Votes> votes = new Dictionary<VotingAction, Votes>();

        private SynchronizationContext synchronizationContext;

        public Voting(MainGame game, Panel votingPanel, Label countdownLabel)
        {
            timer = new System.Timers.Timer();

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            this.game = game;
            this.votingPanel = votingPanel;
            this.countdownLabel = countdownLabel;

            synchronizationContext = SynchronizationContext.Current;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            synchronizationContext.Send(_ => UpdateCountdownLabel(), null);

            if (countdownTimer <= 0)
            {
                synchronizationContext.Send(_ => CommitAction(), null);

                countdownTimer = countdownTime;
            }
            else
            {
                countdownTimer--;
            }
        }

        private void UpdateCountdownLabel()
        {
            countdownLabel.Content = string.Format("{0} second{1}", countdownTimer, countdownTimer == 1 ? "" : "s");
        }

        public void AddVote(VotingAction action)
        {
            var vote = from v in votes
                       where v.Key.Action == action.Action
                       select v;

            if (vote.Count() > 0)
            {
                var actVote = vote.First();

                actVote.Key.Count = (actVote.Key.Count + action.Count) / 2;
                actVote.Value.AddVote(action);

                DisplaySortedVotes();
            }
            else
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

            foreach (KeyValuePair<VotingAction, Votes> vote in sorted)
            {
                vote.Value.AddToPanel(votingPanel, index++);
            }
        }

        private IOrderedEnumerable<KeyValuePair<VotingAction, Votes>> GetSortedVotes()
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

    public class VotingAction
    {
        public Action Action { get; set; }

        public int Count { get; set; }
    }
}
