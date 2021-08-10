using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris.Models;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameViewModel _gameVM;
        public MainWindow()
        {
            _gameVM = new GameViewModel(new Game(), new GameTimer());
            DataContext = _gameVM;
            InitializeComponent();
            tetrisField.Children.Add(_gameVM.Game.GamingGrid);
            previewField.Children.Add(_gameVM.Game.PreviewGrid);
        }
    }
}
