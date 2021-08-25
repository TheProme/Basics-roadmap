using SeaBattle.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Extensions
{
    public interface IClickableCell
    {
        public void InvokeHit();
        public event Action<IClickableCell> HitEvent;
    }
}
