using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace System.Windows.Controls
{
    //http://www.rhyous.com/2010/06/18/how-to-limit-or-prevent-characters-in-a-textbox-in-csharp/
    public class DigitBox :TextBox
    {
        #region Constructors
        /// <summary> 
        /// The default constructor
        /// </summary>
        public DigitBox()
        {
            TextChanged += new TextChangedEventHandler( OnTextChanged );
            KeyDown += new KeyEventHandler( OnKeyDown );
            PreviewTextInput += new TextCompositionEventHandler( OnPreviewTextInput );
        }
        #endregion

        #region Properties
        new public string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
            }
        }

        #endregion

        #region Functions
        private bool IsNumberKey( Key inKey )
        {
            if( inKey == Key.OemMinus )
                return true;

            if( inKey < Key.D0 || inKey > Key.D9 )
            {
                if( inKey < Key.NumPad0 || inKey > Key.NumPad9 )
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsActionKey( Key inKey )
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || inKey == Key.Return || Keyboard.Modifiers.HasFlag( ModifierKeys.Alt );
        }

        private string LeaveOnlyNumbers( string str )
        {
            for( int i = 0; i < str.Length; ++i )
            {
                if( !IsDigit( str[i] ) ) //|| ( str[i] == '-' && i != 0 && str.Contains( "-" ) ) )
                {
                    str = str.Replace( str[i].ToString(), "" );
                }
            }
            return str;
        }

        public bool IsDigit( char c )
        {
            return ( c >= '0' && c <= '9' );
        }
        #endregion

        #region Event Functions
        protected void OnKeyDown( object sender, KeyEventArgs e )
        {
            e.Handled = !IsNumberKey( e.Key ) && !IsActionKey( e.Key );
        }

        protected void OnTextChanged( object sender, TextChangedEventArgs e )
        {
            //base.Text = LeaveOnlyNumbers( Text );
        }

        Regex regex = new Regex( "[^0-9.-]+" );
        protected void OnPreviewTextInput( object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch( e.Text );
        }
        #endregion
    }
}