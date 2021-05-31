using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaPerimeterCalculator.Figures
{
    public enum FigureNames
    {
        Triangle,
        Rectangle,
        Ellipse,
        Trapezoid
    }
    public abstract class BaseFigure: IFigureCalculations
    {
        public FigureNames Name { get; protected set; }
        protected List<double> Sides;

        public virtual double GetPerimeter()
        {
            double perimeter = default;
            foreach (var side in Sides)
            {
                perimeter += side;
            }
            return perimeter;
        }

        public abstract double GetArea();
    }
}
