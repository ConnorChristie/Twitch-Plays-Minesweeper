using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Twitch_Plays_Minesweeper.Game;
using Twitch_Plays_Minesweeper_WPF;

namespace Twitch_Plays_Minesweeper
{
    class Cell
    {
        public System.Windows.Controls.Image CellImage { get; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public int NextTo { get; private set; }
        public bool IsBomb { get; set; }

        private bool IsHovering = false;

        private Board board;

        private State showState = State.NORMAL;

        private State state = State.BOMBS[0];
        private State prevState = State.NORMAL;

        public Cell(Board board, int x, int y)
        {
            this.board = board;

            X = x;
            Y = y;

            int id = x + y * board.Width;

            CellImage = new System.Windows.Controls.Image();

            CellImage.Name = "pictureBox" + id;
            CellImage.Source = State.NORMAL.GetImage();

            CellImage.HorizontalAlignment = HorizontalAlignment.Left;
            CellImage.VerticalAlignment = VerticalAlignment.Top;

            CellImage.Width = board.CellSize;
            CellImage.Height = board.CellSize;

            CellImage.Margin = new Thickness(x * board.CellSize + board.OffsetX, y * board.CellSize + board.OffsetY, 0, 0);
        }

        public bool Click()
        {
            if (IsClickable())
            {
                if (IsBomb)
                {
                    //SetState(State.BOMB);

                    board.ShowEverything();

                    MainWindow.GetInstance().Twitch.GameOver();
                }
                else if (state == State.BOMBS[0])
                {
                    board.ShowEmptyAround(this);

                    return true;
                }
                else
                {
                    Show();

                    return true;
                }
            }

            return false;
        }

        public void Mark(bool flag, bool normal)
        {
            if (!normal)
            {
                if (flag)
                {
                    SetState(State.FLAG);
                }
                else
                {
                    SetState(State.QUESTION);
                }
            }
            else
            {
                SetState(State.NORMAL);
            }
        }

        public void SetState(State state)
        {
            prevState = showState;
            showState = state;

            if (IsHovering)
            {
                CellImage.Source = state.GetHoverImage();
            } else
            {
                CellImage.Source = state.GetImage();
            }
        }

        public void Show()
        {
            IsHovering = false;

            if (!IsBomb)
            {
                SetState(State.BOMBS[NextTo]);
            } else
            {
                //Show bomb

                SetState(State.BOMB);
            }
        }

        public void NextToBomb()
        {
            NextTo++;
            
            state = State.BOMBS[NextTo];
        }

        public void Hover(bool hovering)
        {
            IsHovering = hovering;

            SetState(showState);
        }

        public bool IsClickable()
        {
            return showState.Clickable;
        }
        
        public State GetState()
        {
            return state;
        }

        public State GetShowState()
        {
            return showState;
        }

        public static State States = new State();

        public class State
        {
            public static State NORMAL = new State { ImageSrc = "Tile_Original", HoverImageSrc = "Tile_Hover", Clickable = true };
            public static State FLAG = new State { ImageSrc = "Tile_Flag", HoverImageSrc = "Tile_Flag_Hover", Clickable = true };
            public static State QUESTION = new State { ImageSrc = "Tile_Question", HoverImageSrc = "Tile_Question_Hover", Clickable = true };

            public static State BOMB = new State { ImageSrc = "Tile_Bomb", Clickable = false };

            public static State[] BOMBS = new State[]
            {
                new State { ImageSrc = "Tile_Empty", Clickable = false },
                new State { ImageSrc = "Tile_1", Clickable = false },
                new State { ImageSrc = "Tile_2", Clickable = false },
                new State { ImageSrc = "Tile_3", Clickable = false },
                new State { ImageSrc = "Tile_4", Clickable = false },
                new State { ImageSrc = "Tile_5", Clickable = false },
                new State { ImageSrc = "Tile_6", Clickable = false },
                new State { ImageSrc = "Tile_7", Clickable = false },
                new State { ImageSrc = "Tile_8", Clickable = false }
            };
            
            private string ImageSrc { get; set; }
            private string HoverImageSrc { get; set; }

            public bool Clickable { get; private set; }

            public ImageSource GetImage()
            {
                //pack://siteoforigin:,,,/Resources/Tile_Original.png

                return new BitmapImage(new Uri("pack://application:,,,/Resources/" + ImageSrc + ".png"));
            }

            public ImageSource GetHoverImage()
            {
                return new BitmapImage(new Uri("pack://application:,,,/Resources/" + HoverImageSrc + ".png"));
            }
        }
    }
}
