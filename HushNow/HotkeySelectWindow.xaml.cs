using System;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace HushNow
{
    public partial class HotkeySelectWindow
    {
        public HotkeySelectWindow(Keys current, Action<Keys> callback)
        {
            InitializeComponent();

            TxtHotkey.Text = current.ToString().ToUpper();

            TxtHotkey.TextChanged += (s, a) =>
            {
                var i = TxtHotkey.CaretIndex;
                TxtHotkey.Text = TxtHotkey.Text.ToUpper();
                TxtHotkey.CaretIndex = i;
            };

            BtnCancel.Click += (e, a) =>
            {
                callback?.Invoke(current);
                Close();
            };
            BtnSave.Click += (e, a) =>
            {
                if (Enum.TryParse<Keys>(TxtHotkey.Text, true, out var keys))
                {
                    callback?.Invoke(keys);
                    Close();
                }
                else
                    MessageBox.Show("Error", "Invalid key");
            };
        }
    }
}
