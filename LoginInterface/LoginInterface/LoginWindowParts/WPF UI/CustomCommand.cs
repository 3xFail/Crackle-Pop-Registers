﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

// Document me.
namespace SnapRegisters.Commands
{
    public static class CustomCommands
    {
        public static RoutedUICommand Exit = new RoutedUICommand
        (
            "Exit",
            "Exit",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
        );


        public static readonly RoutedUICommand ManagerFunctions = new RoutedUICommand
            (
            "Manager_Functions",
            "Manager_Functions",
            typeof(CustomCommands)



            );

        public static readonly RoutedUICommand PayByCash = new RoutedUICommand
        (
        "Pay_By_Cash",
        "Pay_By_Cash",
        typeof(CustomCommands)

        );

        public static RoutedUICommand doNothing = new RoutedUICommand();


    }


}
