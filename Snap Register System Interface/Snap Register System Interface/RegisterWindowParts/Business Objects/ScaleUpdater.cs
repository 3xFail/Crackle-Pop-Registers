using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SnapRegisters;


namespace Snap_Register_System_Interface.RegisterWindowParts.Business_Objects
{
    public class ScaleUpdater
    {
        private volatile bool _shouldStop;
        private RegisterMainWindow _mainWindow;
        private Scale.Scale _scale;

        public ScaleUpdater(RegisterMainWindow mainWindow, Scale.Scale scale)
        {
            _mainWindow = mainWindow;
            _scale = scale;
        }

        public void StartUpdating()
        {
            while (!_shouldStop)
            {

                string theWeight = _scale.GetWeightAsString();
                if (theWeight == "null")
                    _mainWindow.m_weightOnScreen = theWeight;
                else if (theWeight == "neg")
                    _mainWindow.m_weightOnScreen = theWeight;
                else
                    _mainWindow.m_weightOnScreen = Math.Round(Convert.ToDouble(_scale.GetWeightAsDecimal()), 2).ToString() + " Lb";


                _mainWindow.m_scaleStatusTextBox = _scale.Status().ToString();

                Thread.Sleep(375);

            }
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
    }
}
