using System.Media;


namespace SimpleTimer
{
    internal class StateMachine
    {
        private State _currentState;
        private StateMachineContext _context;


        public StateMachine(StateMachineContext context)
        {
            _currentState = new InitialState(SwitchState, context);
            _context = context;
        }

        public void Start()
        {
            _currentState.Set();
        }

        public void Restart()
        {
            _currentState.Reset();
            _currentState = new InitialState(SwitchState, _context);
            Start();
        }

        private void SwitchState(State nextState)
        {
            _currentState = nextState;
            _currentState.Set();
        }
    }


    internal abstract class State
    {
        protected Action<State> _switchState;
        protected StateMachineContext _context;


        public State(Action<State> switchState, StateMachineContext context)
        {
            _switchState = switchState;
            _context = context;
        }

        public abstract void Set();

        public virtual void Reset() { }
    }


    internal class InitialState : State
    {
        public InitialState(Action<State> switchState, StateMachineContext context) : base(switchState, context) { }

        public override void Set()
        {
            _context.TimeLabel.Text = TimeSpan.FromSeconds(TimerFactory.CreateTimerForWork().IntervalSeconds).ToString("mm\\:ss");
            _context.ControlButton.Text = "Start";
            _context.ControlButton.Enabled = true;
            _context.ControlButton.Click += SwitchState;
        }

        private void SwitchState(object sender, EventArgs e)
        {
            Reset();
            _switchState(new IntervalElapsingState(_switchState, _context, new TimerSwitch()));
        }

        public override void Reset()
        {
            _context.ControlButton.Click -= SwitchState;
        }
    }


    internal class IntervalElapsingState : State
    {
        private readonly AsyncSecondDrivenTimer _timer;
        private readonly TimerSwitch _switch;


        public IntervalElapsingState(Action<State> switchState, StateMachineContext context, TimerSwitch @switch) : base(switchState, context)
        {
            _switch = @switch;
            _timer = _switch.AnotherTimer;
        }

        public override void Set()
        {
            _timer.OnSecondElapsed += timer => _context.TimeLabel.Text = TimeSpan.FromSeconds(timer.IntervalSeconds - timer.Elapsed).ToString("mm\\:ss");
            _timer.OnIntervalElapsed += () => _switchState(new IntervalElapsedState(_switchState, _context, _switch));
            _context.TimeLabel.Text = TimeSpan.FromSeconds(_timer.IntervalSeconds).ToString("mm\\:ss");
            _context.ControlButton.Enabled = false;
            _context.Form.HideInTray();

            _timer.Start();
        }

        public override void Reset()
        {
            base.Reset();
            _timer.Stop();
        }
    }


    internal class IntervalElapsedState : State
    {
        private readonly SoundPlayer _player;
        private readonly TimerSwitch _switch;


        public IntervalElapsedState(Action<State> switchState, StateMachineContext context, TimerSwitch @switch) : base(switchState, context)
        {
            _switch = @switch;
            _player = new SoundPlayer(Audio.alarm);
        }

        public override void Set()
        {
            _context.ControlButton.Click += SwitchState;
            _context.ControlButton.Text = "Continue";
            _context.ControlButton.Enabled = true;
            _context.Form.ShowFromTray();
            _player.Play();
        }

        private void SwitchState(object sender, EventArgs e)
        {
            Reset();
            _switchState(new IntervalElapsingState(_switchState, _context, _switch));
        }

        public override void Reset()
        {
            base.Reset();
            _player.Stop();
            _player.Dispose();
            _context.ControlButton.Click -= SwitchState;
        }
    }


    internal struct TimerSwitch
    {
        private bool _switch;

        public AsyncSecondDrivenTimer AnotherTimer
        {
            get
            {
                AsyncSecondDrivenTimer result;
                if (_switch)
                {
                    result = TimerFactory.CreateTimerForWork();
                }
                else
                {
                    result = TimerFactory.CreateTimerForRest();
                }
                _switch = !_switch;
                return result;
            }
        }


        public TimerSwitch()
        {
            _switch = true;
        }
    }
}
