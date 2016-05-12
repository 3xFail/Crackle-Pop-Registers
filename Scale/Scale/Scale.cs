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


        public class VolatileDecimal
        {
            public decimal? _internalWeight = null;
        }




        private int _VENDOR_ID = 0x0922;
        private int _PRODUCT_ID = 0x8003;
        private readonly int TIMEOUT_TRIES = 20;

        public bool _Grams_Mode { private get; set; } = false;

        private HidDevice _scale;
        private HidDeviceData _data;


        private volatile VolatileDecimal _volatileWeight = new VolatileDecimal();
        private decimal? _weight = null;
        public volatile bool _isStable = false;


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
                return _volatileWeight._internalWeight.ToString();
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
                return _volatileWeight._internalWeight;
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


        public decimal? getStabilizedWeight()
        {

            decimal? weight_previous = null;
            decimal? weight_current = null;


            if (IsConnected)
            {
                weight_previous = GetWeightAsDecimal();

                int tries = 0;

                while ((weight_current != weight_previous) || weight_current == null || weight_current == -1 || weight_current == 0)
                {
                    weight_current = weight_previous;

                    Thread.Sleep(750);

                    weight_previous = GetWeightAsDecimal();



                    //Timeout condition
                    tries++;
                    if (tries >= TIMEOUT_TRIES)
                        throw new TimeoutException("Stabilized weight retrieval timed out after " + TIMEOUT_TRIES + " tries.");



                }


                return weight_current;

            }
            else throw new NullReferenceException("Attempted to retrieve stabilized weight without being connected to scale.");




        }

        private void GetWeightFromScale()
        {

            _volatileWeight._internalWeight = null;

            _isStable = false;


            _data = _scale.Read(250);



            _volatileWeight._internalWeight = (Convert.ToDecimal(_data.Data[4]) +
                Convert.ToDecimal(_data.Data[5]) *
                (Convert.ToDecimal(_data.Data[3]) + 1)); //Educated guess, could be wrong

            switch (Convert.ToInt32(_data.Data[2]))
            {



                case 2:     //Scale reading in grams
                    if (!_Grams_Mode)
                        _volatileWeight._internalWeight *= (53M / 3600M);
                    else
                        _volatileWeight._internalWeight *= 6M + (2M / 3M);
                    break;
                case 11:    //Scale reading in ounces
                    _volatileWeight._internalWeight /= 10M;
                    if (!_Grams_Mode)
                        _volatileWeight._internalWeight *= 0.0625M;
                    else
                        _volatileWeight._internalWeight *= 28.349523125M;

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
            if (IsConnected)
                return _data.Data[1];
            else
                return 9999;
        }



    }
}
