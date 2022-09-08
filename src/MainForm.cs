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
                HideInTray();
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HideInTray();
                return;
            }
            if (WindowState == FormWindowState.Minimized)
            {
                ShowFromTray();
                return;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _stateMachine.Start();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _stateMachine.Restart();
            }
        }
     
        public void ShowFromTray()
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            TopMost = true;
            Focus();
        }

        public void HideInTray()
        {
            WindowState = FormWindowState.Minimized;
            Hide();
        }
    }
}
