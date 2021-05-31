using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaPerimeterCalculator.Figures
{
    public class Triangle : BaseFigure
    {
        private double SideA { get; set; }
        private double SideB { get; set; }
        private double SideC { get; set; }
        public Triangle()
        {
            Name = FigureNames.Triangle;
        }

        public void SetSidesForTriangle(double side1, double side2, double side3)
        {
            SideA = side1;
            SideB = side2;
            SideC = side3;

            Sides = new List<double> { SideA, SideB, SideC };
        }

        public Triangle(double side1, double side2, double side3): this()
        {
            SetSidesForTriangle(side1, side2, side3);
        }

        public override double GetArea()
        {
            double halfP = this.GetPerimeter() / 2d;
            double halfPDiff = 0;
            foreach (var side in Sides)
            {
                halfPDiff = (halfPDiff == 0) ? halfP - side : halfPDiff *= halfP - side;
            }
            double S = Math.Sqrt(halfP*halfPDiff);
            return S;
        }
    }
}
