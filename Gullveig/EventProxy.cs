using System;

namespace Gullveig
{
    public static class EventProxy
    {
        public static event EventHandler OnActivated;

        internal static void InvokeOnActivated(object sender, EventArgs args)
        {
            OnActivated?.Invoke(sender, args);
        }

        public static event EventHandler OnDeactivated;

        internal static void InvokeOnDeactivated(object sender, EventArgs args)
        {
            OnDeactivated?.Invoke(sender, args);
        }

        public static event EventHandler OnStateChanged;

        internal static void InvokeOnStateChanged(object sender, EventArgs args)
        {
            OnStateChanged?.Invoke(sender, args);
        }
    }
}