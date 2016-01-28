using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpClient
{
    public class connection_session
    {
        public connection_session( string host, int port, string username, string password )
        {
            _username = username;
            //connect to a remote device
            try
            {
                //establish the remote endpoint for the socket
                _remoteEP = new IPEndPoint( IPAddress.Parse( host ), port );

                //create a TCP/IP socket
                _sender = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
                _sender.ReceiveBufferSize = message.header_length + message.id_length + message.max_body_length + message.username_length;

                connect( password );
            }
            catch( Exception e )
            {
                Console.WriteLine( e.ToString() );
            }

        }

        private void connect( string password )
        {
            //connects the socket to the remote endpoint. catch any errors.
            try
            {
                _sender.Connect( _remoteEP );
                Console.WriteLine( "Socket connected to {0}", _sender.RemoteEndPoint.ToString() );
                byte[] msg = Encoding.ASCII.GetBytes( _username + " " + password );
                _sender.Send( msg );
                Thread.Sleep( 500 );
                read_response();


            }
            catch( ArgumentNullException ane )
            {
                Console.WriteLine( "ArgumentNullException: {0}", ane.ToString() );
            }
            catch( SocketException se )
            {
                Console.WriteLine( "SocketException : {0}", se.ToString() );
            }
            catch( Exception e )
            {
                Console.WriteLine( "Unexpected exception : {0}", e.ToString() );
            }
        }

        public void write( message msg )
        {
            bool write_in_progress = ( _msg_queue.Count != 0 );
            _msg_queue.Enqueue( msg );
            if( !write_in_progress )
            {
                write();
            }
        }

        private void write()
        {
            _msg_queue.First().encode_id( _id ); //attach unique ID to message
            _msg_queue.First().encode_username( _username ); //attach username to end of message

            int bytessent = _sender.Send( _msg_queue.First().data(), _msg_queue.First().total_length( true ), SocketFlags.None );
            //Thread.Sleep( 1000 );
            read_response();

            _msg_queue.Dequeue();
            if( _msg_queue.Count > 0 )
            {
                write();
            }
        }

        public void read_response()
        {
            message msg = new message();

            _sender.Receive( msg._data, 0, message.header_length, SocketFlags.None );
            if( msg.decode_header() )
            {
                _sender.Receive( msg._data, message.header_length, message.id_length, SocketFlags.None );
                if( msg.decode_id() )
                {
                    Console.WriteLine( "received messaged of length: " + msg._body_length );
                    _sender.Receive( msg._data, message.header_length + message.id_length, msg._body_length, SocketFlags.None );
                    parse_message( msg );
                }
            }
        }

        public void parse_message( message msg )
        {
            if( msg.ToString() == "valid_login" )
            {
                _id = msg._id;
                Console.WriteLine( "valid_login, your ID is" + _id );
            }
            else
                Console.WriteLine( msg.ToString() );
        }


        private IPEndPoint _remoteEP;
        private Socket _sender;

        private Queue<message> _msg_queue = new Queue<message>();

        private int _id;
        private string _username;
    }
}
