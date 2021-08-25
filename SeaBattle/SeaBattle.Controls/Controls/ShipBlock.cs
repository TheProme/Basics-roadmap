using SeaBattle.Extensions;
using SeaBattle.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SeaBattle.Controls
{
    public class ShipBlock : Control
    {
        static ShipBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShipBlock), new FrameworkPropertyMetadata(typeof(ShipBlock)));
        }

        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Position), typeof(ShipBlock), new PropertyMetadata(new Position(0, 0)));




        public bool IsHit
        {
            get { return (bool)GetValue(IsHitProperty); }
            set { SetValue(IsHitProperty, value); }
        }

        public static readonly DependencyProperty IsHitProperty =
            DependencyProperty.Register("IsHit", typeof(bool), typeof(ShipBlock), new PropertyMetadata(false));




        public bool ShipDestroyed
        {
            get { return (bool)GetValue(ShipDestroyedProperty); }
            set { SetValue(ShipDestroyedProperty, value); }
        }

        public static readonly DependencyProperty ShipDestroyedProperty =
            DependencyProperty.Register("ShipDestroyed", typeof(bool), typeof(ShipBlock), new PropertyMetadata(false));




        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetHitEvents(GetTemplateChild(PART_HitButton) as Button);
        }

        protected const string PART_HitButton = nameof(PART_HitButton);
        private Button _hitButton = null;

        private void SetHitEvents(Button button)
        {
            if (_hitButton != null)
            {
                _hitButton.Click -= HitButton_Click;
            }
            _hitButton = button;
            if (_hitButton != null)
            {
                _hitButton.Click += HitButton_Click;
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
