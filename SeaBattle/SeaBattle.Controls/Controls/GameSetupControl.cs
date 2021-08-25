using System.Windows;
using System.Windows.Controls;

namespace SeaBattle.Controls
{
    public class GameSetupControl : Control
    {
        static GameSetupControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GameSetupControl), new FrameworkPropertyMetadata(typeof(GameSetupControl)));
        }
    }
}
