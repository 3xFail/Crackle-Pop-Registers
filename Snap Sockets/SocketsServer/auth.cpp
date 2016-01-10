/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: auth.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#include "auth.h"

// valid_login(const string & user, const string & pass)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
bool auth::valid_login( const string & user, const string & pass )
{
	try
	{
		cppdb::statement statement = sql_singleton::instance() << "SELECT Password FROM SnapUsers WHERE Username = CONVERT( VARCHAR(50), ? )" << user;
		cppdb::result res = statement.row();

		//if the result isn't empty( user was found ) AND the passed password matches the row read
		return !res.empty() && pass == res.get<string>( 0 );
	}
	catch( const cppdb::cppdb_error & e )
	{
		std::cout << e.what() << '\n';
		return false;
	}
}

// hash_salt_password(const string & user, const string & pass)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/

string auth::hash_salt_password( const string & user, const string & pass )
{
	return string();
}
