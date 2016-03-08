using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gullveig
{
    /// <summary>
    /// CaptionButtons.xaml の相互作用ロジック
    /// </summary>
    public partial class CaptionButtons : UserControl
    {
        private BorderedWindow BorderedWindow { get; set; }
        private SolidColorBrush ActiveColorBlush { get; set; }
        private SolidColorBrush DeActiveColorBlush { get; set; } = new SolidColorBrush(Color.FromArgb(0xFF, 0x30, 0x30, 0x30));
        private bool _isLightTheme;

        public CaptionButtons()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                new Action(() =>
                {
                    var parent = this.Parent;
                    while (true)
                    {
                        try
                        {
                            if (parent.GetType() == typeof(BorderedWindow))
                            {
                                BorderedWindow = parent as BorderedWindow;
                                return;
                            }
                            else
                            {
                                parent = LogicalTreeHelper.GetParent(parent);
                            }
                        }
                        catch
                        {
                            return;
                        }
                    }
                })();
                if (BorderedWindow == null)
                {
                    throw new NotSupportedException();
                }

                var accentColor = BorderedWindow.AccentColor.Color;
                var grayscale = 0.298912 * accentColor.R + 0.586611 * accentColor.G + 0.114478 * accentColor.B;
                if (grayscale > 127)
                {
                    ActiveColorBlush = new SolidColorBrush(Color.FromArgb(0xFF, 0x30, 0x30, 0x30));
                }
                else
                {
                    ActiveColorBlush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFC, 0xFC));
                }

                MinimizeButton.PreviewMouseLeftButtonUp += (s, a) => System.Windows.Window.GetWindow(this).WindowState = WindowState.Minimized;
                MaximizeButton.PreviewMouseLeftButtonUp += (s, a) =>
                {
                    switch (System.Windows.Window.GetWindow(this)?.WindowState)
                    {
                        case WindowState.Normal:
                            System.Windows.Window.GetWindow(this).WindowState = WindowState.Maximized;
                            break;

                        case WindowState.Maximized:
                            System.Windows.Window.GetWindow(this).WindowState = WindowState.Normal;
                            break;
                    }
                };
                CloseButton.PreviewMouseLeftButtonUp += (s, a) => System.Windows.Window.GetWindow(this)?.Close();

                EventProxy.OnActivated += (s, a) =>
                {
                    MinimizeButtonText.Foreground = ActiveColorBlush;
                    MaximizeButtonText.Foreground = ActiveColorBlush;
                    CloseButtonText.Foreground = ActiveColorBlush;
                };

                EventProxy.OnDeactivated += (s, a) =>
                {
                    MinimizeButtonText.Foreground = DeActiveColorBlush;
                    MaximizeButtonText.Foreground = DeActiveColorBlush;
                    CloseButtonText.Foreground = DeActiveColorBlush;
                };

                EventProxy.OnStateChanged += (s, a) =>
                {
                    switch (System.Windows.Window.GetWindow(this)?.WindowState)
                    {
                        case WindowState.Normal:
                            MaximizeButtonText.Text = "1";
                            break;

                        case WindowState.Maximized:
                            MaximizeButtonText.Text = "2";
                            break;
                    }
                };
            };
        }
    }
}