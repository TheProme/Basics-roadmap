using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimplePaint.Controls
{
    public class NumericUpDown : Control
    {
        protected const string PART_NumericView = nameof(PART_NumericView);
        private TextBox _numericView = null;

        protected const string PART_incrementButton = nameof(PART_incrementButton);
        private RepeatButton _incrementButton = null;

        protected const string PART_decrementButton = nameof(PART_decrementButton);
        private RepeatButton _decrementButton = null;


        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ApplyTemplateEvents();
        }

        private void ApplyTemplateEvents()
        {
            SetNumericViewEvents(GetTemplateChild(PART_NumericView) as TextBox);
            SetIncreaseEvents(GetTemplateChild(PART_incrementButton) as RepeatButton);
            SetDecrementEvents(GetTemplateChild(PART_decrementButton) as RepeatButton);
        }

        private void SetNumericViewEvents(TextBox textBox)
        {
            if (_numericView != null)
            {
                _numericView.PreviewTextInput -= TextInputHandler;
                DataObject.RemovePastingHandler(_numericView, PastingHandler);
                _numericView.PreviewMouseWheel -= PreviewMouseWheelHandler;
            }
            _numericView = textBox;
            if (_numericView != null)
            {
                _numericView.PreviewTextInput += TextInputHandler;
                DataObject.AddPastingHandler(_numericView, PastingHandler);
                _numericView.PreviewMouseWheel += PreviewMouseWheelHandler;
            }
        }
        private void SetIncreaseEvents(RepeatButton repeatButton)
        {
            if (_incrementButton != null)
            {
                _incrementButton.Click -= IncrementButton_Click;
                _incrementButton.KeyDown -= IncrementButtonKeyHandler;
            }
            _incrementButton = repeatButton;
            if (_incrementButton != null)
            {
                _incrementButton.Click += IncrementButton_Click;
                _incrementButton.KeyDown += IncrementButtonKeyHandler;
            }
        }

        private void SetDecrementEvents(RepeatButton repeatButton)
        {
            if (_decrementButton != null)
            {
                _decrementButton.Click -= DecrementButton_Click;
                _decrementButton.KeyDown -= DecrementButtonKeyHandler;
            }
            _decrementButton = repeatButton;
            if (_decrementButton != null)
            {
                _decrementButton.Click += DecrementButton_Click;
                _decrementButton.KeyDown += DecrementButtonKeyHandler;
            }
        }

        public double NumericValue
        {
            get { return (double)GetValue(NumericValueProperty); }
            set { SetValue(NumericValueProperty, value); }
        }

        public static readonly DependencyProperty NumericValueProperty =
            DependencyProperty.Register("NumericValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(0d, NumericValueChanged, NumericCoerceValue), new ValidateValueCallback(IsTextAllowed));

        private static bool IsTextAllowed(object value)
        {
            double res = 0d;
            return Double.TryParse(value.ToString(), out res);
        }
        private static void NumericValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumericUpDown).SetValue(NumericValueProperty, (double)e.NewValue);
        }

        private static object NumericCoerceValue(DependencyObject d, object baseValue)
        {
            NumericUpDown control = (NumericUpDown)d;
            if (((double)baseValue) < control.MinValue)
            {
                return control.MinValue;
            }
            if (((double)baseValue) > control.MaxValue)
            {
                return control.MaxValue;
            }
            return baseValue;
        }

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(int), typeof(NumericUpDown), new PropertyMetadata(100));

        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(NumericUpDown), new PropertyMetadata(100));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(Double.MaxValue, MaxValueChanged, MaxCoerceValue));

        private static void MaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumericUpDown).SetValue(MaxValueProperty, (double)e.NewValue);
        }

        private static object MaxCoerceValue(DependencyObject d, object baseValue)
        {
            NumericUpDown control = (NumericUpDown)d;
            if (((double)baseValue) < control.MinValue)
            {
                return control.MinValue;
            }
            return baseValue;
        }

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(Double.MinValue, MinValueChanged, MinCoerceValue));

        private static object MinCoerceValue(DependencyObject d, object baseValue)
        {
            NumericUpDown control = (NumericUpDown)d;
            if (((double)baseValue) > control.MaxValue)
            {
                return control.MaxValue;
            }
            return baseValue;
        }
        private static void MinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumericUpDown).SetValue(MinValueProperty, (double)e.NewValue);
        }
        public double Tick
        {
            get { return (double)GetValue(TickProperty); }
            set { SetValue(TickProperty, value); }
        }

        public static readonly DependencyProperty TickProperty =
            DependencyProperty.Register("Tick", typeof(double), typeof(NumericUpDown), new PropertyMetadata(1d));





        private void IncrementButtonKeyHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                NumericValue += Tick;
        }

        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            NumericValue += Tick;
        }

        private void DecrementButtonKeyHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
                NumericValue -= Tick;
        }
        private void DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            NumericValue -= Tick;
        }

        private void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
        private void PreviewMouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            NumericValue = e.Delta > 0 ? NumericValue += Tick : NumericValue -= Tick;
        }

        private void TextInputHandler(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
