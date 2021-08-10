using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Tetris.Extensions;

namespace Tetris.Models
{
    public class PreviewField : Field
    {
        private Tetrimino _tetriminoPreview;
        public Tetrimino TetriminoPreview
        { 
            get => _tetriminoPreview;
            set
            {
                _tetriminoPreview = value;
                SetTetriminoPreview(_tetriminoPreview);
            }
        }

        public Grid PreviewGrid { get; private set; }

        public PreviewField()
        {
            Rows = 2;
            Columns = 4;
            PreviewGrid = BuildField(Rows, Columns);
        }
        private void SetTetriminoPreview(Tetrimino tetrimino)
        {
            PreviewGrid.Children.Clear();
            foreach (var item in tetrimino.Figure)
            {
                PreviewGrid.Children.Add(item.Rectangle);
            }
        }
    }
}
