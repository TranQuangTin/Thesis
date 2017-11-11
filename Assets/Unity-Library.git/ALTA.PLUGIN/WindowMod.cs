using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class WindowMod : MonoBehaviour
{
    [DllImport("user32.dll")]

    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    [DllImport("user32.dll")]

    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]

    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]

    static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    public Rect screenPosition;


    // not used rigth now  

    //const uint SWP_NOMOVE = 0x2;  

    //const uint SWP_NOSIZE = 1;  

    //const uint SWP_NOZORDER = 0x4;  

    //const uint SWP_HIDEWINDOW = 0x0080;  

    const uint SWP_SHOWWINDOW = 0x0040;

    const int GWL_EXSTYLE = -20;

    const int GWL_STYLE = -16;

    const int WS_BORDER = 1;

    const int WS_EX_LAYERED = 0x80000;

    public const int LWA_ALPHA = 0x2;

    public const int LWA_COLORKEY = 0x1;

    void Start()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR      
        //SetWindowLong(GetForegroundWindow (), GWL_EXSTYLE, WS_EX_LAYERED); 
        SetWindowLong(GetForegroundWindow (), GWL_STYLE, WS_BORDER);
        SetLayeredWindowAttributes(GetForegroundWindow (), 0, 0, LWA_COLORKEY);
         bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, Screen.width, Screen.height, SWP_SHOWWINDOW);
#endif

    }

    public void HideBorder(bool flag=false)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR  
        if (flag)
        {
            SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);
        }
        else
        {
            SetWindowLong(GetForegroundWindow(), GWL_EXSTYLE, WS_BORDER);
        }
         SetLayeredWindowAttributes(GetForegroundWindow (), 0, 0, LWA_COLORKEY);
#endif

    }

    public void HideWindow(bool flag)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR  
        SetWindowLong(GetForegroundWindow(), GWL_EXSTYLE, WS_EX_LAYERED);
        SetLayeredWindowAttributes(GetForegroundWindow (), 0, 0, LWA_COLORKEY);
#endif
    }

    public void SetWindowPos(int x, int y)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR  
        screenPosition.x = x;
        screenPosition.y = y;
        bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
#endif
    }
    public void ChangeSize(int width,int height)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR  
        screenPosition.width = width;
        screenPosition.height = height;
        bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
#endif
    }
}
