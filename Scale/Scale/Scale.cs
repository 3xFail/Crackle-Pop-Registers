using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HidLibrary;
using System.Threading;

namespace Scale
{

    //Usage: 
    //Create new scale instance
    //Set _Grams_Mode to true if you want grams intead of pounds
    //Call GetWeightAsString() or GetWeightAsDecimal() as many times as you want
    //Scale automatically connects as needed. 
    //NOTE: CURRENTLY ONLY COMPATIBLE WITH DYMO M10 & M25 SCALES
    public class Scale
    {

        private int _VENDOR_ID = 0x0922;
        private int _PRODUCT_ID = 0x8003;

        public bool _Grams_Mode { private get; set; } = false;

        private HidDevice _scale;
        private HidDeviceData _data;


        private decimal? _weight;

        public bool _isStable;


        public bool IsConnected
        {
            get
            {
                if (_scale == null)
                    return false;
                else
                    return _scale.IsConnected;
            }
        }

        public string GetWeightAsString()
        {
            if (!IsConnected)
            {
                Connect();
            }


            if (IsConnected)
            {
                GetWeightFromScale();
                if (Status() == 5)
                {
                    return "neg";
                }
                return _weight.ToString();
            }
            else
                return "null";


        }
        public decimal? GetWeightAsDecimal()
        {
            if (!IsConnected)
            {
                Connect();
            }


            if (IsConnected)
            {
                GetWeightFromScale();
                if (Status() == 5)
                {
                    return -1;
                }
                return _weight;
            }
            else
                return null;

        }





        private bool Connect()
        {
            _scale = HidDevices.Enumerate(_VENDOR_ID, _PRODUCT_ID).FirstOrDefault();


            if (_scale != null)
            {

                int tries = 0;
                _scale.OpenDevice();

                while (!_scale.IsConnected && tries < 10)
                {
                    Thread.Sleep(50);
                    tries++;
                }

                return _scale.IsConnected;
            }
            else
                return false;
        }


        public void Disconnect()
        {
            if (_scale.IsConnected)
            {
                _scale.CloseDevice();
                _scale.Dispose();
            }
        }

        public void DebugScaleData()
        {
            if (IsConnected)
            {
                for (int i = 0; i < _data.Data.Length; ++i)
                {
                    Console.WriteLine("Byte {0}: {1}", i, _data.Data[i]);
                }
            }
        }


        private void GetWeightFromScale()
        {

            _weight = null;
            _isStable = false;

            _data = _scale.Read(250);



            _weight = (Convert.ToDecimal(_data.Data[4]) +
                Convert.ToDecimal(_data.Data[5]) *
                (Convert.ToDecimal(_data.Data[3]) + 1)); //Educated guess, could be wrong

            switch (Convert.ToInt32(_data.Data[2]))
            {



                case 2:     //Scale reading in grams
                    if (!_Grams_Mode)
                        _weight *= (53M / 3600M);
                    else
                        _weight *= 6M + (2M / 3M);
                    break;
                case 11:    //Scale reading in ounces
                    _weight /= 10M;
                    if (!_Grams_Mode)
                        _weight *= 0.0625M;
                    else
                        _weight *= 28.349523125M;

                    break;
                case 12:    //Scale reading in pounds
                            //Already in pounds, don't need to do anything
                    break;
            }
            _isStable = _data.Data[1] == 0x4;
        }


        //Returns the status of the scale.
        //      Byte 1 == Scale Status
        //          1 == Fault
        //          2 == Stable @ 0
        //          3 == In Motion
        //          4 == Stable
        //          5 == Under 0
        //          6 == Over Weight
        //          7 == Requires Calibration
        //          8 == Requires Re-Zeroing
        private decimal Status()
        {
            return _data.Data[1];
        }



    }
}
