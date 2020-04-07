using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    /// <summary>
    /// A stylish treeview like the one from Windows Explorer 
    /// </summary>
    [DesignerCategory("Code")]
    [ToolboxBitmap(typeof(TreeView))]
    public class TreeViewAero : TreeView
    {
        public TreeViewAero()
        {
            // sets double buffering for flickerfree treeview
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            // like the one in Windows Explorer
            base.ShowLines = true;
            base.HideSelection = false;
        }

        /// <summary>
        /// OnPaint-Method, modified for UserPaint-Flag at Style
        /// </summary>
        /// <param name="e">base PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint))
            {
                // create new message
                Message m = new Message();

                // content handle
                m.HWnd = this.Handle;

                // message itself
                m.Msg = 0x0318; // WM_PRINTCLIENT message

                // params
                m.WParam = e.Graphics.GetHdc();
                m.LParam = (IntPtr)4; // PRF_CLIENT message

                // send this message
                DefWndProc(ref m);

                // release hdc
                e.Graphics.ReleaseHdc(m.WParam);
            }

            // do the basics
            base.OnPaint(e);
        }

        /// <summary>
        /// OnHandleCreated-Method
        /// </summary>
        /// <param name="e">base EventArgs</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            // do the basics
            base.OnHandleCreated(e);

            // set the theme of this treeview from "explorer" (application name)
            NativeMethods.SetWindowTheme(base.Handle, "explorer", null);

            if (Environment.OSVersion.Version.Major > 5) // greater than xp
            {
                // send some messages for the stylish effects
                IntPtr lParam = (IntPtr)(NativeMethods.SendMessage(base.Handle, 0x112d, IntPtr.Zero, IntPtr.Zero).ToInt32() | 0x60);
                NativeMethods.SendMessage(base.Handle, 0x112c, IntPtr.Zero, lParam);
            }
        }

        /// <summary>
        /// CreateParams-Property
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                // get base params
                CreateParams createParams = base.CreateParams;

                if (Environment.OSVersion.Version.Major > 5) // greater than xp
                {
                    // a bitwise logical OR operation
                    createParams.Style |= 0x8000;
                }

                // return the modified value
                return createParams;
            }
        }
    }

    public abstract class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);
    }
}
