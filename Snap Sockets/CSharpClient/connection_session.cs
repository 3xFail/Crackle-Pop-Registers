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
    class connection_session
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
                int bytesSent = _sender.Send( msg );

                Thread.Sleep( 1000 );


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

            int bytessent = _sender.Send( _msg_queue.First().data() );
            Thread.Sleep( 1000 );

            _msg_queue.Dequeue();
            if( _msg_queue.Count > 0 )
            {
                write();
            }
        }




        private IPEndPoint _remoteEP;
        private Socket _sender;



        private message _cur_msg;
        private Queue<message> _msg_queue;

        private int _id;
        private string _username;
    }
}
