using System;
using System.Drawing;
using System.Windows.Controls;
using TPM.Logic.Twitch;

namespace TPM.Logic.Game
{
    public class Board
    {
        public Difficulty Difficulty { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Grid CellGrid { get; set; }
        public Point CursorPos { get; private set; }

        public double CellSize { get; private set; }

        public int OffsetX = 1;
        public int OffsetY = 1;

        private Cell[,] cells;

        private Random random = new Random();

        private int[] bombsX;
        private int[] bombsY;

        public void CreateBoard()
        {
            Width = Difficulty.Width;
            Height = Difficulty.Height;

            CellSize = Difficulty.GetCellSize();

            cells = new Cell[Width, Height];

            CursorPos = new Point(0, 0);

            CellGrid.Width = Width * CellSize + OffsetX;
            CellGrid.Height = Height * CellSize + OffsetY;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cells[x, y] = CreateCell(x, y);
                }
            }

            GetCell(0, 0).Hover(true);
        }

        public void PopulateBombs(int bombs)
        {
            bombsX = new int[bombs];
            bombsY = new int[bombs];

            for (int i = 0; i < bombs; i++)
            {
                SetRandomBomb();
            }
        }

        private void SetRandomBomb()
        {
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);

            Cell cell = GetCell(x, y);

            if (!cell.IsBomb)
            {
                cell.IsBomb = true;

                CheckBombsAround(cell);
            }
            else
            {
                SetRandomBomb();
            }
        }

        private void CheckBombsAround(Cell cell)
        {
            for (int x = cell.X - 1; x <= cell.X + 1; x++)
            {
                for (int y = cell.Y - 1; y <= cell.Y + 1; y++)
                {
                    Cell check = GetCell(x, y);

                    if (check != null && !check.IsBomb)
                    {
                        check.NextToBomb();
                    }
                }
            }
        }

        public void ShowEmptyAround(Cell cell)
        {
            if (cell.IsBomb) return;

            for (int x = cell.X - 1; x <= cell.X + 1; x++)
            {
                for (int y = cell.Y - 1; y <= cell.Y + 1; y++)
                {
                    Cell check = GetCell(x, y);

                    if (check != null && check.GetShowState().Clickable && !check.IsBomb)
                    {
                        if (check.GetState() == Cell.State.BOMBS[0])
                        {
                            check.Show();

                            ShowEmptyAround(check);
                        }
                        else if (!check.GetState().Clickable)
                        {
                            check.Show();
                        }
                    }
                }
            }
        }

        public void ShowEverything()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    GetCell(x, y).Show();
                }
            }
        }

        private Cell CreateCell(int x, int y)
        {
            Cell cell = new Cell(this) { X = x, Y = y };

            cell.InstantiateCell();

            CellGrid.Children.Add(cell.CellImage);

            return cell;
        }

        public Cell GetCell(Point point)
        {
            return GetCell(point.X, point.Y);
        }

        public Cell GetCell(int id)
        {
            int x = id % Width;
            int y = (id - x) / Width;

            return GetCell(x, y);
        }

        public Cell GetCell(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return cells[x, y];
            }

            return null;
        }
        
        public bool GetCellRelativeTo(int x, int y, out Cell producedCell)
        {
            Cell cell = GetCell(x, y);

            if (cell != null && cell.IsClickable())
            {
                producedCell = cell;

                return true;
            }

            producedCell = null;

            return false;
        }

        public bool CanMoveByCount(Action action, int tryCount)
        {
            int tryCountX = 0;
            int tryCountY = 0;

            switch (action.Key)
            {
                case Key.RIGHT:
                    tryCountX = GetMaxTryCount(tryCount, Width - CursorPos.X);
                    tryCountY = 0;
                    
                    break;
            }

            Cell cell = null;

            GetCellRelativeTo(tryCountX, tryCountY, out cell);
            
            return cell != null;
        }

        private int GetMaxTryCount(int tryCount, int maxCount)
        {
            return tryCount <= maxCount ? tryCount : maxCount;
        }

        public void MoveCursor(VotingAction key)
        {
            Cell cell = null;

            switch (key.Action.Key)
            {
                case Key.NEXT:
                    int refX = CursorPos.X;
                    int refY = CursorPos.Y;

                    for (int y = refY; y < Height + refY; y++)
                    {
                        for (int x = refX + 1; x < Width; x++)
                        {
                            int yy = y >= Height ? y - Height : y;

                            cell = GetCell(x, yy);

                            if (cell.IsClickable()) goto Finished;
                            else cell = null;
                        }

                        refX = -1;
                    }

                    Finished: break;
                case Key.UP:
                    for (int i = CursorPos.Y - 1; i >= 0; i++)
                        if (GetCellRelativeTo(CursorPos.X, i, out cell)) break;

                    break;
                case Key.DOWN:
                    for (int i = CursorPos.Y + 1; i < Height; i++)
                        if (GetCellRelativeTo(CursorPos.X, i, out cell)) break;

                    break;
                case Key.LEFT:
                    for (int i = CursorPos.X - 1; i >= 0; i--)
                        if (GetCellRelativeTo(i, CursorPos.Y, out cell)) break;

                    break;
                case Key.RIGHT:

                    GetCellRelativeTo(key.Count, CursorPos.Y, out cell);

                    /*
                    for (int i = CursorPos.X + 1; i < Width; i++)
                        if (GetCellRelativeTo(i, CursorPos.Y, out cell)) break;
                        */
                    break;
                case Key.UP_LEFT:
                    for (int xy = 1; xy < Math.Min(CursorPos.X + 1, CursorPos.Y + 1); xy++)
                        if (GetCellRelativeTo(CursorPos.X - xy, CursorPos.Y - xy, out cell)) break;

                    break;
                case Key.UP_RIGHT:
                    for (int xy = 1; xy < Math.Min(Width - CursorPos.X, CursorPos.Y + 1); xy++)
                        if (GetCellRelativeTo(CursorPos.X + xy, CursorPos.Y - xy, out cell)) break;

                    break;
                case Key.DOWN_LEFT:
                    for (int xy = 1; xy < Math.Min(CursorPos.X + 1, Height - CursorPos.Y); xy++)
                        if (GetCellRelativeTo(CursorPos.X - xy, CursorPos.Y + xy, out cell)) break;

                    break;
                case Key.DOWN_RIGHT:
                    for (int xy = 1; xy < Math.Min(Width - CursorPos.X, Height - CursorPos.Y); xy++)
                        if (GetCellRelativeTo(CursorPos.X + xy, CursorPos.Y + xy, out cell)) break;

                    break;
            }

            if (cell != null)
            {
                GetCell(CursorPos).Hover(false);

                CursorPos = new Point(cell.X, cell.Y);

                cell.Hover(true);
            }
        }

        public void ClickCell()
        {
            Cell cell = GetCell(CursorPos);

            if (cell != null)
            {
                if (cell.Click())
                {
                    //Move to next available cell

                    MoveCursor(new VotingAction() { Action = Action.NEXT, Count = -1 });
                }
            }
        }

        public void MarkCell(bool flag, bool normal)
        {
            Cell cell = GetCell(CursorPos);

            if (cell != null)
            {
                cell.Mark(flag, normal);
            }
        }
    }
}
