using System.Media;


namespace SimpleTimer
{
    internal class StateMachine
    {
        private State _currentState;


        public StateMachine(StateMachineContext context)
        {
            _currentState = new InitialState(SwitchState, context);
        }

        public void Start()
        {
            _currentState.Set();
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
    }


    internal class InitialState : State
    {
        public InitialState(Action<State> switchState, StateMachineContext context) : base(switchState, context) { }

        public override void Set()
        {
            _context.TimeLabel.Text = TimeSpan.FromSeconds(TimerFactory.CreateTimerForWork().IntervalSeconds).ToString("mm\\:ss");
            _context.ControlButton.Text = "Start";
            _context.ControlButton.Click += SwitchState;
        }

        private void SwitchState(object sender, EventArgs e)
        {
            _context.ControlButton.Click -= SwitchState;
            _switchState(new IntervalElapsingState(_switchState, _context, new TimerSwitch()));
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
            _timer.OnSecondElapsed += UpdateContext;
            _timer.OnIntervalElapsed += SwitchState;
            _context.Form.HideInTray();
            _context.TimeLabel.Text = TimeSpan.FromSeconds(_timer.IntervalSeconds).ToString("mm\\:ss");
            _context.ControlButton.Enabled = false;

            _timer.Start();
        }

        private void UpdateContext(AsyncSecondDrivenTimer timer)
        {
            _context.TimeLabel.Text = TimeSpan.FromSeconds(timer.IntervalSeconds - timer.Elapsed).ToString("mm\\:ss");
        }

        private void SwitchState()
        {
            _switchState(new IntervalElapsedState(_switchState, _context, _switch));
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
            _context.ControlButton.Enabled = true;
            _context.ControlButton.Text = "Continue";
            _context.Form.ShowFromTray();
            _player.Play();
        }

        private void SwitchState(object sender, EventArgs e)
        {
            _player.Stop();
            _player.Dispose();
            _context.ControlButton.Click -= SwitchState;
            _switchState(new IntervalElapsingState(_switchState, _context, _switch));
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
