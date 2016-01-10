/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: sv_main.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma region includes

//boost includes(networking)
#include <boost/asio.hpp>
using boost::asio::ip::address;
using boost::asio::io_service;
using boost::asio::ip::tcp; //tcp master race

//cppdb includes(sql server)
#include <cppdb/frontend.h>
using cppdb::cppdb_error;
#include "db_conn_str.h"
#include "sql_singleton.h"

#include "client.h"
#include "server.h"
#include "../SharedCode/scrypt-jane.c"

#include <iostream>
using std::cin;
using std::cout;
#include <string>
using std::string;
#include <fstream>
using std::ifstream;
#include <exception>
using std::exception;
#pragma endregion

db_conn_str load_connection_string();

#define RESULT_SIZE 16
#ifdef _DEBUG
	#define N_FACTOR 13 //how many chunks, increases both memory and CPU usage
#else
	#define N_FACTOR 16
#endif
#define R_FACTOR 3 //how many blocks are in a chunk, increases memory usage
#define P_FACTOR 1 //how many passes over N chunks. Increases CPU usage

string hash_salt_password( const string & user, const string & pass )
{
	unsigned char res[RESULT_SIZE];

	unsigned char * p = (unsigned char *)pass.c_str();
	unsigned char * s = (unsigned char *)user.c_str();

	scrypt( p, pass.length(), s, user.length(), N_FACTOR, R_FACTOR, P_FACTOR, res, RESULT_SIZE );
	return string( (char *)res );
}


int main()
{
#ifdef _DEBUG
	cout << "DEBUG BUILD. THIS WILL GREALY SLOW DOWN PASSWORD HASHING\n";
#endif

	unsigned short port;
	cout << "Enter server port: ";
	cin >> port;
	try
	{
		sql_singleton::instance().open( load_connection_string().get() );
		cout << "Sucessfully connected to database.\n";

		io_service io_service;
		server server( io_service, port );

		cout << "Server is listening on port " << port << "...\n";
		io_service.run();
	}
	catch( const cppdb_error & e ) //if it's a database error we'll catch that
	{
		cout << "Database Exception: " << e.what() << '\n';
	}
	catch( const exception & e ) //otherwise who knows what went wrong, jesus.
	{
		cout << "General Exception: " << e.what() << "\n";
	}

	system( "pause" );
	return 0;
}

db_conn_str load_connection_string()
{
	ifstream file( "database_connection_info.txt" );
	if( file.is_open() )
	{
		string server, db, user, pass;
		file >> server >> db >> user >> pass;
		return db_conn_str( server, db, user, pass );
	}
	else throw exception( "Cannot open database_connection_info.txt" );
}

