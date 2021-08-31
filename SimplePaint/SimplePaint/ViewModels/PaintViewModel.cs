using SimplePaint.Commands;
using SimplePaint.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace SimplePaint.ViewModels
{
    public enum Figures
    {
        None = 0,
        Free,
        Line,
        Rectangle,
        Ellipse,
        Eraser
    }

    public class PaintViewModel : BaseViewModel
    {
        private static readonly double _precision = 0.01;
        private Point _startPoint;
        private List<Point> EraserPath { get; set; } = new List<Point>();
        public StrokeCollection CurrentStrokes { get; set; } = new StrokeCollection();
        public Stroke CurrentStroke { get; set; }
        private bool _erased = false;




        private Figures _selectedFigure = Figures.None;

        public Figures SelectedFigure
        {
            get => _selectedFigure;
            set 
            { 
                _selectedFigure = value;
                OnPropertyChanged();
            }
        }


        private InkCanvasEditingMode _editingMode = InkCanvasEditingMode.None;

        public InkCanvasEditingMode EditingMode
        {
            get => _editingMode;
            set 
            { 
                _editingMode = value;
                OnPropertyChanged();
            }
        }

        private Color _selectedColor = Colors.Black;

        public Color SelectedColor
        {
            get => _selectedColor;
            set 
            { 
                _selectedColor = value; 
                OnPropertyChanged(); 
            }
        }

        private int _thickness = 2;

        public int Thickness
        {
            get => _thickness;
            set 
            { 
                if(value <= 0)
                {
                    value = 1;
                }
                _thickness = value; 
                OnPropertyChanged(); 
            }
        }

        public PaintViewModel()
        {
            CurrentStrokes.StrokesChanged += StrokesChangedHandler;
        }

        private StrokeCollection _removedStrokes = new StrokeCollection();
        private void StrokesChangedHandler(object sender, StrokeCollectionChangedEventArgs e)
        {
            if(e.Removed.Count > 0 && _erased)
            {
                _removedStrokes.Add(e.Removed);
            }
        }

        #region Commands
        private ICommand _undoAction;

        public ICommand UndoAction
        {
            get => _undoAction ?? (_undoAction = new RelayCommand(obj =>
            {
                Undo();
            }, obj => CurrentStrokes.Count > 0));
        }
        private ICommand _redoAction;

        public ICommand RedoAction
        {
            get => _redoAction ?? (_redoAction = new RelayCommand(obj =>
            {
                Redo();
            }, obj => _removedStrokes.Count > 0));
        }

        private ICommand _selectEmptyStyle;

        public ICommand SelectEmptyStyle
        {
            get => _selectEmptyStyle ?? (_selectEmptyStyle = new RelayCommand(obj =>
            {
                SelectedFigure = Figures.None;
            }));
        }

        private ICommand _selectFreeStyle;

        public ICommand SelectFreeStyle
        {
            get => _selectFreeStyle ?? (_selectFreeStyle = new RelayCommand(obj =>
            {
                SelectedFigure = Figures.Free;
            }));
        }

        private ICommand _selectLine;

        public ICommand SelectLine
        {
            get => _selectLine ?? (_selectLine = new RelayCommand(obj =>
            {
                SelectedFigure = Figures.Line;
            }));
        }

        private ICommand _selectRectangle;

        public ICommand SelectRectangle
        {
            get => _selectRectangle ?? (_selectRectangle = new RelayCommand(obj => 
            {
                SelectedFigure = Figures.Rectangle;
            }));
        }

        private ICommand _selectEllipse;

        public ICommand SelectEllipse
        {
            get => _selectEllipse ?? (_selectEllipse = new RelayCommand(obj =>
            {
                SelectedFigure = Figures.Ellipse;
            }));
        }

        private ICommand _selectEraser;

        public ICommand SelectEraser
        {
            get => _selectEraser ?? (_selectEraser = new RelayCommand(obj =>
            {
                SelectedFigure = Figures.Eraser;
            }));
        }



        private ICommand _startDrawFigure;

        public ICommand StartDrawFigure
        {
            get => _startDrawFigure ?? (_startDrawFigure = new RelayCommand(obj =>
            {
                StartDraw(obj as MousePositionToElementEventArgs);
            }));
        }

        private ICommand _continueDrawFigure;

        public ICommand ContinueDrawFigure
        {
            get => _continueDrawFigure ?? (_continueDrawFigure = new RelayCommand(obj =>
            {
                ContinueDraw(SelectedFigure, obj as MousePositionToElementEventArgs);
            }));
        }

        private ICommand _endDrawFigure;

        public ICommand EndDrawFigure
        {
            get => _endDrawFigure ?? (_endDrawFigure = new RelayCommand(obj =>
            {
                EndDraw();
            }));
        }
        #endregion

        private void Undo()
        {
            var last = CurrentStrokes.LastOrDefault();
            CurrentStrokes.Remove(last);
            _removedStrokes.Add(last);
        }

        private void Redo()
        {
            var last = _removedStrokes.LastOrDefault();
            CurrentStrokes.Add(last);
            _removedStrokes.Remove(last);
        }

        private void StartDraw(MousePositionToElementEventArgs e)
        {
            if(SelectedFigure != Figures.None)
            {
                IsStarted = true;
                DrawingAttributes attributes = null;
                if (SelectedFigure == Figures.Eraser)
                {
                    EditingMode = InkCanvasEditingMode.EraseByStroke;
                    attributes = new DrawingAttributes
                    {
                        Color = Colors.White,
                        FitToCurve = false,
                        Height = Thickness,
                        IgnorePressure = true,
                        Width = Thickness,
                        StylusTip = StylusTip.Rectangle
                    };
                }
                else
                {
                    EditingMode = InkCanvasEditingMode.Ink;
                    attributes = new DrawingAttributes
                    {
                        Color = SelectedColor,
                        FitToCurve = false,
                        Height = Thickness,
                        IgnorePressure = true,
                        Width = Thickness,
                        StylusTip = StylusTip.Rectangle
                    };
                }
                _startPoint = e.RelativeToElementPoint;
                CurrentStroke = new Stroke(new StylusPointCollection() { new StylusPoint { X = _startPoint.X, Y = _startPoint.Y } })
                {
                    DrawingAttributes = attributes
                };
            }
            else
            {
                EditingMode = InkCanvasEditingMode.None;
            }
        }

        private bool IsStarted = false;
        private void ContinueDraw(Figures currentFigure, MousePositionToElementEventArgs e)
        {
            if (e.MouseEventArgs.LeftButton == MouseButtonState.Released || !IsStarted)
            {
                return;
            }
            switch (currentFigure)
            {
                case Figures.None:
                    break;
                case Figures.Free:
                    CurrentStrokes.Remove(CurrentStroke);
                    CurrentStroke.StylusPoints.Add(new StylusPoint(e.RelativeToElementPoint.X, e.RelativeToElementPoint.Y));
                    CurrentStrokes.Add(CurrentStroke);
                    break;
                case Figures.Line:
                    CurrentStrokes.Remove(CurrentStroke);
                    CurrentStroke.StylusPoints = new StylusPointCollection(GetLineGeometry(_startPoint, e.RelativeToElementPoint));
                    CurrentStrokes.Add(CurrentStroke);
                    break;
                case Figures.Rectangle:
                    CurrentStrokes.Remove(CurrentStroke);
                    CurrentStroke.StylusPoints = new StylusPointCollection(GetRectangleGeometry(_startPoint, e.RelativeToElementPoint));
                    CurrentStrokes.Add(CurrentStroke);
                    break;
                case Figures.Ellipse:
                    CurrentStrokes.Remove(CurrentStroke);
                    CurrentStroke.StylusPoints = new StylusPointCollection(GetEllipseGeometry(_startPoint, e.RelativeToElementPoint));
                    CurrentStrokes.Add(CurrentStroke);
                    break;
                case Figures.Eraser:
                    EraserPath.Add(e.RelativeToElementPoint);
                    CurrentStrokes.Remove(CurrentStroke);
                    CurrentStroke.StylusPoints.Add(new StylusPoint(e.RelativeToElementPoint.X, e.RelativeToElementPoint.Y));
                    CurrentStrokes.Add(CurrentStroke);
                    break;
                default:
                    break;
            }
        }

        private bool ExistInEraserPath(Stroke stroke)
        {
            return EraserPath.Any(point => stroke.GetGeometry().StrokeContains(new Pen(Brushes.Black, Thickness), point));
        }

        private void EndDraw()
        {
            if(SelectedFigure == Figures.Eraser)
            {
                CurrentStrokes.Remove(CurrentStroke);
                _erased = true;
                StrokeCollection erasedStrokes = new StrokeCollection();
                foreach (var item in CurrentStrokes)
                {
                    if(ExistInEraserPath(item) && !erasedStrokes.Contains(item))
                    {
                        erasedStrokes.Add(item);
                    }
                }
                CurrentStrokes.Remove(erasedStrokes);
                EraserPath.Clear();
                _erased = false;
            }
            else if(!CurrentStrokes.Contains(CurrentStroke) && CurrentStroke != null)
                CurrentStrokes.Add(CurrentStroke);
            CurrentStroke = null;
            _startPoint = default;
            IsStarted = false;
        }

        #region Geometries
        private List<Point> GetEllipseGeometry(Point startPoint, Point endPoint)
        {
            double a = (endPoint.X - startPoint.X) / 2;
            double b = (endPoint.Y - startPoint.Y) / 2;
            List<Point> pointList = new List<Point>();
            for (double radius = 0; radius <= 2 * Math.PI; radius = radius + _precision)
            {
                pointList.Add(new Point((startPoint.X + endPoint.X) / 2 + a * Math.Cos(radius), (startPoint.Y + endPoint.Y) / 2 + b * Math.Sin(radius)));
            }
            return pointList;
        }

        private List<Point> GetRectangleGeometry(Point startPoint, Point endPoint)
        {
            return new List<Point>
            {
                new Point(startPoint.X, startPoint.Y),
                new Point(startPoint.X, endPoint.Y),
                new Point(endPoint.X, endPoint.Y),
                new Point(endPoint.X, startPoint.Y),
                new Point(startPoint.X, startPoint.Y),
            };
        }


        private List<Point> GetLineGeometry(Point startPoint, Point endPoint)
        {
            return new List<Point>
            {
                startPoint,
                endPoint
            };
        }
        #endregion
    }
}
