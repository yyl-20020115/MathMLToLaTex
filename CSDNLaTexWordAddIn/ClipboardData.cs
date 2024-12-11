using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;


namespace CSDNLaTexWordAddIn
{

    public static class ClipboardData
    {
        public static string GetHtml() => Encoding.UTF8.GetString(GetHtmlBytes());
        public static byte[] GetHtmlBytes() => GetHtml(Clipboard.GetDataObject());

        /**//// <summary>
            /// Extracts data of type Dataformat.Html from an IdataObject data container
            /// This method shouldn't throw any exception but writes relevant exception informations in the debug window
            /// </summary>
            /// <param name="data">IdataObject data container</param>
            /// <returns>A byte[] array with the decoded string or null if the method fails</returns>
            /// 
        public static byte[] GetHtml(System.Windows.Forms.IDataObject data)
        {
            System.Runtime.InteropServices.ComTypes.IDataObject interopData = data as System.Runtime.InteropServices.ComTypes.IDataObject;

            var format = new FORMATETC
            {
                cfFormat = (short)DataFormats.GetFormat(DataFormats.Html).Id,
                dwAspect = DVASPECT.DVASPECT_CONTENT,
                lindex = -1,
                tymed = TYMED.TYMED_HGLOBAL
            };

            var handle = new HandleRef();

            byte[] result = null;

            try
            {
                if (0 == interopData.QueryGetData(ref format))
                {
                    interopData.GetData(ref format, out var stgmedium);

                    if (stgmedium.tymed == TYMED.TYMED_HGLOBAL
                        && stgmedium.unionmember != IntPtr.Zero)
                    {
                        var pointer = GlobalLock(
                            handle = new HandleRef(null, stgmedium.unionmember));
                        if (pointer != IntPtr.Zero)
                        {
                            var length = GlobalSize(handle);

                            Marshal.Copy(pointer, result = new byte[length], 0, length);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                GlobalUnlock(handle);
            }

            return result;
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        // 定义内存分配标志
        const uint GMEM_FIXED = 0x0000;
        const uint GMEM_MOVEABLE = 0x0002;
        const uint GMEM_ZEROINIT = 0x0040;
        const uint GHND = GMEM_MOVEABLE | GMEM_ZEROINIT;
        const uint GPTR = GMEM_FIXED | GMEM_ZEROINIT;


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GlobalLock(HandleRef handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern bool GlobalUnlock(HandleRef handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern int GlobalSize(HandleRef handle);


        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetClipboardData(uint uFormat);

        // 声明OpenClipboard函数
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        // 声明CloseClipboard函数
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseClipboard();

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EmptyClipboard();

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint RegisterClipboardFormatW(string lpszFormat);

        private static uint HTMLFormat = 0;
        public static void SetHTMLFormatTextToClipboard(byte[] buffer)
        {
            if (HTMLFormat == 0)
            {
                HTMLFormat = RegisterClipboardFormatW("HTML Format");  
            }
            // 打开剪贴板
            if (OpenClipboard(IntPtr.Zero))
            {
                if (EmptyClipboard())
                {
                    IntPtr data = Marshal.AllocHGlobal(buffer.Length);

                    Marshal.Copy(buffer, 0, data, buffer.Length);

                    IntPtr result = SetClipboardData(HTMLFormat, data);

                    CloseClipboard();

                    if (result == IntPtr.Zero)
                    {
                    }
                }
                else
                {
                }
            }
            else
            {
            }
        }

        public static byte[] GetHTMLFormatTextToClipboard()
        {
            byte[] buffer = null;
            if (HTMLFormat == 0)
            {
                HTMLFormat = RegisterClipboardFormatW("HTML Format");
            }
            // 打开剪贴板
            if (OpenClipboard(IntPtr.Zero))
            {
                var hCd = new HandleRef(null, GetClipboardData(HTMLFormat));
                if (hCd.Handle != IntPtr.Zero)
                {
                    var hGlobal = new HandleRef(null,GlobalLock(hCd));
                    if (hGlobal.Handle != IntPtr.Zero)
                    {
                        int size = GlobalSize(hGlobal);
                        Marshal.Copy(hGlobal.Handle, buffer = new byte[size], 0, size);
                        GlobalUnlock(hGlobal);
                    }
                }

                CloseClipboard();
            }
            else
            {
            }
            return buffer;
        }


    }
}
