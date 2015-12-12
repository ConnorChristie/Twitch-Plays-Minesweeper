using System.Windows.Controls;
using TPM.Logic.Game;

namespace Twitch_Plays_Minesweeper.Game
{
    public class MainGame
    {
        private Board board;

        private Difficulty difficulty = Difficulty.MEDIUM;
        
        public MainGame(Grid cellGrid, double Height)
        {
            Difficulty.ScreenHeight = Height;

            board = new Board() {
                Width = difficulty.Width,
                Height = difficulty.Height,
                CellGrid = cellGrid,
                CellSize = difficulty.GetCellSize()
            };

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
    }
}
