namespace SimpleTimer
{
    public partial class MainForm : Form
    {
        private readonly StateMachine _stateMachine;


        public MainForm()
        {            
            InitializeComponent();
            var context = new StateMachineContext(TimeLabel, ControlButton, this);
            _stateMachine = new StateMachine(context);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                NotifyIcon.Visible = true;
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowFromTray();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _stateMachine.Start();
        }

        public void ShowFromTray()
        {
            Show();
            WindowState = FormWindowState.Normal;
            NotifyIcon.Visible = false;
        }
    }
}