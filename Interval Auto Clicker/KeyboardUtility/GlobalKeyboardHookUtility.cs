using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Interval_Auto_Clicker.KeyboardUtility
{
    /**
     * Algorithm for using native windows hooks
     * taken from https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application
     */
    internal class GlobalKeyboardHookUtility
    {

        private MainWindow mainWindowInstance;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc? _proc = null;
        private static IntPtr _hookID = IntPtr.Zero;

        // Start hotkey
        private const uint VK_F11 = 0x7A;

        public GlobalKeyboardHookUtility(MainWindow mainWindow)
        {
            mainWindowInstance = mainWindow;
            Debug.WriteLine("Init global keyboard hook utility.");
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule? curModule = curProcess.MainModule)
            {
                if (curModule != null)
                {
                    Debug.WriteLine("Hook set for current process and module.");
                    string? moduleName = curModule.ModuleName;
                    if (moduleName != null)
                    {
                        IntPtr moduleHandle = GetModuleHandle(moduleName);
                        return SetWindowsHookEx(
                            WH_KEYBOARD_LL,
                            proc,
                            moduleHandle,
                            0
                        );
                    }
                    else
                    {
                        return IntPtr.Zero;
                    }
                }
                else
                {
                    return IntPtr.Zero;
                }
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int virtualKeyKeyCode = Marshal.ReadInt32(lParam);
                Debug.WriteLine("Pressed virtual key: " + virtualKeyKeyCode);
                mainWindowInstance.onStartKeyPressed();
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(uint vKey);
    }
}
