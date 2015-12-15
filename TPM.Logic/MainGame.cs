using System.Windows.Controls;
using TPM.Logic.Game;
using TPM.Logic.Twitch;

namespace TPM.Logic
{
    public class MainGame
    {
        public Board Board { get; private set; }

        private Difficulty difficulty = Difficulty.MEDIUM;

        public MainGame(Grid cellGrid, double Height)
        {
            Difficulty.ScreenHeight = Height;

            Board = new Board()
            {
                Difficulty = difficulty,
                CellGrid = cellGrid
            };

            StartGame();
        }

        private void StartGame()
        {
            Board.CreateBoard();

            Board.PopulateBombs(difficulty.Bombs);
        }

        private void ClickCell()
        {
            Board.ClickCell();
        }

        private void MarkCell(bool flag, bool normal)
        {
            Board.MarkCell(flag, normal);
        }

        private void PressedArrowKey(VotingAction key)
        {
            Board.MoveCursor(key);
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
