﻿using System.Windows;
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
            Label[] labels = GetVoteLabels(this.index = index, action, 1);

            titleLabel = labels[0];
            valueLabel = labels[1];
        }

        internal void AddVote(VotingAction action)
        {
            _Votes++;

            titleLabel.Content = action.Action.Title + "" + action.Count;
            valueLabel.Content = (int)valueLabel.Content + 1;
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

        private Label[] GetVoteLabels(int index, VotingAction action, int value)
        {
            Label nameLabel = new Label();
            Label valueLabel = new Label();

            nameLabel.Content = action.Action.Title + "" + action.Count;
            nameLabel.HorizontalAlignment = HorizontalAlignment.Left;
            nameLabel.VerticalAlignment = VerticalAlignment.Top;
            nameLabel.Margin = new Thickness(10, 10 + (index * 40), 0, 0);
            nameLabel.Width = 156;
            nameLabel.FontWeight = FontWeights.Bold;
            nameLabel.FontSize = 26.667;
            nameLabel.Padding = new Thickness(0);

            valueLabel.Content = value;
            valueLabel.HorizontalAlignment = HorizontalAlignment.Left;
            valueLabel.VerticalAlignment = VerticalAlignment.Top;
            valueLabel.HorizontalContentAlignment = HorizontalAlignment.Right;
            valueLabel.Margin = new Thickness(171, 10 + (index * 40), 0, 0);
            valueLabel.Width = 73;
            valueLabel.FontWeight = FontWeights.Bold;
            valueLabel.FontSize = 26.667;
            valueLabel.Padding = new Thickness(0);

            return new Label[] { nameLabel, valueLabel };
        }
    }
}