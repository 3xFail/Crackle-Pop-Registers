//#define RESET_DEFAULT_COLORS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapRegisters;
using PointOfSales;
using PointOfSales.Users;
using System.Xml;
using CSharpClient;
using System.Windows;
using System.IO;

namespace SnapRegisters
{
    public class ProgramSettings
    {
        private object _streamsync = new object();
        static object _lock = new object();

        static ProgramSettings _instance = null;

        private Employee _loggedIn = null;

        string settingsString = null;

        public SettingsBox _settings { get; private set; } = null;





        public static ProgramSettings getInstance(Employee loggedIn)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ProgramSettings(loggedIn);
                    }
                }
            }
            else if (_instance._loggedIn.ID != loggedIn.ID)
            {
                _instance = new ProgramSettings(loggedIn);
            }

            return _instance;
        }



        private ProgramSettings(Employee loggedIn)
        {

            _loggedIn = loggedIn;
            _settings = new SettingsBox();

#if !RESET_DEFAULT_COLORS
            //Download();
#endif

#if RESET_DEFAULT_COLORS

            _settings = new SettingsBox();
            _settings.Primary_Color = System.Drawing.Color.LightBlue;
            _settings.Secondary_Color = System.Drawing.Color.Blue;
            _settings.Border_Color = System.Drawing.Color.Black;

#endif


        }


        public void Download()
        {


            DBInterface.GetSettings(_loggedIn.Permissions);

            settingsString = DBInterface.Response[0].Get("SettingsFile");



            using (XmlReader reader = XmlReader.Create(new StringReader(settingsString)))
            {

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "PrimaryColor":
                                _settings.Primary_Color = System.Drawing.Color.FromArgb(Convert.ToInt32(reader.ReadElementContentAsString()));
                                //break;
                            //case "SecondaryColor":
                                _settings.Secondary_Color = System.Drawing.Color.FromArgb(Convert.ToInt32(reader.ReadElementContentAsString()));
                                //break;
                            //case "BorderColor":
                                _settings.Border_Color = System.Drawing.Color.FromArgb(Convert.ToInt32(reader.ReadElementContentAsString()));
                                break;
                        }
                    }

                }
            }
        }

        public void Upload()
        {
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("Colors");

                //Readable format RGB
                //writer.WriteElementString("PrimaryColor", "RGB(" + _settings.Primary_Color.R.ToString() + "," + _settings.Primary_Color.G.ToString() + "," + _settings.Primary_Color.B.ToString() + ")");
                //writer.WriteElementString("SecondaryColor", "RGB(" + _settings.Secondary_Color.R.ToString() + "," + _settings.Secondary_Color.G.ToString() + "," + _settings.Secondary_Color.B.ToString() + ")");
                //writer.WriteElementString("BorderColor", "RGB(" + _settings.Border_Color.R.ToString() + "," + _settings.Border_Color.G.ToString() + "," + _settings.Border_Color.B.ToString() + ")");

                //RGB int values
                writer.WriteElementString("PrimaryColor", _settings.Primary_Color.R.ToString() + _settings.Primary_Color.G.ToString() + _settings.Primary_Color.B.ToString());
                writer.WriteElementString("SecondaryColor", _settings.Secondary_Color.R.ToString() + _settings.Secondary_Color.G.ToString() + _settings.Secondary_Color.B.ToString());
                writer.WriteElementString("BorderColor", _settings.Border_Color.R.ToString() + _settings.Border_Color.G.ToString() + _settings.Border_Color.B.ToString());

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }


            DBInterface.ChangeSettings(_loggedIn.Permissions, sb.ToString());


        }



        //Settings Variables
        public class SettingsBox
        {
            public System.Drawing.Color Primary_Color { get; set; }
            public System.Drawing.Color Secondary_Color { get; set; }
            public System.Drawing.Color Border_Color { get; set; }
        }







    }
}
