/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: db_conn_str.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma once

#include <string>
using std::string;
#include <boost\format.hpp>
using boost::format;
#include <boost\algorithm\string\replace.hpp>
using boost::replace_all;

/************************************************************************
* Class: db_conn_str, inherits from: None
*
* Purpose:
*
* Manager functions:	
*
*	Constructor db_conn_str(const string server = "", const string db = "", const string user = "", const string pass = "")
*		Modifiers: public    
*		Description:
*
*
* Operator overloads:
*
*
* Methods:	
*	
*	string format_string()
*		Modifiers: public const   
*		Description:
*
*	void format_string(const string & format_string)
*		Modifiers: public    
*		Description:
*
*	string server()
*		Modifiers: public const   
*		Description:
*
*	void server(const string & server)
*		Modifiers: public    
*		Description:
*
*	string database()
*		Modifiers: public const   
*		Description:
*
*	void database(const string & database)
*		Modifiers: public    
*		Description:
*
*	string username()
*		Modifiers: public const   
*		Description:
*
*	void username(const string & user)
*		Modifiers: public    
*		Description:
*
*	string password()
*		Modifiers: public const   
*		Description:
*
*	void password(const string & pass)
*		Modifiers: public    
*		Description:
*
*	string get()
*		Modifiers: public const   
*		Description:
*
*
* Data Members:
*
*	string _format_string = "odbc:Driver={SQL Server};SERVER=%SV%;DATABASE=%DB%;UID=%USER%;PWD=%PASS%"
*		modifiers: private  
*
*	string _server 
*		modifiers: private  
*
*	string _database 
*		modifiers: private  
*
*	string _username 
*		modifiers: private  
*
*	string _password 
*		modifiers: private  
*
*
*************************************************************************/
class db_conn_str
{
public:
	db_conn_str( const string server = "", const string db = "", const string user = "", const string pass = "" );

	string format_string() const;
	void format_string( const string & format_string );

	string server() const;
	void server( const string & server );

	string database() const;
	void database( const string & database );

	string username() const;
	void username( const string & user );

	string password() const;
	void password( const string & pass );

	string get() const;
private:
	string _format_string = "odbc:Driver={SQL Server};SERVER=%SV%;DATABASE=%DB%;UID=%USER%;PWD=%PASS%";

	string _server;
	string _database;	
	string _username;
	string _password;
};

/* Formatting string format:
	Anywhere %SV% is found, the server value will be inserted.
	Anywhere %DB% is found, the database value will be inserted.
	Anywhere %USER% is found, the username value will be inserted.
	Anywhere %PASS% is found, the password value will be inserted.
*/
