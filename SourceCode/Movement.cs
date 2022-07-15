using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AccountManager
{
    public partial class AccountManagerForm
    {
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")] public static extern bool ReleaseCapture();

        public void AccountManagerForm_MouseDown(object sender, MouseEventArgs args)
        {
            MoveHandler(args);
        }

        private void AccountManagerTitle_MouseDown(object sender, MouseEventArgs args)
        {
            MoveHandler(args);
        }

        private void MoveHandler(MouseEventArgs args)
        {
            // Allows you to drag the application by holding down left mouse button
            if (args.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }
    }
}