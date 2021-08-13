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



        public ShipBlockViewModel ShipBlockModel
        {
            get { return (ShipBlockViewModel)GetValue(ShipBlockModelProperty); }
            set { SetValue(ShipBlockModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShipBlockModelProperty =
            DependencyProperty.Register("ShipBlockModel", typeof(ShipBlockViewModel), typeof(ShipBlock), new PropertyMetadata(null));



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
            if(ShipBlockModel.IsHit)
                return;
            ShipBlockModel.IsHit = true;
        }

    }
}
