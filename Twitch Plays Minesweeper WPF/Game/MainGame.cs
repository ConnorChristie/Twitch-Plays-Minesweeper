using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Twitch_Plays_Minesweeper.Game
{
    public class MainGame
    {
        private Board board;

        private Difficulty difficulty = Difficulty.MEDIUM;

        private static double ScreenHeight = 0;

        public MainGame(Grid cellGrid, double Height)
        {
            ScreenHeight = Height;

            board = new Board(difficulty.Width, difficulty.Height) { CellGrid = cellGrid, CellSize = difficulty.GetCellSize() };

            StartGame();
        }

        private void StartGame()
        {
            board.CreateBoard();

            board.PopulateBombs(difficulty.Bombs);
        }

        private void ClickCell()
        {
            board.ClickCell();
        }

        private void MarkCell(bool flag, bool normal)
        {
            board.MarkCell(flag, normal);
        }

        private void PressedArrowKey(Action key)
        {
            board.MoveCursor(key);
        }

        public void CommitAction(Action action)
        {
            switch (action.Key)
            {
                case Key.CLICK:
                    ClickCell();

                    break;
                case Key.FLAG:
                    MarkCell(true, false);

                    break;
                case Key.QUESTION:
                    MarkCell(false, false);

                    break;
                case Key.UNTOUCHED:
                    MarkCell(false, true);

                    break;
                default:
                    PressedArrowKey(action);

                    break;
            }
        }
        
        public enum Key
        {
            NEXT,

            UP,
            DOWN,
            LEFT,
            RIGHT,

            UP_LEFT,
            UP_RIGHT,

            DOWN_LEFT,
            DOWN_RIGHT,
            
            CLICK,
            FLAG,
            QUESTION,
            UNTOUCHED
        }

        public class Action
        {
            public static Action NEXT = new Action { Title = "Next", GamePlay = true, Key = Key.NEXT };

            public static Action UP = new Action { Title = "Up", GamePlay = true, Key = Key.UP };
            public static Action DOWN = new Action { Title = "Down", GamePlay = true, Key = Key.DOWN };
            public static Action LEFT = new Action { Title = "Left", GamePlay = true, Key = Key.LEFT };
            public static Action RIGHT = new Action { Title = "Right", GamePlay = true, Key = Key.RIGHT };

            public static Action UP_LEFT = new Action { Title = "Up Left", GamePlay = true, Key = Key.UP_LEFT };
            public static Action UP_RIGHT = new Action { Title = "Up Right", GamePlay = true, Key = Key.UP_RIGHT };

            public static Action DOWN_LEFT = new Action { Title = "Down Left", GamePlay = true, Key = Key.DOWN_LEFT };
            public static Action DOWN_RIGHT = new Action { Title = "Down Right", GamePlay = true, Key = Key.DOWN_RIGHT };

            public static Action CLICK = new Action { Title = "Click", GamePlay = true, Key = Key.CLICK };
            public static Action FLAG = new Action { Title = "Flag", GamePlay = true, Key = Key.FLAG };
            public static Action QUESTION = new Action { Title = "Question", GamePlay = true, Key = Key.QUESTION };
            public static Action UNTOUCHED = new Action { Title = "Untouched", GamePlay = true, Key = Key.UNTOUCHED };

            public string Title { get; private set; }
            public bool GamePlay { get; private set; }

            public Key Key { get; private set; }
        }
        
        public class Difficulty
        {
            public static Difficulty EASY = new Difficulty { Width = 10, Height = 10, Bombs = 10 };
            public static Difficulty MEDIUM = new Difficulty { Width = 50, Height = 35, Bombs = 75 };
            public static Difficulty HARD = new Difficulty { Width = 75, Height = 50, Bombs = 500 };

            public int Width { get; private set; }
            public int Height { get; private set; }

            public int Bombs { get; private set; }

            public double GetCellSize()
            {
                return ScreenHeight / Height;
            }
        }
    }
}
