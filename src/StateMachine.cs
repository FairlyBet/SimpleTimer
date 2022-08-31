using System.Media;


namespace SimpleTimer
{
    internal sealed class StateMachine
    {
        private State _currentState;


        public StateMachine(StateMachineContext context)
        {
            _currentState = new InitialState(SwitchState, context);
        }

        public void Start()
        {
            _currentState.Activate();
        }

        private void SwitchState(State nextState)
        {
            _currentState = nextState;
            _currentState.Activate();
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

        public abstract void Activate();
    }


    internal class InitialState : State
    {
        public InitialState(Action<State> switchState, StateMachineContext context) : base(switchState, context) { }

        public override void Activate()
        {
            _context.TimeLabel.Text = TimeSpan.FromSeconds(TimerFactory.TimerForWork().IntervalSeconds).ToString("mm\\:ss");
            _context.ControlButton.Text = "Start";
            _context.ControlButton.Click += SwitchState;
        }

        private void SwitchState(object sender, EventArgs e)
        {
            _context.ControlButton.Click -= SwitchState;
            _switchState(new WorkingIntervalElapsingState(_switchState, _context));
        }
    }


    internal class WorkingIntervalElapsingState : State
    {
        public WorkingIntervalElapsingState(Action<State> switchState, StateMachineContext context) : base(switchState, context) { }

        public override void Activate()
        {
            var timer = TimerFactory.TimerForWork();
            timer.OnSecondElapsed += UpdateLayout;
            timer.OnIntervalElapsed += SwitchState;
            _context.TimeLabel.Text = TimeSpan.FromSeconds(timer.IntervalSeconds).ToString("mm\\:ss");
            _context.ControlButton.Enabled = false;
            timer.Start();
        }

        private void UpdateLayout(AsyncSecondBasedTimer timer)
        {
            _context.TimeLabel.Text = TimeSpan.FromSeconds(timer.IntervalSeconds - timer.Elapsed).ToString("mm\\:ss");
        }

        private void SwitchState()
        {
            _switchState(new IntervalElapsedState(_switchState, _context, IntervalElapsedState.CreatedFrom.WorkingState));
        }
    }


    internal class RestingIntervalElapsingState : State
    {
        public RestingIntervalElapsingState(Action<State> switchState, StateMachineContext context) : base(switchState, context) { }

        public override void Activate()
        {
            var timer = TimerFactory.TimerForRest();
            timer.OnSecondElapsed += UpdateLayout;
            timer.OnIntervalElapsed += SwitchState;
            _context.TimeLabel.Text = TimeSpan.FromSeconds(timer.IntervalSeconds).ToString("mm\\:ss");
            _context.ControlButton.Enabled = false;
            timer.Start();
        }

        private void UpdateLayout(AsyncSecondBasedTimer timer)
        {
            _context.TimeLabel.Text = TimeSpan.FromSeconds(timer.IntervalSeconds - timer.Elapsed).ToString("mm\\:ss");
        }

        private void SwitchState()
        {
            _switchState(new IntervalElapsedState(_switchState, _context, IntervalElapsedState.CreatedFrom.RestingState));
        }
    }


    internal class IntervalElapsedState : State
    {
        public enum CreatedFrom
        {
            WorkingState,
            RestingState
        }


        private readonly CreatedFrom _from;
        private readonly SoundPlayer _player;


        public IntervalElapsedState(Action<State> switchState, StateMachineContext context, CreatedFrom from) : base(switchState, context)
        {
            _from = from;
            _player = new SoundPlayer(Audio.alarm);
        }

        public override void Activate()
        {
            _context.ControlButton.Click += SwitchState;
            _context.ControlButton.Enabled = true;
            _context.ControlButton.Text = "Continue";
            _context.Form.ShowFromTray();
            _context.Form.BringToFront();
            _context.Form.TopMost = true;
            _context.Form.Focus();
            _player.PlayLooping();
        }

        private void SwitchState(object sender, EventArgs e)
        {
            _player.Stop();
            _player.Dispose();
            _context.ControlButton.Click -= SwitchState;
            if (_from == CreatedFrom.WorkingState)
            {
                _switchState(new RestingIntervalElapsingState(_switchState, _context));
            }
            if (_from == CreatedFrom.RestingState)
            {
                _switchState(new WorkingIntervalElapsingState(_switchState, _context));
            }
            _context.Form.HideInTray();
        }
    }
}
