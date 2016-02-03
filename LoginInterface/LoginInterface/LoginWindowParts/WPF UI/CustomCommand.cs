using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

// Document me.
namespace SnapRegisters
{
	public static class CustomCommands
	{
		public static readonly RoutedUICommand Exit = new RoutedUICommand
		(
			"Exit",
			"Exit",
			typeof(CustomCommands),
			new InputGestureCollection()
			{
					new KeyGesture(Key.F4, ModifierKeys.Alt)
			}
		);
	}
}
