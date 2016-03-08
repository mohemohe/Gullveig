using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gullveig
{
    public class StatusBar : DockPanel
    {
        private TextBlock _textBlock = new TextBlock();
        private BorderedWindow BorderedWindow { get; set; }

        #region DependencyProperty

        #region Text

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text",
                                    typeof(string),
                                    typeof(BorderedWindow),
                                    new FrameworkPropertyMetadata(null, OnTextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                // 仕方ないからここに書く
                _textBlock.Text = value;
            }
        }

        // コールバックが動いていない気がする
        private static void OnTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var window = obj as StatusBar;
            if (window != null)
            {
                window._textBlock.Text = (string)e.NewValue;
            }
        }

        #endregion Text

        #region Foreground

        public static readonly DependencyProperty ForegroundProperty =
        DependencyProperty.Register("Foreground",
                                    typeof(SolidColorBrush),
                                    typeof(StatusBar),
                                    new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))));

        public SolidColorBrush Foreground
        {
            get { return (SolidColorBrush)GetValue(ForegroundProperty); }
            set
            {
                SetValue(ForegroundProperty, value);
                _textBlock.Foreground = value;
            }
        }

        #endregion Foreground

        #endregion DependencyProperty

        public StatusBar()
        {
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Top;

            try
            {
                this.SetCurrentValue(Grid.RowProperty, 2);
            }
            catch { }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (this.Parent.GetType() != typeof(BorderedWindow))
            {
                throw new NotSupportedException();
            }
            BorderedWindow = this.Parent as BorderedWindow;

            EventProxy.OnActivated += EventProxyOnOnActivated;
            EventProxy.OnDeactivated += EventProxyOnOnDeactivated;

            //this.Orientation = Orientation.Horizontal;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Bottom;
            if (this.Height.Equals(double.NaN))
            {
                this.Height = 24;
            }
            this.Background = BorderedWindow.AccentColor;

            _textBlock.VerticalAlignment = VerticalAlignment.Center;
            _textBlock.Margin = new Thickness(4, 0, 0, 0);
            _textBlock.Foreground = Foreground;
            _textBlock.Text = Text;
            this.Children.Add(_textBlock);

            if (System.Windows.Window.GetWindow(this)?.ResizeMode == ResizeMode.CanResize)
            {
                System.Windows.Window.GetWindow(this).ResizeMode = ResizeMode.CanResizeWithGrip;
            }
        }

        private void EventProxyOnOnActivated(object sender, EventArgs eventArgs)
        {
            this.Background = BorderedWindow.AccentColor;
        }

        private void EventProxyOnOnDeactivated(object sender, EventArgs eventArgs)
        {
            this.Background = BorderedWindow.DeActiveColorBlush;
        }
    }
}