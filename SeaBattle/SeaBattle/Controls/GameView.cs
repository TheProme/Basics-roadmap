using SeaBattle.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class GameView : Control
    {
        static GameView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GameView), new FrameworkPropertyMetadata(typeof(GameView)));
        }


        public ObservableCollection<PlayerViewModel> Players
        {
            get { return (ObservableCollection<PlayerViewModel>)GetValue(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }

        public static readonly DependencyProperty PlayersProperty =
            DependencyProperty.Register("Players", typeof(ObservableCollection<PlayerViewModel>), typeof(GameView), new PropertyMetadata(null));


        public bool IsGameOver
        {
            get { return (bool)GetValue(IsGameOverProperty); }
            set { SetValue(IsGameOverProperty, value); }
        }

        public static readonly DependencyProperty IsGameOverProperty =
            DependencyProperty.Register("IsGameOver", typeof(bool), typeof(GameView), new PropertyMetadata(false));



    }
}
