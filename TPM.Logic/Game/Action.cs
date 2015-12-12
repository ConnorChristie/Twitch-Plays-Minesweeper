using System.Drawing;

namespace TPM.Logic.Game
{
    public class Action
    {
        #region Move Keys
        public static Action NEXT = new Action
        {
            Title = "Next",
            GamePlay = true,
            Key = Key.NEXT
        };

        public static Action UP = new Action
        {
            Title = "Up",
            GamePlay = true,
            Key = Key.UP,
            Act = board =>
            {
                Cell cell = null;

                for (int i = board.CursorPos.Y - 1; i >= 0; i--)
                    if (board.GetCellRelativeTo(board.CursorPos.X, i, out cell)) return cell;

                return null;
            }
        };
        public static Action DOWN = new Action
        {
            Title = "Down",
            GamePlay = true,
            Key = Key.DOWN
        };
        public static Action LEFT = new Action
        {
            Title = "Left",
            GamePlay = true,
            Key = Key.LEFT
        };
        public static Action RIGHT = new Action
        {
            Title = "Right",
            GamePlay = true,
            Key = Key.RIGHT
        };

        public static Action UP_LEFT = new Action
        {
            Title = "Up Left",
            GamePlay = true,
            Key = Key.UP_LEFT
        };
        public static Action UP_RIGHT = new Action
        {
            Title = "Up Right",
            GamePlay = true,
            Key = Key.UP_RIGHT
        };

        public static Action DOWN_LEFT = new Action
        {
            Title = "Down Left",
            GamePlay = true,
            Key = Key.DOWN_LEFT
        };
        public static Action DOWN_RIGHT = new Action
        {
            Title = "Down Right",
            GamePlay = true,
            Key = Key.DOWN_RIGHT
        };
        #endregion

        #region Action Keys
        public static Action CLICK = new Action
        {
            Title = "Click",
            GamePlay = true,
            Key = Key.CLICK
        };
        public static Action FLAG = new Action
        {
            Title = "Flag",
            GamePlay = true,
            Key = Key.FLAG
        };
        public static Action QUESTION = new Action
        {
            Title = "Question",
            GamePlay = true,
            Key = Key.QUESTION
        };
        public static Action UNTOUCHED = new Action
        {
            Title = "Untouched",
            GamePlay = true,
            Key = Key.UNTOUCHED
        };
        #endregion

        public string Title{ get; private set; }
        public bool GamePlay { get; private set; }

        public Key Key { get; private set; }

        public System.Func<Board, Cell> Act { get; private set; }
    }
}
