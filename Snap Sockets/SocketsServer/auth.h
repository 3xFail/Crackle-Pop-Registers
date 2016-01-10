/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: auth.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma once

#include "../SharedCode/scrypt-jane.h"
#include <string>
using std::string;
#include <iostream>
#include "sql_singleton.h"

/************************************************************************
* Class: auth, inherits from: None
*
* Purpose:
*
* Manager functions:	
*
*
* Operator overloads:
*
*
* Methods:	
*	
*	bool valid_login(const string & user, const string & pass)
*		Modifiers: public  static  
*		Description:
*
*	string hash_salt_password(const string & user, const string & pass)
*		Modifiers: public  static  
*		Description:
*
*
* Data Members:
*
*
*************************************************************************/
class auth
{
public:
	static bool valid_login( const string & user, const string & pass );
	static string hash_salt_password(const string & user, const string & pass );
private:
};
