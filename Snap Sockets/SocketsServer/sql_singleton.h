/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: sql_singleton.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
/************************************************************************
* Class: sql_singleton, inherits from: None
*
* Purpose:
*
* Manager functions:	
*
*	Constructor sql_singleton()
*		Modifiers: private    
*		Description:
*
*	Constructor sql_singleton(sql_singleton const & copy)
*		Modifiers: private    
*		Description:
*
*
* Operator overloads:
*
*	void operator=(sql_singleton const & copy)
*		Modifiers: private    
*		Description:
*
*
* Methods:	
*	
*	cppdb::session & instance()
*		Modifiers: public  static  
*		Description:
*
*
* Data Members:
*
*	cppdb::session sv 
*		modifiers: public static 
*
*	return sv 
*		modifiers: public  
*
*
*************************************************************************/
#pragma once
#include <cppdb\frontend.h>
class sql_singleton
{
public:
	static cppdb::session & instance()
	{
		static cppdb::session sv;
		return sv;
	}

private:
	sql_singleton() {}
	sql_singleton( sql_singleton const & copy ) = delete;
	void operator=( sql_singleton const & copy ) = delete;
};
