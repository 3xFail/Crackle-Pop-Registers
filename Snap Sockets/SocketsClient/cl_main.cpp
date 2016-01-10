/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsClient
* Filename: cl_main.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#include "../SharedCode/message.h"
#include "connection_session.h"
#include <boost/asio.hpp>
using boost::asio::ip::tcp;
using boost::asio::io_service;

#include <iostream>
using std::cout;
using std::cin;
#include <thread>
using std::thread;
#include <string>
using std::string;


int main()
{
	try
	{
		string host;
		cout << "Enter the host: ";
		cin >> host;
		if( host == "l" )
			host = "localhost";

		string port;
		cout << "Enter the port: ";
		cin >> port;

		string username;
		cout << "Enter your username: ";
		cin >> username;

		string password;
		cout << "Enter your password: ";
		cin >> password;

		io_service service;

		connection_session connection( service, host, port, username, password );

		thread thread( [&service]() { service.run(); } );

		char line[message::max_body_length + 1];
		cin.ignore();
		while( cin.getline( line, message::max_body_length ) )
		{
			connection.write( message( line ) );
		}

		connection.close(); 
		thread.join();
	}
	catch( std::exception & e )
	{
		cout << "Exception: " << e.what() << "\n";
	}

	system( "pause" );
	return 0;
}
