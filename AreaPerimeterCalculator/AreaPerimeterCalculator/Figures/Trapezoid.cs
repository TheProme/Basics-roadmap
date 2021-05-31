using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaPerimeterCalculator.Figures
{
    public class Trapezoid: BaseFigure
    {
        private double BasisA { get; set; }
        private double BasisB { get; set; }
        private double SideC { get; set; }
        private double SideD { get; set; }
        public double Height { get; private set; }
        public Trapezoid()
        {
            Name = FigureNames.Trapezoid;
        }

        public void SetSidesForTrapezoid(double base1, double base2, double side1, double side2, double height)
        {
            BasisA = base1;
            BasisB = base2;
            SideC = side1;
            SideD = side2;
            Sides = new List<double> { BasisA, BasisB, SideC, SideD };
            Height = height;
        }

        public Trapezoid(double base1, double base2, double side1, double side2, double height) : this()
        {
            SetSidesForTrapezoid(base1, base2, side1, side2, height);
        }

        public override double GetArea()
        {
            return ((BasisA + BasisB) / 2) * Height;
        }
    }
}
