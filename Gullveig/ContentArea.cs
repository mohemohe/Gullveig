using System;
using System.Windows;
using System.Windows.Controls;

namespace Gullveig
{
    public class ContentArea : Grid
    {
        public ContentArea()
        {
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;

            try
            {
                this.SetCurrentValue(Grid.RowProperty, 1);
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
        }
    }
}