namespace TPM.Logic.Game
{
    public class Difficulty
    {
        public static readonly Difficulty EASY = new Difficulty { Width = 10, Height = 10, Bombs = 10 };
        public static readonly Difficulty MEDIUM = new Difficulty { Width = 50, Height = 35, Bombs = 75 };
        public static readonly Difficulty HARD = new Difficulty { Width = 75, Height = 50, Bombs = 500 };
        
        public static double ScreenHeight { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int Bombs { get; private set; }

        public double GetCellSize()
        {
            return ScreenHeight / Height;
        }
    }
}
