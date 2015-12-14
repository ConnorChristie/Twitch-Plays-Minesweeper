using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Controls;
using TPM.Logic;

namespace TPM.Test
{
    [TestClass]
    public class MinesweeperGenerate
    {
        [TestMethod]
        public void GenerateBoard()
        {
            Mock<Grid> grid = new Mock<Grid>();

            grid.SetupAllProperties();
            
            MainGame game = new MainGame(grid.Object, 200);

            grid.VerifySet(x => x.Width = It.IsAny<double>());
            grid.VerifySet(x => x.Height = It.IsAny<double>());
        }
    }
}
