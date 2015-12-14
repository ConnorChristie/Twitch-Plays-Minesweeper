using ChatSharp;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using TPM.Logic.Game;
using TPM.Logic.Twitch;

namespace TPM.Logic
{
    public class MainTwitch
    {
        private MainGame game;
        private Voting voting;

        private string Host = "irc.twitch.tv";
        private string Channel = "#minesweeperbytwitch";

        private string Username = "MinesweeperByTwitch";
        private string Password = "oauth:qedxefrm0tv5yy54bcyp087idqr22g";
        
        private Dictionary<string, VotingAction> Actions = new Dictionary<string, VotingAction>();

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
            };

            client.ConnectAsync();
        }

        private void ParseMessage(string user, string message)
        {
            string[] components = message.Split(' ');
            string possibleCount = components.Last();

            int count = -1;

            if (int.TryParse(possibleCount, out count))
            {
                components.

                message = string.Join(" ", components);
            }

            System.Console.WriteLine("Message: " + message);

            var acts = from a in Actions
                       where a.Key.Equals(message.ToLower())
                       select a;

            if (acts.Count() > 0)
            {
                VotingAction action = acts.First().Value;
                
                action.Count = count;

                SynchronizationContext.Current.Send(_ => voting.AddVote(action), null);
            }
        }

        internal void GameOver()
        {
            voting.GameOver = true;
        }

        private void AddActions()
        {
            Actions.Add("up", new VotingAction() { Action = Action.UP });
            Actions.Add("down", new VotingAction() { Action = Action.DOWN });
            Actions.Add("left", new VotingAction() { Action = Action.LEFT });
            Actions.Add("right", new VotingAction() { Action = Action.RIGHT });

            Actions.Add("up left", new VotingAction() { Action = Action.UP_LEFT });
            Actions.Add("up right", new VotingAction() { Action = Action.UP_RIGHT });
            Actions.Add("down left", new VotingAction() { Action = Action.DOWN_LEFT });
            Actions.Add("down right", new VotingAction() { Action = Action.DOWN_RIGHT });

            Actions.Add("next", new VotingAction() { Action = Action.NEXT });

            Actions.Add("click", new VotingAction() { Action = Action.CLICK });
            Actions.Add("flag", new VotingAction() { Action = Action.FLAG });
            Actions.Add("question", new VotingAction() { Action = Action.QUESTION });
            Actions.Add("untouched", new VotingAction() { Action = Action.UNTOUCHED });
        }
    }
}
