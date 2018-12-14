using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace HushNow
{
    public partial class App
    {
        private MainWindow _mainWindow;
        private NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow = new MainWindow();
            MainWindow = _mainWindow;
            MainWindow.Closing += MainWindow_Closing;

            _notifyIcon = new NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Icon = HushNow.Properties.Resources.Icon;
            _notifyIcon.Visible = true;

            _mainWindow.OnStatusChanged += muted => _notifyIcon.Icon = muted ? HushNow.Properties.Resources.Icon : HushNow.Properties.Resources.Icon_Active;

            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Show HushNow Window").Click += (s, e) => ShowMainWindow();

            _notifyIcon.ContextMenuStrip.Items.Add("Change Hotkey").Click += (s, e) =>
                GlobalMenuActions.ShowHotkeySelectWindow(_mainWindow.CurrentHotkey, keys => _mainWindow.CurrentHotkey = keys);
                
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;

            _mainWindow.Dispose();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                    MainWindow.WindowState = WindowState.Normal;

                MainWindow.Activate();
            }
            else
                MainWindow.Show();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (_isExit) return;
            
            e.Cancel = true;
            MainWindow.Hide(); // A hidden window can be shown again, a closed one cannot
        }
    }
}