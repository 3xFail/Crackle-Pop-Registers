/*****************************************************************
* Author: Jacob Asmuth
* Project: SharedCode
* Filename: message.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma once

#include <cstring>
using std::strncat;
#include <string>
using std::string;
using std::size_t;

/************************************************************************
* Class: message, inherits from: None
*
* Purpose:
*
* Manager functions:	
*
*	Constructor message()
*		Modifiers: public    
*		Description:
*
*	Constructor message(const char * buf)
*		Modifiers: public    
*		Description:
*
*
* Operator overloads:
*
*
* Methods:	
*	
*	char * data()
*		Modifiers: public    
*		Description:
*
*	char * id()
*		Modifiers: public    
*		Description:
*
*	char * body()
*		Modifiers: public    
*		Description:
*
*	char * username()
*		Modifiers: public    
*		Description:
*
*	size_t total_length()
*		Modifiers: public const   
*		Description:
*
*	size_t body_length()
*		Modifiers: public const   
*		Description:
*
*	void body_length(size_t new_length)
*		Modifiers: public    
*		Description:
*
*	void write(const char * buf)
*		Modifiers: public    
*		Description:
*
*	bool decode_header()
*		Modifiers: public    
*		Description:
*
*	void encode_header()
*		Modifiers: public    
*		Description:
*
*	void decode_id()
*		Modifiers: public    
*		Description:
*
*	void encode_id(int id)
*		Modifiers: public    
*		Description:
*
*	void decode_username()
*		Modifiers: public    
*		Description:
*
*	void encode_username(string username)
*		Modifiers: public    
*		Description:
*
*	int get_id()
*		Modifiers: public const   
*		Description:
*
*	string get_username()
*		Modifiers: public const   
*		Description:
*
*
* Data Members:
*
*	enum { header_length = 4 }
*		modifiers: public  
*
*	enum { id_length = 4 }
*		modifiers: public  
*
*	enum { max_body_length = 200000 }
*		modifiers: public  
*
*	enum { username_length = 24 }
*		modifiers: public  
*
*	char _data[header_length + id_length + username_length + max_body_length] 
*		modifiers: private  
*
*	size_t _body_length 
*		modifiers: private  
*
*	int _id 
*		modifiers: private  
*
*	string _username 
*		modifiers: private  
*
*
*************************************************************************/
class message
{
public:
	enum { header_length = 4 };
	enum { id_length = 4 };
	enum { max_body_length = 200000 };
	enum { username_length = 24 };
	 
	message();
	message( const char * buf );

	char * data();
	char * id();
	char * body();
	char * username();

	size_t total_length() const;
	size_t body_length() const;
	void body_length( size_t new_length );

	void write( const char * buf );

	bool decode_header();
	void encode_header();

	void decode_id();
	void encode_id( int id );

	void decode_username();
	void encode_username( string username );

	int get_id() const;
	string get_username() const;

private:
	char _data[header_length + id_length + username_length + max_body_length];
	size_t _body_length;
	int _id;
	string _username;
};
