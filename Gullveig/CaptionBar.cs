using System;
using System.Windows;
using System.Windows.Controls;

namespace Gullveig
{
    public class CaptionBar : DockPanel
    {
        private BorderedWindow BorderedWindow { get; set; }

        public CaptionBar()
        {
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Top;

            try
            {
                this.SetCurrentValue(Grid.RowProperty, 0);
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
            this.VerticalAlignment = VerticalAlignment.Top;
            if (this.Height.Equals(double.NaN))
            {
                this.Height = 32;
            }
            this.Background = BorderedWindow?.AccentColor;

            this.MouseLeftButtonDown += (_, __) => System.Windows.Window.GetWindow(this)?.DragMove();
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