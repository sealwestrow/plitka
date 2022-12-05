using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlitkaApp
{
    internal class UserCommands
    {
        static UserCommands()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.A, ModifierKeys.Control, "Ctrl+A"));

            SelectAll = new RoutedUICommand("SelectAll", "SelectAll", typeof(UserCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.Delete));

            Delete = new RoutedUICommand("Delete", "Delete", typeof(UserCommands), inputs);
        }

        public static RoutedCommand SelectAll { get; private set; }

        public static RoutedCommand Delete { get; private set; }
    }
}
