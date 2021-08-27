using SeaBattle.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SeaBattle.Controls
{
    public class EmptyFieldCell : Control
    {
        static EmptyFieldCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmptyFieldCell), new FrameworkPropertyMetadata(typeof(EmptyFieldCell)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetHitEvents(GetTemplateChild(PART_EmptyCellHitButton) as Button);
        }

        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Position), typeof(EmptyFieldCell), new PropertyMetadata(new Position(0, 0)));


        public bool IsHit
        {
            get { return (bool)GetValue(IsHitProperty); }
            set { SetValue(IsHitProperty, value); }
        }

        public static readonly DependencyProperty IsHitProperty =
            DependencyProperty.Register("IsHit", typeof(bool), typeof(EmptyFieldCell), new PropertyMetadata(false));


        protected const string PART_EmptyCellHitButton = nameof(PART_EmptyCellHitButton);
        private Button _emptyCellHitButton = null;

        private void SetHitEvents(Button button)
        {
            if (_emptyCellHitButton != null)
            {
                _emptyCellHitButton.Click -= HitButton_Click;
            }
            _emptyCellHitButton = button;
            if (_emptyCellHitButton != null)
            {
                _emptyCellHitButton.Click += HitButton_Click;
            }
        }

        private void HitButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsHit)
                return;
            IsHit = true;
        }
    }
}
