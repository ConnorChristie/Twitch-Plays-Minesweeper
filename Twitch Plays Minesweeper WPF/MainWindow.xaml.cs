using System;
using System.Threading;
using System.Timers;
using System.Windows;
using Twitch_Plays_Minesweeper.Game;
using Twitch_Plays_Minesweeper.Twitch;
using Twitch_Plays_Minesweeper_WPF.Twitch;

namespace Twitch_Plays_Minesweeper_WPF
{
    public partial class MainWindow : Window
    {
        private static MainWindow instance;

        private System.Timers.Timer timer;
        private DateTime _StartTime = DateTime.Now;

        public MainGame Game { get; private set; }
        public MainTwitch Twitch { get; private set; }

        public static SynchronizationContext synchronizationContext;

        public MainWindow()
        {
            instance = this;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new System.Timers.Timer();
            synchronizationContext = SynchronizationContext.Current;

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            Game = new MainGame(CellHolder, ActualHeight - 41);
            Twitch = new MainTwitch(Game, new Voting(Game, votingPanel, countdown));
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpan elapsed = DateTime.Now.Subtract(_StartTime);

            synchronizationContext.Send(_ => UpdateElapsed(elapsed), null);
        }

        private void UpdateElapsed(TimeSpan elapsed)
        {
            playedForCounter.Content = string.Format("{0} day{3}, {1} hour{4}, {2} min{5}",
                elapsed.Days, elapsed.Hours, elapsed.Minutes,
                elapsed.Days == 1 ? "" : "s", elapsed.Hours == 1 ? "" : "s", elapsed.Minutes == 1 ? "" : "s");
        }

        public static MainWindow GetInstance()
        {
            return instance;
        }
    }
}
