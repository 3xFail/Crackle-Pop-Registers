using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpClient
{
    public class SynchronousSocketClient
    {
        public static int Main( String[] args )
        {
            Console.WriteLine( "Enter the host: " );
            string host = Console.ReadLine();

            Console.WriteLine( "Enter the port: " );
            int port = Int32.Parse( Console.ReadLine() );

            Console.WriteLine( "Enter your username: " );
            string username = Console.ReadLine();

            Console.WriteLine( "Enter your password: " );
            string password = Console.ReadLine();

            var server = new connection_session( host, port, username, password );
            Console.WriteLine( "connecting..." );
            server.write( new message( Console.ReadLine() ) );
            return 0;


        }
    }
}