using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectangles
{
    public class RectangleProgram
    {
        public List<Rectangle> rectangles = new List<Rectangle>();
        bool willContinue = true;
        public int minXPoint, maxXPoint, minYPoint, maxYPoint, xCoordinate = 0, yCoordinate = 0;
        public Coordinate gridCoordinates = new Coordinate();
        public string errorMessage = string.Empty;
        public bool isTest = false;
        public void Start()
        {
            Console.WriteLine("Welcome to Rectangle App! Please select from the options below and type the corresponding number to proceed");

            while (willContinue)
            {
                Console.WriteLine("1 = Create Grid, 2 = Place Rectangles, 3 = Find a Rectangle, 4 = Remove a Rectangle, 5 = Exit");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            GetInputs(out xCoordinate, out yCoordinate);
                            GenerateGrid(new Coordinate { XCoordinate = xCoordinate, YCoordinate = yCoordinate});
                            rectangles = new List<Rectangle>();
                            break;
                        case 2:
                            Console.Clear();
                            GenerateGrid(gridCoordinates);
                            DrawRectangles();
                            Console.SetCursorPosition(0, yCoordinate + 5);

                            Console.WriteLine("Enter X,Y coordinates, separated by spaces (e.g, 6,0 9,0 6,3 9,3 or 2,5 7,5 2,6 7,6)");
                            string inputCoordinates = Console.ReadLine();
                            PlotPoints(inputCoordinates);
                            DrawRectangles();

                            Console.SetCursorPosition(0, yCoordinate + 5);

                            break;
                        case 3:
                            Console.Clear();
                            GenerateGrid(gridCoordinates);
                            DrawRectangles();
                            Console.SetCursorPosition(0, yCoordinate + 5);
                            Console.WriteLine("Enter X Coordinate");
                            var xSearch = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter Y Coordinate");
                            var ySearch = Convert.ToInt32(Console.ReadLine());
                            FindRectangleByPoint(xSearch, ySearch);
                            break;
                        case 4:
                            Console.WriteLine("Enter X Coordinate");
                            var xDelete = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter Y Coordinate");
                            var yDelete = Convert.ToInt32(Console.ReadLine());
                            RemoveRectangleByPoint(xDelete, yDelete);
                            Console.Clear();
                            GenerateGrid(gridCoordinates);
                            DrawRectangles();
                            Console.SetCursorPosition(0, yCoordinate + 5);
                            break;
                        case 5:
                            willContinue = false;
                            break;
                        default:
                            Console.WriteLine("Please enter a valid choice.");
                            willContinue = true;
                            break;
                    }
                }
            }

            Console.SetCursorPosition(0, 30);
        }

        public void GenerateGrid(Coordinate coordinate)
        {
            if ((coordinate.XCoordinate >= 5 && coordinate.XCoordinate <= 25) && (coordinate.YCoordinate >= 5 && coordinate.YCoordinate <= 25))
            {
                Console.Write(" ");

                for (int i = 0; i < coordinate.XCoordinate + 1; i++)
                {
                    Console.Write(i.ToString() + "");
                }
                Console.WriteLine();
                for (int i = 0; i < coordinate.YCoordinate + 1; i++)
                {
                    Console.Write(i.ToString() + "\n");
                }

                gridCoordinates = new Coordinate
                {
                    XCoordinate = coordinate.XCoordinate,
                    YCoordinate = coordinate.YCoordinate
                };
            }
            else
            {
                errorMessage = "A grid must have a width and height of no less than 5 and no greater than 25";
                Console.WriteLine(errorMessage);
            }
        }

        public void PlotPoints(string points)
        {
            List<int> validPoints = new List<int>();
            List<string> splitted = points.Split(" ").ToList();
            List<Coordinate> coordinates = new List<Coordinate>();
            int result = 0;
            bool isSuccess = false;
            bool hasInvalidInput = false;

            foreach (var item in splitted)
            {
                int ctr = 0;
                var coordinate = new Coordinate();
                foreach (var commaSplittedItem in item.Split(","))
                {
                    isSuccess = int.TryParse(commaSplittedItem, out result);

                    if (isSuccess)
                    {
                        if (result < 0)
                        {
                            hasInvalidInput = true;
                            break;
                        }
                        else
                        {
                            if (ctr == 0)
                            {
                                coordinate.XCoordinate = result;
                            }
                            else
                            {
                                coordinate.YCoordinate = result;

                            }
                        }
                    }

                    ctr++;
                }
                if (hasInvalidInput)
                    break;
                coordinates.Add(coordinate);
            }

            if (hasInvalidInput)
            {
                if(!isTest)
                    Console.SetCursorPosition(0, yCoordinate + 7);
                errorMessage = "Positions on the grid are non-negative integer coordinates starting at 0";
                Console.WriteLine(errorMessage);
            }
            else
            {
                bool isValid = true;

                if (rectangles.Count == 0)
                {
                    foreach (var plotPoints in coordinates)
                    {
                        if (plotPoints.XCoordinate > gridCoordinates.XCoordinate || plotPoints.YCoordinate > gridCoordinates.YCoordinate)
                        {
                            if (!isTest)
                                Console.SetCursorPosition(0, yCoordinate + 7);
                            errorMessage = "Rectangles must not extend beyond the edge of the grid";
                            Console.WriteLine(errorMessage);
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        rectangles.Add(new Rectangle { Coordinates = coordinates });

                        foreach (var plotPoints in coordinates)
                        {
                            if (!isTest)
                                Console.SetCursorPosition(plotPoints.XCoordinate + 1, plotPoints.YCoordinate + 1);
                            Console.WriteLine("o");
                        }
                    }
                }
                else
                {
                    Rectangle rectangle;
                    foreach (var plotPoints in coordinates)
                    {
                        rectangle = GetRectangleDetails(plotPoints.XCoordinate, plotPoints.YCoordinate);

                        if (rectangle != null)
                        {
                            if (!isTest)
                                Console.SetCursorPosition(0, yCoordinate + 7);
                            errorMessage = "Rectangles must not overlap";
                            Console.WriteLine(errorMessage);
                            isValid = false;
                            break;
                        }
                        else if (plotPoints.XCoordinate > gridCoordinates.XCoordinate || plotPoints.YCoordinate > gridCoordinates.YCoordinate)
                        {
                            if (!isTest)
                                Console.SetCursorPosition(0, yCoordinate + 7);
                            errorMessage = "Rectangles must not extend beyond the edge of the grid";
                            Console.WriteLine(errorMessage);
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        rectangles.Add(new Rectangle { Coordinates = coordinates });

                        foreach (var plotPoints in coordinates)
                        {
                            if (!isTest)
                                Console.SetCursorPosition(plotPoints.XCoordinate + 1, plotPoints.YCoordinate + 1);
                            Console.WriteLine("o");
                        }
                    }

                }

            }
        }

        public void DrawRectangles()
        {
            foreach (var rectangle in rectangles)
            {
                foreach (var coordinates in rectangle.Coordinates)
                {
                    if (!isTest)
                        Console.SetCursorPosition(coordinates.XCoordinate + 1, coordinates.YCoordinate + 1);
                    Console.WriteLine("o");
                }
            }
        }

        public Rectangle GetRectangleDetails(int xCoordinates, int yCoordinates)
        {
            Rectangle foundRectangle = null;

            foreach (var rectangle in rectangles)
            {
                minXPoint = rectangle.Coordinates.Select(c => c.XCoordinate).Min();
                maxXPoint = rectangle.Coordinates.Select(c => c.XCoordinate).Max();

                minYPoint = rectangle.Coordinates.Select(c => c.YCoordinate).Min();
                maxYPoint = rectangle.Coordinates.Select(c => c.YCoordinate).Max();

                if ((xCoordinates >= minXPoint && xCoordinates <= maxXPoint) && (yCoordinates >= minYPoint && yCoordinates <= maxYPoint))
                {
                    foundRectangle = rectangle;
                    break;
                }
            }

            return foundRectangle;
        }

        public void FindRectangleByPoint(int xCoordinates, int yCoordinates)
        {
            Rectangle rectangle;
            string output = string.Empty;

            rectangle = GetRectangleDetails(xCoordinates, yCoordinates);

            if (rectangle != null)
            {
                foreach (var item in rectangle.Coordinates)
                {
                    output += $"{item.XCoordinate},{item.YCoordinate} ";
                }
                if (!isTest)
                    Console.SetCursorPosition(0, yCoordinate + 9);

                errorMessage = $"Rectangle Found! Coordinates: {output}";
                Console.WriteLine(errorMessage);
            }
            else
            {
                if (!isTest)
                    Console.SetCursorPosition(0, yCoordinate + 9);

                errorMessage = "No Rectangle found. Please try again.";
                Console.WriteLine(errorMessage);
            }
        }

        public void RemoveRectangleByPoint(int xCoordinates, int yCoordinates)
        {
            Rectangle rectangle;
            rectangle = GetRectangleDetails(xCoordinates, yCoordinates);
            string output = string.Empty;

            if (rectangle != null)
            {
                rectangles.Remove(rectangle);
                foreach (var item in rectangle.Coordinates)
                {
                    output += $"{item.XCoordinate},{item.YCoordinate} ";
                }

                if (!isTest)
                    Console.SetCursorPosition(0, yCoordinate + 9);

                errorMessage = $"Rectangle found and removed! Coordinates of the removed rectangle: {output}";
                Console.WriteLine(errorMessage);
            }
            else
            {
                if (!isTest)
                    Console.SetCursorPosition(0, yCoordinate + 9);

                errorMessage = "No Rectangle found. Nothing was removed.";
                Console.WriteLine(errorMessage);
            }
        }

        public void GetInputs(out int xCoordinate, out int yCoordinate)
        {
            Console.WriteLine("Enter X Coordinate");
            xCoordinate = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Y Coordinate");
            yCoordinate = Convert.ToInt32(Console.ReadLine());
        }


    }

    public class Rectangle
    {
        public List<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
    }

    public class Coordinate
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
    }
}
