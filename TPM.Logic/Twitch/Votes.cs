using System.Windows;
using System.Windows.Controls;

namespace TPM.Logic.Twitch
{
    public class Votes
    {
        private int index = 0;

        private Label titleLabel;
        private Label valueLabel;

        public int _Votes { get; private set; }

        public Votes(int index, VotingAction action)
        {
            SetupVoteLabels(this.index = index, action, 1);
        }

        internal void AddVote(VotingAction action)
        {
            _Votes++;
            
            SetLabelsText(action.Action.Title, action.Count, _Votes);
        }

        internal void AddToPanel(Panel panel)
        {
            AddToPanel(panel, index);
        }

        internal void AddToPanel(Panel panel, int index)
        {
            SetPosition(index);

            panel.Children.Add(titleLabel);
            panel.Children.Add(valueLabel);
        }

        internal void SetPosition(int index)
        {
            titleLabel.Margin = new Thickness(10, 10 + (index * 40), 0, 0);
            valueLabel.Margin = new Thickness(171, 10 + (index * 40), 0, 0);
        }

        private void SetLabelsText(string title, int count, int value)
        {
            titleLabel.Content = string.Format("{0}: {1}", title, count);
            valueLabel.Content = value;
        }

        private void SetupVoteLabels(int index, VotingAction action, int value)
        {
            titleLabel = new Label();
            valueLabel = new Label();

            titleLabel.HorizontalAlignment = HorizontalAlignment.Left;
            titleLabel.VerticalAlignment = VerticalAlignment.Top;
            titleLabel.Margin = new Thickness(10, 10 + (index * 40), 0, 0);
            titleLabel.Width = 156;
            titleLabel.FontWeight = FontWeights.Bold;
            titleLabel.FontSize = 26.667;
            titleLabel.Padding = new Thickness(0);
            
            valueLabel.HorizontalAlignment = HorizontalAlignment.Left;
            valueLabel.VerticalAlignment = VerticalAlignment.Top;
            valueLabel.HorizontalContentAlignment = HorizontalAlignment.Right;
            valueLabel.Margin = new Thickness(171, 10 + (index * 40), 0, 0);
            valueLabel.Width = 73;
            valueLabel.FontWeight = FontWeights.Bold;
            valueLabel.FontSize = 26.667;
            valueLabel.Padding = new Thickness(0);

            SetLabelsText(action.Action.Title, action.Count, value);
        }
    }
}
