using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaPerimeterCalculator.Figures
{
    public class Ellipse: BaseFigure
    {
        private double SemiMinorAxis { get; set; }
        private double SemiMajorAxis { get; set; }

        public Ellipse()
        {
            Name = FigureNames.Ellipse;
        }

        public void SetEllipseAxis(double minorAxis, double majorAxis)
        {
            SemiMinorAxis = minorAxis;
            SemiMajorAxis = majorAxis;
        }

        public Ellipse(double minorAxis, double majorAxis): this()
        {
            SetEllipseAxis(minorAxis, majorAxis);
        }

        public override double GetPerimeter()
        {
            return 4 * ((GetArea() + (SemiMajorAxis - SemiMinorAxis)) / (SemiMinorAxis + SemiMajorAxis));
        }
        public override double GetArea()
        {
            return Math.PI * SemiMinorAxis * SemiMajorAxis;
        }
    }
}
