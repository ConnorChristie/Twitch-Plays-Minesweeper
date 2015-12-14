using System.Windows.Controls;
using TPM.Logic.Game;
using TPM.Logic.Twitch;

namespace TPM.Logic
{
    public class MainGame
    {
        private Board board;

        private Difficulty difficulty = Difficulty.MEDIUM;

        public MainGame(Grid cellGrid, double Height)
        {
            Difficulty.ScreenHeight = Height;

            board = new Board()
            {
                Difficulty = difficulty,
                CellGrid = cellGrid
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

        private void PressedArrowKey(VotingAction key)
        {
            board.MoveCursor(key);
        }

        public void CommitAction(VotingAction action)
        {
            switch (action.Action.Key)
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
