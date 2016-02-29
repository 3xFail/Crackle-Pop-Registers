using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace CSharpClient
{
    public class connection_session
    {

        public connection_session(string hostString, int port, string username, string password)
        {
            _username = username;
            //connect to a remote device


            //parses the passed in host string to check for multiple servers
            //DNS resolver within 
            List<IPHostEntry> hosts = parseServerList(hostString);

            //DNS resolver resolves IP address from hostname (currently only using first returned IP)
            //host = Dns.GetHostEntry(hostString);




            //establish the remote endpoint for the socket
            //_remoteEP = new IPEndPoint( IPAddress.Parse( host ), port );      **DEPRECATED** 
            //_remoteEP = new IPEndPoint(hosts[0].AddressList[0], port);        **DEPRECATED**
            //support for server redundancy - if one server fails, try the next one
            for (int idx = 0; idx < hosts.Count; idx++)
            {
                try
                {
                    _remoteEP = new IPEndPoint(hosts[idx].AddressList[0], port);
                    break;
                }
                catch (Exception e)
                {
                    if (idx == hosts.Count - 1)
                        throw e;
                }

            }




            //create a TCP/IP socket
            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _sender.ReceiveBufferSize = message.header_length + message.id_length + message.max_body_length + message.username_length;

            connect(password);
        }


        //parses the passed in string and returns a list of all host entries found
        private List<IPHostEntry> parseServerList(string rawHostString)
        {
            //list of hosts to be returned
            List<IPHostEntry> parsedHosts = new List<IPHostEntry>();

            //splits the passed strings for multiple hosts
            string[] splitHostNames = rawHostString.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

            //adds a host entry to the final product for each host detected
            foreach (string individualHost in splitHostNames)
            {
                IPAddress throwAwayIP;
                if (individualHost == "localhost" || individualHost == "127.0.0.1")
                {
                    IPHostEntry localhost = new IPHostEntry();
                    localhost.AddressList = new IPAddress[1];
                    localhost.AddressList[0] = IPAddress.Parse("127.0.0.1");
                    localhost.HostName = "localhost";
                    parsedHosts.Add(localhost);
                }
                else if (IPAddress.TryParse(individualHost, out throwAwayIP))
                {

                }
                else
                    parsedHosts.Add(Dns.GetHostEntry(individualHost));
            }

            return parsedHosts;
        }


        private void connect(string password)
        {
            //connects the socket to the remote endpoint. catch any errors.

            _sender.Connect(_remoteEP);
            byte[] msg = Encoding.ASCII.GetBytes(_username + " " + password);

            _sender.Send(msg);

            Thread.Sleep(500);
            read_response();

        }

        public void write(message msg)
        {
            bool write_in_progress = (_msg_queue.Count != 0);
            _msg_queue.Enqueue(msg);
            if (!write_in_progress)
            {
                write();
            }
        }

        public void write(string msg)
        {
            write(new message(msg));
        }

        private void write()
        {
            _msg_queue.First().encode_id(_id); //attach unique ID to message
            _msg_queue.First().encode_username(_username); //attach username to end of message

            int bytessent = _sender.Send(_msg_queue.First().data(), _msg_queue.First().total_length(true), SocketFlags.None);
            read_response();

            _msg_queue.Dequeue();
            if (_msg_queue.Count > 0)
            {
                write();
            }
        }

        public void read_response()
        {
            message msg = new message();

            _sender.Receive(msg._data, 0, message.header_length, SocketFlags.None);
            if (msg.decode_header())
            {
                _sender.Receive(msg._data, message.header_length, message.id_length, SocketFlags.None);
                if (msg.decode_id())
                {
                    if (msg._body_length != 0)
                    {
                        _sender.Receive(msg._data, message.header_length + message.id_length, msg._body_length, SocketFlags.None);
                        parse_message(msg);
                    }
                    else
                        Response = new XmlDocument().GetElementsByTagName(""); //if we have an incoming empty message, don't bother parsing. Just set Response to an empty list
                }
            }
        }

        public void parse_message(message msg)
        {
            if (msg.ToString() == "valid_login")
                _id = msg._id;
            else if (msg.ToString() == "invalid_login")
                throw new InvalidOperationException("Invalid Login");
            else
            {
                var doc = new XmlDocument();

                try
                {
                    doc.LoadXml(msg.ToString()); //try to convert the response to an xml object.
                }
                catch (XmlException) { } //if it fails, then GetElementsByTagName is still fine. It will simply return an empty list. So we don't need to do anything here.

                Response = doc.GetElementsByTagName("row"); //If any stored procedure does not return data with 'FOR XML RAW', this will be empty despite valid xml being found.

            }
        }


        private IPEndPoint _remoteEP;
        private Socket _sender;
        public XmlNodeList Response { get; set; }
        private Queue<message> _msg_queue = new Queue<message>();

        public int _id = -1;
        private string _username;
    }
}
