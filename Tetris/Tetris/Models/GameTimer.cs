using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tetris.Models
{
    public class GameTimer
    {
        public event Action MoveTickEvent;
        public TimeSpan CurrentTime { get; private set; } = new TimeSpan();
        public Timer Timer { get; private set; }
        public Timer TickTimer { get; private set; }

        private object _locker = new object();

        private int _gameSpeed;

        private int _gamePeriod = 1000;
        private int _minimalGamePeriod = 200;
        private int _tick = 100;

        private int _passedMinutes = 0;

        public GameTimer(int gameSpeed = 1)
        {
            _gameSpeed = gameSpeed;
        }

        public void StartTimer()
        {
            if(Timer != null || TickTimer != null)
            {
                StopTimer();
            }
            Timer = new Timer(TimerCallback, null, 0, 1000);
            TickTimer = new Timer(TickCallback, null, 0, _gamePeriod);
        }

        public void StopTimer()
        {
            SetDefaults();
            Timer.Dispose();
            TickTimer.Dispose();
        }

        private void SetDefaults()
        {
            _gamePeriod = 1000;
            _passedMinutes = 0;
            CurrentTime = new TimeSpan();
        }

        private void TimerCallback(object state)
        {
            UpdateTime();
        }

        private void TickCallback(object state)
        {
            int multiply = _passedMinutes != 0 ? _passedMinutes : 1;
            if (CurrentTime.TotalSeconds > 60 && CurrentTime.TotalSeconds % (60 * multiply) == 0)
            {
                _passedMinutes++;
                if(_gamePeriod > _minimalGamePeriod)
                {
                    _gamePeriod -= _tick * _gameSpeed;
                    TickTimer.Change(0, _gamePeriod);
                }    
            }
            if(CurrentTime.TotalSeconds >= 1)
            {
                MoveTickEvent?.Invoke();
            }
        }

        private void UpdateTime()
        {
            lock(_locker)
            {
                CurrentTime += new TimeSpan(0, 0, 1);
            }
        }
    }
}
