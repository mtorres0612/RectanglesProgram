using RectangleApplication;
using Rectangles;

namespace RectangleTests
{
    [TestClass]
    public class RectangleTests
    {
        [TestMethod]
        public void GenerateGrid()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });

            Assert.AreEqual(11, rp.gridCoordinates.XCoordinate);
            Assert.AreEqual(15, rp.gridCoordinates.YCoordinate);
        }

        [TestMethod]
        public void GenerateGridInvalidWidth()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.GenerateGrid(new Coordinate { XCoordinate = 4, YCoordinate = 15 });

            Assert.AreEqual(0, rp.gridCoordinates.XCoordinate);
        }

        [TestMethod]
        public void GenerateGridInvalidHeight()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.GenerateGrid(new Coordinate { XCoordinate = 15, YCoordinate = 26 });

            Assert.AreEqual(0, rp.gridCoordinates.YCoordinate);
        }

        [TestMethod]
        public void PlotPointsSuccess()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.isTest = true;
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });
            rp.PlotPoints("6,0 9,0 6,3 9,3");

            Assert.AreEqual(rp.errorMessage, string.Empty);
            Assert.IsTrue(rp.rectangles.Count > 0);
        }

        [TestMethod]
        public void PlotPointsBeyondRange()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.isTest = true;
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });
            rp.PlotPoints("6,0 9,0 6,3 9,33");

            Assert.AreEqual(rp.errorMessage, "Rectangles must not extend beyond the edge of the grid");
            Assert.IsTrue(rp.rectangles.Count == 0);
        }

        [TestMethod]
        public void PlotPointsInvalidInput()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.isTest = true;
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });
            rp.PlotPoints("6,0 9,0 6,3 -9,2");

            Assert.AreEqual(rp.errorMessage, "Positions on the grid are non-negative integer coordinates starting at 0");
            Assert.IsTrue(rp.rectangles.Count == 0);
        }

        [TestMethod]
        public void PlotPointsOverlap()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.isTest = true;
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });
            rp.PlotPoints("6,0 9,0 6,3 9,3");
            rp.PlotPoints("6,0 9,0 6,2 9,2");

            Assert.AreEqual(rp.errorMessage, "Rectangles must not overlap");
            Assert.IsTrue(rp.rectangles.Count == 1);
        }

        [TestMethod]
        public void FindRectangleByPointWithResult()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.isTest = true;
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });
            rp.PlotPoints("6,0 9,0 6,3 9,3");
            rp.PlotPoints("2,5 7,5 2,6 7,6");
            rp.FindRectangleByPoint(6, 2);


            Assert.IsTrue(rp.errorMessage.Contains("Rectangle Found"));
            Assert.IsTrue(rp.rectangles.Count == 2);
        }

        [TestMethod]
        public void FindRectangleByPointWithoutResult()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.isTest = true;
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });
            rp.PlotPoints("6,0 9,0 6,3 9,3");
            rp.PlotPoints("2,5 7,5 2,6 7,6");
            rp.FindRectangleByPoint(9, 7);

            Assert.AreEqual(rp.errorMessage, "No Rectangle found. Please try again.");
            Assert.IsTrue(rp.rectangles.Count == 2);
        }

        [TestMethod]
        public void RemoveRectangleByPointWithResult()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.isTest = true;
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });
            rp.PlotPoints("6,0 9,0 6,3 9,3");
            rp.PlotPoints("2,5 7,5 2,6 7,6");
            rp.RemoveRectangleByPoint(6, 2);


            Assert.IsTrue(rp.errorMessage.Contains("Rectangle found and removed!"));
            Assert.IsTrue(rp.rectangles.Count == 1);
        }

        [TestMethod]
        public void RemoveRectangleByPointWithoutResult()
        {
            RectangleProgram rp = new RectangleProgram();
            rp.isTest = true;
            rp.GenerateGrid(new Coordinate { XCoordinate = 11, YCoordinate = 15 });
            rp.PlotPoints("6,0 9,0 6,3 9,3");
            rp.PlotPoints("2,5 7,5 2,6 7,6");
            rp.RemoveRectangleByPoint(9, 7);

            Assert.AreEqual(rp.errorMessage, "No Rectangle found. Nothing was removed.");
            Assert.IsTrue(rp.rectangles.Count == 2);
        }
    }
}