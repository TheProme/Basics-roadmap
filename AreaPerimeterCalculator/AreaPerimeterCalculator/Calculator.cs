using AreaPerimeterCalculator.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaPerimeterCalculator
{
    public class Calculator
    {
        public List<BaseFigure> ExistingFigures = new List<BaseFigure> { new Triangle(), new Rectangle(), new Trapezoid(), new Ellipse() };

        private Dictionary<int, BaseFigure> FigureNumberPair = new Dictionary<int, BaseFigure>();

        public Calculator()
        {
            int counter = 0;
            foreach (var figure in ExistingFigures)
            {
                FigureNumberPair.Add(++counter, figure);
            }
        }

        public void ShowExistingFigures()
        {
            string tempStr = "Existing figures are:";
            string tempGaps = ReturnGapsForString(tempStr);
            Console.WriteLine(tempStr);
            Console.WriteLine(tempGaps);
            foreach (var figure in ExistingFigures)
            {
                var figureKeyPairItem = FigureNumberPair.FirstOrDefault(item => item.Value == figure);
                Console.WriteLine($"{figureKeyPairItem.Value.Name} , it's number [{figureKeyPairItem.Key}]");
            }
            Console.WriteLine(tempGaps);
        }


        private string ReturnGapsForString(string str)
        {
            StringBuilder stringBuilder = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                stringBuilder.Append('-');
            }
            return stringBuilder.ToString();
        }


        private void CreateTriangle(Triangle triangle)
        {
            Console.WriteLine("Input side A : ");
            double sideA = 0;
            double.TryParse(Console.ReadLine(), out sideA);
            Console.WriteLine("Input side B : ");
            double sideB = 0;
            double.TryParse(Console.ReadLine(), out sideB);
            Console.WriteLine("Input side C : ");
            double sideC = 0;
            double.TryParse(Console.ReadLine(), out sideC);
            triangle.SetSidesForTriangle(sideA, sideB, sideC); 
        }

        private void CreateRectangle(Rectangle rectangle)
        {
            Console.WriteLine("Input side A : ");
            double sideA = 0;
            double.TryParse(Console.ReadLine(), out sideA);
            Console.WriteLine("Input side B : ");
            double sideB = 0;
            double.TryParse(Console.ReadLine(), out sideB);
            rectangle.SetSidesForRectangle(sideA, sideB);
        }

        private void CreateEllipse(Ellipse ellipse)
        {
            Console.WriteLine("Input minor axis : ");
            double minorAxis = 0;
            double.TryParse(Console.ReadLine(), out minorAxis);
            Console.WriteLine("Input major axis : ");
            double majorAxis = 0;
            double.TryParse(Console.ReadLine(), out majorAxis);
            ellipse.SetEllipseAxis(minorAxis, majorAxis);
        }

        private void CreateTrapezoid(Trapezoid trapezoid)
        {
            Console.WriteLine("Input basis A : ");
            double basisA = 0;
            double.TryParse(Console.ReadLine(), out basisA);
            Console.WriteLine("Input basis B : ");
            double basisB = 0;
            double.TryParse(Console.ReadLine(), out basisB);
            Console.WriteLine("Input side C : ");
            double sideC = 0;
            double.TryParse(Console.ReadLine(), out sideC);
            Console.WriteLine("Input side D : ");
            double sideD = 0;
            double.TryParse(Console.ReadLine(), out sideD);
            Console.WriteLine("Input height : ");
            double height = 0;
            double.TryParse(Console.ReadLine(), out height);
            trapezoid.SetSidesForTrapezoid(basisA, basisB, sideC, sideD, height);
        }

        public void ShowFigureData(BaseFigure figure)
        {
            string pickStr = $"|You choosed figure : {figure.Name}|";
            string gaps = ReturnGapsForString(pickStr);
            Console.WriteLine(gaps);
            Console.WriteLine(pickStr);
            Console.WriteLine(gaps);

            Console.WriteLine($"It's perimeter : {figure.GetPerimeter()}");
            Console.WriteLine($"It's area : {figure.GetArea()}");
            Console.WriteLine(gaps);
        }

        public void StartCalculationProgram()
        {
            bool flag = true;
            while(flag)
            {
                ShowExistingFigures();
                int figureKey = 0;
                BaseFigure figure;
                if (int.TryParse(Console.ReadLine(), out figureKey) && FigureNumberPair.TryGetValue(figureKey, out figure))
                {
                    switch (figure.Name)
                    {
                        case FigureNames.Triangle:
                            CreateTriangle(figure as Triangle);
                            break;
                        case FigureNames.Rectangle:
                            CreateRectangle(figure as Rectangle);
                            break;
                        case FigureNames.Ellipse:
                            CreateEllipse(figure as Ellipse);
                            break;
                        case FigureNames.Trapezoid:
                            CreateTrapezoid(figure as Trapezoid);
                            break;
                        default:
                            break;
                    }
                    ShowFigureData(figure);
                }
                else
                {
                    Console.WriteLine("Please, input figure relevant number!");
                }
                Console.WriteLine("To continue press [ENTER], to exit input any character");
                if (!String.IsNullOrEmpty(Console.ReadLine()))
                {
                    flag = false;
                }
                Console.Clear();
            }
        }
    }
}
