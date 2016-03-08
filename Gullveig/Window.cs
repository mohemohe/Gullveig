using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace Gullveig
{
    public class Window : System.Windows.Window
    {
        public NotifyIcon NotifyIcon;
        protected BorderedWindow BorderedWindow { get; private set; }

        #region DependencyProperty

        #region RegisterTaskTrayIcon

        public static readonly DependencyProperty RegisterTaskTrayIconProperty =
        DependencyProperty.Register("RegisterTaskTrayIcon",
                                    typeof(bool),
                                    typeof(Window),
                                    new FrameworkPropertyMetadata(false));

        public bool RegisterTaskTrayIcon
        {
            get { return (bool)GetValue(RegisterTaskTrayIconProperty); }
            set
            {
                SetValue(RegisterTaskTrayIconProperty, value);
            }
        }

        #endregion RegisterTaskTrayIcon

        #endregion DependencyProperty

        public Window()
        {
            Loaded += (sender, args) => StateChanged += EventProxy.InvokeOnStateChanged;
            Closing += (sender, args) =>
            {
                if (RegisterTaskTrayIcon && NotifyIcon != null)
                {
                    args.Cancel = true;
                    base.Hide();
                }
                else
                {
                    NotifyIcon?.Dispose();
                }
            };
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var chrome = new WindowChrome { CaptionHeight = 0, UseAeroCaptionButtons = false };
            WindowChrome.SetWindowChrome(this, chrome);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (RegisterTaskTrayIcon && this.Icon != null)
            {
                Bitmap icon;
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create((BitmapSource)this.Icon));
                    enc.Save(outStream);
                    icon = new Bitmap(outStream);
                }

                NotifyIcon = new NotifyIcon
                {
                    Icon = System.Drawing.Icon.FromHandle(icon.GetHicon()),
                    Visible = true
                };

                var cms = new ContextMenuStrip();
                var tsm = new ToolStripMenuItem { Text = @"Exit" };
                tsm.Click += (sender, args) =>
                {
                    NotifyIcon.Dispose();
                    NotifyIcon = null;
                    Close();
                };
                cms.Items.Add(tsm);
                NotifyIcon.ContextMenuStrip = cms;
            }

            Loaded += (s, a) =>
            {
                // 地の果てまで追い詰めて内部Gridを取得する
                var child = VisualTreeHelper.GetChild(this, 0);
                new Action(() =>
                {
                    var maxSearch = long.MaxValue;
                    var count = 0L;
                    while (count < maxSearch)
                    {
                        try
                        {
                            if (child.GetType() == typeof(BorderedWindow))
                            {
                                BorderedWindow = (BorderedWindow)child;
                                return;
                            }
                            else
                            {
                                child = VisualTreeHelper.GetChild(child, 0);
                            }
                            count++;
                        }
                        catch
                        {
                            return;
                        }
                    }
                })();
            };

            StateChanged += (s, a) =>
            {
                var marginValue = 0;
                if (this.WindowState == WindowState.Maximized)
                {
                    marginValue = 8;
                }
                BorderedWindow.Margin = new Thickness(marginValue);
            };
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            // アタッチしたウィンドウ内の要素すべてに通知してアクセントカラーの適用・非適用を切り替えられるようにする
            EventProxy.InvokeOnActivated(this, e);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            EventProxy.InvokeOnDeactivated(this, e);
        }

        public void SetNotifyIconMenu(string exitText = @"Exit", List<NotifyIconMenu> notifyIconMenuList = null)
        {
            var cms = new ContextMenuStrip();
            ToolStripMenuItem tsm;
            notifyIconMenuList?.ForEach(x =>
            {
                tsm = new ToolStripMenuItem {Text = x.Text};
                tsm.Click += (sender, args) => { if (x.Action != null) x.Action(); };
                cms.Items.Add(tsm);
            });

            tsm = new ToolStripMenuItem { Text = exitText };
            tsm.Click += (sender, args) =>
            {
                NotifyIcon.Dispose();
                NotifyIcon = null;
                Close();
            };
            cms.Items.Add(tsm);

            NotifyIcon.ContextMenuStrip = cms;
        }

        #region public static void ShowNotifyBaloon()

        public void ShowNotifyBaloon(string title, string body, int timeout = 10000)
        {
            if (NotifyIcon.Visible && NotifyIcon.Icon != null)
            {
                NotifyIcon.BalloonTipTitle = title;
                NotifyIcon.BalloonTipText = body;

                NotifyIcon.ShowBalloonTip(timeout);
            }
        }

        public void ShowNotifyBaloon(string title, string body, ToolTipIcon icon, int timeout = 10000)
        {
            if (NotifyIcon.Visible && NotifyIcon.Icon != null)
            {
                NotifyIcon.BalloonTipTitle = title;
                NotifyIcon.BalloonTipText = body;
                NotifyIcon.BalloonTipIcon = icon;

                NotifyIcon.ShowBalloonTip(timeout);
            }
        }

        #endregion public static void ShowNotifyBaloon()
    }

    public class NotifyIconMenu
    {
        public string Text { get; private set; }
        public Action Action { get; private set; }

        public NotifyIconMenu(string text, Action action)
        {
            Text = text;
            Action = action;
        }
    }
}