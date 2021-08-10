using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeaBattle.Controls
{
    public class ShipBlock : Control
    {
        static ShipBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShipBlock), new FrameworkPropertyMetadata(typeof(ShipBlock)));
        }

        public event Action BlockHitEvent;
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
            if(IsHit)
                return;
            IsHit = true;
            BlockHitEvent?.Invoke();
        }

        public bool IsHit
        {
            get { return (bool)GetValue(IsHitProperty); }
            set { SetValue(IsHitProperty, value); }
        }

        public static readonly DependencyProperty IsHitProperty =
            DependencyProperty.Register("IsHit", typeof(bool), typeof(ShipBlock), new PropertyMetadata(false));

    }
}
