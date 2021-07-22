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

namespace CustomControls
{
    public class CardView : Control
    {
        static CardView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardView), new FrameworkPropertyMetadata(typeof(CardView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EditButton = GetTemplateChild(PART_EditButton) as Button;
        }

        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(CardView), new PropertyMetadata(false));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CardView));


        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(CardView), new PropertyMetadata(null));


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(CardView), new PropertyMetadata(String.Empty));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CardView), new PropertyMetadata(String.Empty));

        protected const string PART_EditButton = nameof(PART_EditButton);
        private Button _editButton = null;
        private Button EditButton
        {
            get
            {
                return _editButton;
            }
            set
            {
                if (_editButton != null)
                {
                    _editButton.Click -= EditButton_Click;
                }
                _editButton = value;
                if (_editButton != null)
                {
                    _editButton.Click += EditButton_Click;
                }
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            IsEditing = !IsEditing;
        }

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }
    }
}
