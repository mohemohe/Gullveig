using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gullveig
{
    public class BorderedWindow : Grid
    {
        public SolidColorBrush DeActiveColorBlush = new SolidColorBrush(Color.FromArgb(0xFF, 0x5E, 0x5E, 0x5E));
        private Border WindowBorder { get; set; }

        #region DependencyProperty

        #region AccentColor

        public static readonly DependencyProperty AccentColorProperty =
        DependencyProperty.Register("AccentColor",
                                    typeof(SolidColorBrush),
                                    typeof(BorderedWindow),
                                    new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC))));

        public SolidColorBrush AccentColor
        {
            get { return (SolidColorBrush)GetValue(AccentColorProperty); }
            set
            {
                SetValue(AccentColorProperty, value);
            }
        }

        #endregion AccentColor

        #region Theme

        public enum ThemeColor
        {
            Dark,
            White
        }

        public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register("Theme",
                                    typeof(ThemeColor),
                                    typeof(BorderedWindow),
                                    new FrameworkPropertyMetadata(ThemeColor.White, OnThemeChanged));

        public ThemeColor Theme
        {
            get { return (ThemeColor)GetValue(ThemeProperty); }
            set
            {
                SetValue(ThemeProperty, value);
                switch (value)
                {
                    case ThemeColor.Dark:
                        this.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x2D, 0x2D, 0x30));
                        break;

                    case ThemeColor.White:
                        this.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFC, 0xFC));
                        break;
                }
            }
        }

        private static void OnThemeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var window = obj as BorderedWindow;
            if (window != null)
            {
                switch (window.Theme)
                {
                    case ThemeColor.Dark:
                        window.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x2D, 0x2D, 0x30));
                        break;

                    case ThemeColor.White:
                        window.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFC, 0xFC));
                        break;
                }
            }
        }

        #endregion Theme

        #endregion DependencyProperty

        public BorderedWindow()
        {
            this.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(double.MaxValue, GridUnitType.Star) });
            this.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            EventProxy.OnActivated += EventProxyOnOnActivated;
            EventProxy.OnDeactivated += EventProxyOnOnDeactivated;

            // ウィンドウボーダーの生成
            WindowBorder = new Border();
            WindowBorder.BorderBrush = new SolidColorBrush();
            WindowBorder.BorderThickness = new Thickness(1);
            WindowBorder.SetCurrentValue(RowSpanProperty, 3);

            // 現在の子要素を待避
            var currentChildren = new UIElement[this.Children.Count];
            this.Children.CopyTo(currentChildren, 0);
            this.Children.Clear();

            // 待避した子要素を内部Gridに追加
            var innerGrid = new Grid();
            innerGrid.BeginInit();
            foreach (UIElement currentChild in currentChildren)
            {
                innerGrid.Children.Add(currentChild);
            }
            innerGrid.EndInit();

            // よしなにする
            WindowBorder.Child = innerGrid;
            this.Children.Add(WindowBorder);
        }

        private void EventProxyOnOnActivated(object sender, EventArgs eventArgs)
        {
            WindowBorder.BorderBrush = AccentColor;
        }

        private void EventProxyOnOnDeactivated(object sender, EventArgs eventArgs)
        {
            WindowBorder.BorderBrush = DeActiveColorBlush;
        }
    }
}