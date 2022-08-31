namespace SimpleTimer
{
    internal sealed class StateMachineContext
    {
        public readonly Label TimeLabel;
        public readonly Button ControlButton;
        public readonly MainForm Form;


        public StateMachineContext(Label timeLabel, Button controlButton, MainForm form)
        {
            TimeLabel = timeLabel;
            ControlButton = controlButton;
            Form = form;
        }
    }
}
