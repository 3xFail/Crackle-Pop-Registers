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

int main(int argc, char ** argv)
{
	//if (argc < 2)
	//{
	//	cout << "Required port number not passed.\n";
	//}

	// Hope you don't mind if I set the port to default to 6119
	int port = 0;

	if (argc >= 2)
	{
		port = atoi(argv[1]);
	}

	if (port == 0)
	{
		port = 6119;
		cout << "Valid port number not passed. Defaulting to 6119.\n";
		//cout << "Valid port number not passed.\n";
		//return 1;
	}

	try
	{
		sql_singleton::instance().open(load_connection_string().get());
		cout << "Sucessfully connected to database.\n";

		io_service io_service;
		server server(io_service, port);

		cout << "Server is listening on port " << port << "...\n";
		io_service.run();
	}
	catch (const cppdb_error & e) //if it's a database error we'll catch that
	{
		cout << "Database Exception: " << e.what() << '\n';
	}
	catch (const exception & e) //otherwise who knows what went wrong, jesus.
	{
		cout << "General Exception: " << e.what() << "\n";
	}

	system("pause");
	return 0;
}

db_conn_str load_connection_string()
{
	ifstream file("database_connection_info.txt");
	if (file.is_open())
	{
		string server, db, user, pass;
		file >> server >> db >> user >> pass;
		return db_conn_str(server, db, user, pass);
	}
	else throw exception("Cannot open database_connection_info.txt");
}

