using System;
using System.Windows.Forms;

namespace HushNow
{
    public static class GlobalMenuActions
    {
        public static void ShowHotkeySelectWindow(Keys current, Action<Keys> callback)
        {
            var window = new HotkeySelectWindow(current, callback);
            window.ShowDialog();
        }
    }
}
