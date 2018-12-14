using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Dfust.Hotkeys;
using MessageBox = System.Windows.MessageBox;

namespace HushNow
{
    public partial class MainWindow : IDisposable
    {
        private Keys _currentHotkey;

        public Keys CurrentHotkey
        {
            get => _currentHotkey;
            set
            {
                SetHotkey(value);
                _currentHotkey = value;
            }
        }

        private readonly CoreAudioController _controller = new CoreAudioController();
        private readonly CoreAudioDevice _micDevice;
        private readonly bool _originalMuteStatus;
        public event Action<bool> OnStatusChanged;

        private readonly HotkeyCollection _hkCollection;

        private readonly Settings _settings;

        public MainWindow(Action<bool> onStatusChanged = null)
        {
            InitializeComponent();

            _micDevice = _controller.GetCaptureDevices(DeviceState.Active)
                .FirstOrDefault(x => x.IsDefaultDevice);

            if (_micDevice == null)
            {
                MessageBox.Show("Error", "No default device found. Exiting..");
                Close();
                return;
            }

            var icon = Properties.Resources.Icon.ToImageSource();
            var iconActive = Properties.Resources.Icon_Active.ToImageSource();

            OnStatusChanged += muted =>
            {
                Icon = muted ? icon : iconActive;
                LblStatus.Content = muted ? "Mic Off" : "Mic On";
            };
            OnStatusChanged += onStatusChanged;

            _originalMuteStatus = _micDevice.IsMuted;
            SetMuted(_originalMuteStatus); // Set for initialization of controls

            _hkCollection = new HotkeyCollection(Enums.Scope.Global);
            _settings = Settings.Read();
            CurrentHotkey = _settings.SavedKey;
        }

        private void MuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            ToggleMute();
        }

        private void ToggleMute()
        {
            var newVal = !_micDevice.IsMuted;
            SetMuted(newVal);
        }

        private void SetMuted(bool muted)
        {
            _micDevice.SetMuteAsync(muted);
            OnStatusChanged?.Invoke(muted);
        }

        private void SetHotkey(Keys newKey)
        {
            _hkCollection.StopListening();
            _hkCollection.UnregisterHotkey(CurrentHotkey);
            _hkCollection.RegisterHotkey(newKey, OnHotkey, Guid.NewGuid().ToString());
            _hkCollection.StartListening();
            _settings.SavedKey = newKey;
            Settings.Write(_settings);
        }

        private void OnHotkey(HotKeyEventArgs args)
        {
            ToggleMute();
        }

        public async void Dispose()
        {
            Settings.Write(_settings);
            await _micDevice.SetMuteAsync(_originalMuteStatus);
            Close();
        }
    }
}