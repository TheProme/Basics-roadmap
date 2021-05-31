using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaPerimeterCalculator.Figures
{
    public class Rectangle: BaseFigure
    {
        private double SideA { get; set; }
        private double SideB { get; set; }
        public void SetSidesForRectangle(double side1, double side2)
        {
            SideA = side1;
            SideB = side2;
            Sides = new List<double> { SideA, SideB };
        }
        public Rectangle()
        {
            Name = FigureNames.Rectangle;
        }

        public Rectangle(double side1, double side2) : this()
        {
            SetSidesForRectangle(side1, side2);
        }

        public override double GetPerimeter()
        {
            return base.GetPerimeter() * 2;
        }
        public override double GetArea()
        {
            double S = 0;
            foreach (var side in Sides)
            {
                S = S == 0 ? side : S *= side;
            }
            return S;
        }
    }
}
