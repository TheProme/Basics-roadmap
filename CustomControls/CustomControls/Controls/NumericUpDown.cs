using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CustomControls
{
    public class NumericUpDown : Control
    {
        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            NumericView = GetTemplateChild(PART_NumericView) as TextBox;
            RaiseButton = GetTemplateChild(PART_RaiseButton) as RepeatButton;
            DecreaseButton = GetTemplateChild(PART_DecreaseButton) as RepeatButton;
        }
        public int NumericValue
        {
            get { return (int)GetValue(NumericValueProperty); }
            set { SetValue(NumericValueProperty, value); }
        }

        public static readonly DependencyProperty NumericValueProperty =
            DependencyProperty.Register("NumericValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(int), typeof(NumericUpDown), new PropertyMetadata(SystemParameters.KeyboardDelay));

        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(NumericUpDown), new PropertyMetadata(SystemParameters.KeyboardSpeed));




        protected const string PART_RaiseButton = nameof(PART_RaiseButton);
        private RepeatButton _raiseButton = null;
        private RepeatButton RaiseButton
        {
            get
            {
                return _raiseButton;
            }
            set
            {
                if (_raiseButton != null)
                {
                    _raiseButton.Click -= RaiseButton_Click;
                    _raiseButton.KeyDown -= RaiseButtonKeyHandler;
                }
                _raiseButton = value;
                if (_raiseButton != null)
                {
                    _raiseButton.Click += RaiseButton_Click;
                    _raiseButton.KeyDown += RaiseButtonKeyHandler;
                }
            }
        }

        private void RaiseButtonKeyHandler(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up)
                NumericValue++;
        }

        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            NumericValue++;
        }

        protected const string PART_DecreaseButton = nameof(PART_DecreaseButton);
        private RepeatButton _decreaseButton = null;
        private RepeatButton DecreaseButton
        {
            get
            {
                return _decreaseButton;
            }
            set
            {
                if (_decreaseButton != null)
                {
                    _decreaseButton.Click -= DecreaseButton_Click;
                    _decreaseButton.KeyDown -= DecreaseButtonKeyHandler;
                }
                _decreaseButton = value;
                if (_decreaseButton != null)
                {
                    _decreaseButton.Click += DecreaseButton_Click;
                    _decreaseButton.KeyDown += DecreaseButtonKeyHandler;
                }
            }
        }
        private void DecreaseButtonKeyHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
                NumericValue--;
        }
        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            NumericValue--;
        }

        protected const string PART_NumericView = nameof(PART_NumericView);
        private TextBox _numericView = null;
        private TextBox NumericView
        {
            get
            {
                return _numericView;
            }
            set
            {
                if (_numericView != null)
                {
                    _numericView.PreviewTextInput -= TextInputHandler;
                    DataObject.RemovePastingHandler(_numericView, PastingHandler);
                    _numericView.PreviewMouseWheel -= PreviewMouseWheelHandler;
                }
                _numericView = value;
                if (_numericView != null)
                {
                    _numericView.PreviewTextInput += TextInputHandler;
                    DataObject.AddPastingHandler(NumericView, PastingHandler);
                    _numericView.PreviewMouseWheel += PreviewMouseWheelHandler;
                }
            }
        }

        private static bool IsTextAllowed(string text)
        {
            int res = 0;
            return int.TryParse(text, out res);
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
            NumericValue = e.Delta > 0 ? ++NumericValue : --NumericValue;
        }

        private void TextInputHandler(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        

    }
}
