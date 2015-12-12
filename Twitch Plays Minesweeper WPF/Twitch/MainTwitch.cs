using ChatSharp;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using Twitch_Plays_Minesweeper.Game;
using Twitch_Plays_Minesweeper_WPF.Twitch;

namespace Twitch_Plays_Minesweeper.Twitch
{
    public class MainTwitch
    {
        private MainGame game;
        private Voting voting;

        private string Host = "irc.twitch.tv";
        private string Channel = "#minesweeperbytwitch";

        private string Username = "MinesweeperByTwitch";
        private string Password = "oauth:qedxefrm0tv5yy54bcyp087idqr22g";

        private StringCollection Voted = new StringCollection();

        private Dictionary<string, MainGame.Action> Actions = new Dictionary<string, MainGame.Action>();

        public MainTwitch(MainGame game, Voting voting)
        {
            this.game = game;
            this.voting = voting;

            AddActions();

            IrcClient client = new IrcClient(Host, new IrcUser(Username, Username, Password));
            
            client.ConnectionComplete += (s, e) => client.JoinChannel(Channel);
            client.PrivateMessageRecieved += (s, e) =>
            {
                ParseMessage(e.PrivateMessage.User.RealName, e.PrivateMessage.Message);

                //Console.WriteLine("User: " + e.PrivateMessage.User.RealName + ", Message: " + e.PrivateMessage.Message);
            };

            client.ConnectAsync();
        }

        private void ParseMessage(string user, string message)
        {
            var acts = from a in Actions
                       where a.Key.Equals(message.ToLower())
                       select a;

            if (acts.Count() > 0)
            {
                MainGame.Action action = acts.First().Value;
                
                SynchronizationContext.Current.Send(_ => voting.AddVote(action), null);
            }
        }

        internal void GameOver()
        {
            voting.GameOver = true;
        }

        private void AddActions()
        {
            Actions.Add("up", MainGame.Action.UP);
            Actions.Add("down", MainGame.Action.DOWN);
            Actions.Add("left", MainGame.Action.LEFT);
            Actions.Add("right", MainGame.Action.RIGHT);

            Actions.Add("up left", MainGame.Action.UP_LEFT);
            Actions.Add("up right", MainGame.Action.UP_RIGHT);
            Actions.Add("down left", MainGame.Action.DOWN_LEFT);
            Actions.Add("down right", MainGame.Action.DOWN_RIGHT);

            Actions.Add("next", MainGame.Action.NEXT);

            Actions.Add("click", MainGame.Action.CLICK);
            Actions.Add("flag", MainGame.Action.FLAG);
            Actions.Add("question", MainGame.Action.QUESTION);
            Actions.Add("untouched", MainGame.Action.UNTOUCHED);
        }
    }
}