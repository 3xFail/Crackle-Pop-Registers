/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: db_conn_str.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#include "db_conn_str.h"

//Constructor db_conn_str(string server, string db, string user, string pass)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
db_conn_str::db_conn_str(string server, string db, string user, string pass) : _server(server), _database(db), _username(user), _password(pass)
{
}

// format_string()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
string db_conn_str::format_string() const
{
	return _format_string;
}

// format_string(const string & format_string)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void db_conn_str::format_string(const string & format_string)
{
	_format_string = format_string;
}

// server()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
string db_conn_str::server() const
{
	return _server;
}

// server(const string & server)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void db_conn_str::server(const string & server)
{
	_server = server;
}

// database()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
string db_conn_str::database() const
{
	return _database;
}

// database(const string & database)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void db_conn_str::database(const string & database)
{
	_database = database;
}

// username()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
string db_conn_str::username() const
{
	return _username;
}

// username(const string & user)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void db_conn_str::username(const string & user)
{
	_username = user;
}

// password()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
string db_conn_str::password() const
{
	return _password;
}

// password(const string & pass)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void db_conn_str::password(const string & pass)
{
	_password = pass;
}

// get()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
string db_conn_str::get() const
{
	string t = _format_string;
	replace_all(t, "%DB%", _database);
	replace_all(t, "%SV%", _server);
	replace_all(t, "%USER%", _username);
	replace_all(t, "%PASS%", _password);
	return t;
}