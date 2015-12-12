using System;
using System.Drawing;
using System.Windows.Controls;

namespace Twitch_Plays_Minesweeper.Game
{
    class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int OffsetX = 1;
        public int OffsetY = 1;

        public double CellSize { get; set; }

        private Cell[,] Cells;
        private Random random = new Random();

        public Grid CellGrid { get; set; }
        private Point CursorPos = new Point(0, 0);

        private int[] bombsX;
        private int[] bombsY;

        public Board(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;

            Cells = new Cell[Width, Height];
        }

        public void CreateBoard()
        {
            CellGrid.Width = Width * CellSize + OffsetX;
            CellGrid.Height = Height * CellSize + OffsetY;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cells[x, y] = CreateCell(x, y);
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

        internal void ShowEmptyAround(Cell cell)
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

        internal void ShowEverything()
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
            Cell cell = new Cell(this, x, y);

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
                return Cells[x, y];
            }

            return null;
        }

        public void MoveCursor(MainGame.Action key)
        {
            Cell cell = null;

            switch (key.Key)
            {
                case MainGame.Key.NEXT:
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
                case MainGame.Key.UP:
                    for (int i = CursorPos.Y - 1; i >= 0; i--)
                    {
                        cell = GetCell(CursorPos.X, i);

                        if (cell.IsClickable()) break;
                        else cell = null;
                    }

                    break;
                case MainGame.Key.DOWN:
                    for (int i = CursorPos.Y + 1; i < Height; i++)
                    {
                        cell = GetCell(CursorPos.X, i);

                        if (cell.IsClickable()) break;
                        else cell = null;
                    }

                    break;
                case MainGame.Key.LEFT:
                    for (int i = CursorPos.X - 1; i >= 0; i--)
                    {
                        cell = GetCell(i, CursorPos.Y);

                        if (cell.IsClickable()) break;
                        else cell = null;
                    }

                    break;
                case MainGame.Key.RIGHT:
                    for (int i = CursorPos.X + 1; i < Width; i++)
                    {
                        cell = GetCell(i, CursorPos.Y);

                        if (cell.IsClickable()) break;
                        else cell = null;
                    }

                    break;
                case MainGame.Key.UP_LEFT:
                    for (int xy = 1; xy < Math.Min(CursorPos.X + 1, CursorPos.Y + 1); xy++)
                    {
                        cell = GetCell(CursorPos.X - xy, CursorPos.Y - xy);

                        if (cell != null)
                        {
                            if (cell.IsClickable()) break;
                            else cell = null;
                        }
                    }

                    break;
                case MainGame.Key.UP_RIGHT:
                    for (int xy = 1; xy < Math.Min(Width - CursorPos.X, CursorPos.Y + 1); xy++)
                    {
                        cell = GetCell(CursorPos.X + xy, CursorPos.Y - xy);

                        if (cell != null)
                        {
                            if (cell.IsClickable()) break;
                            else cell = null;
                        }
                    }

                    break;
                case MainGame.Key.DOWN_LEFT:
                    for (int xy = 1; xy < Math.Min(CursorPos.X + 1, Height - CursorPos.Y); xy++)
                    {
                        cell = GetCell(CursorPos.X - xy, CursorPos.Y + xy);

                        if (cell != null)
                        {
                            if (cell.IsClickable()) break;
                            else cell = null;
                        }
                    }

                    break;
                case MainGame.Key.DOWN_RIGHT:
                    for (int xy = 1; xy < Math.Min(Width - CursorPos.X, Height - CursorPos.Y); xy++)
                    {
                        cell = GetCell(CursorPos.X + xy, CursorPos.Y + xy);

                        if (cell != null)
                        {
                            if (cell.IsClickable()) break;
                            else cell = null;
                        }
                    }

                    break;
            }
            
            if (cell != null)
            {
                GetCell(CursorPos).Hover(false);
                
                CursorPos.X = cell.X;
                CursorPos.Y = cell.Y;

                cell.Hover(true);
            }
        }

        internal void ClickCell()
        {
            Cell cell = GetCell(CursorPos);

            if (cell != null)
            {
                if (cell.Click())
                {
                    //Move to next available cell

                    MoveCursor(MainGame.Action.NEXT);
                }
            }
        }

        internal void MarkCell(bool flag, bool normal)
        {
            Cell cell = GetCell(CursorPos);

            if (cell != null)
            {
                cell.Mark(flag, normal);
            }
        }
    }
}
