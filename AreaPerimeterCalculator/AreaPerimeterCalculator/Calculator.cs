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

        private Dictionary<BaseFigure, int> FigureNumberPair = new Dictionary<BaseFigure, int>();

        public Calculator()
        {
            int counter = 0;
            foreach (var figure in ExistingFigures)
            {
                FigureNumberPair.Add(figure, ++counter);
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
                int key = 0;
                if(FigureNumberPair.TryGetValue(figure, out key))
                {
                    Console.WriteLine($"{figure.Name} , it's number [{key}]");
                }
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
                var input = Console.ReadLine();
                int figureKey = 0;
                if(int.TryParse(input, out figureKey))
                {
                    BaseFigure figure = FigureNumberPair.FirstOrDefault(element => element.Value == figureKey).Key;
                    if(figure != null)
                    {
                        switch (figure.Name)
                        {
                            case FigureNames.Triangle:
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
                                    (figure as Triangle).SetSidesForTriangle(sideA, sideB, sideC);

                                    ShowFigureData(figure);
                                    break;
                                }
                                
                            case FigureNames.Rectangle:
                                {
                                    Console.WriteLine("Input side A : ");
                                    double sideA = 0;
                                    double.TryParse(Console.ReadLine(), out sideA);
                                    Console.WriteLine("Input side B : ");
                                    double sideB = 0;
                                    double.TryParse(Console.ReadLine(), out sideB);
                                    (figure as Rectangle).SetSidesForRectangle(sideA, sideB);

                                    ShowFigureData(figure);
                                    break;
                                }
                            case FigureNames.Ellipse:
                                {
                                    Console.WriteLine("Input minor axis : ");
                                    double minorAxis = 0;
                                    double.TryParse(Console.ReadLine(), out minorAxis);
                                    Console.WriteLine("Input major axis : ");
                                    double majorAxis = 0;
                                    double.TryParse(Console.ReadLine(), out majorAxis);
                                    (figure as Ellipse).SetEllipseAxis(minorAxis, majorAxis);

                                    ShowFigureData(figure);
                                    break;
                                }
                            case FigureNames.Trapezoid:
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
                                    (figure as Trapezoid).SetSidesForTrapezoid(basisA, basisB, sideC, sideD, height);

                                    ShowFigureData(figure);
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Please, input figure relevant number!");
                }
                Console.WriteLine("To continue press [ENTER], to exit input any character");
                input = Console.ReadLine();
                if(!String.IsNullOrEmpty(input))
                {
                    flag = false;
                }
                else
                {
                    Console.Clear();
                }
            }
        }
    }
}
