/*****************************************************************
* Author: Jacob Asmuth
* Project: SharedCode
* Filename: message.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#include "message.h"

//Constructor message()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
message::message(): _body_length( 0 ), _username( "" ) {}

//Constructor message(const char * buf)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
message::message( const char * buf )
{
	write( buf );
}

//char* data()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
char * message::data()
{
	return _data;
}

//char* id()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
char * message::id()
{
	return _data + header_length;
}

//char* body()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
char* message::body()
{
	return _data + header_length + id_length;
}

//char* username()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
char * message::username()
{
	return _data + header_length + id_length + _body_length;
}

//size_t total_length()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
size_t message::total_length() const
{
#ifdef CLIENT
		return header_length + id_length + _body_length + username_length;
#elif SERVER
		return header_length + id_length + _body_length;
#endif
}

// body_length()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
size_t message::body_length() const
{
	return _body_length;
}

// body_length(size_t new_length)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void message::body_length( size_t new_length )
{
	_body_length = new_length > max_body_length ? max_body_length : new_length;
}

// write(const char * buf)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void message::write( const char * buf )
{
	body_length( strlen( buf ) );
	memcpy( body(), buf, _body_length );
	encode_header();
}

// decode_header()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
//when we decode it just take the bytes and convert them to an int. gotta love pointer gymnastics.
bool message::decode_header()
{
	_body_length = *(int *)_data;
	return _body_length >= 0;
}

// encode_header()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
//I know this looks like hell. Basically just write 8 bits at a time to the buffer
void message::encode_header()
{
	_data[3] = ( _body_length >> 24 ) & 0xFF;
	_data[2] = ( _body_length >> 16 ) & 0xFF;
	_data[1] = ( _body_length >> 8 ) & 0xFF;
	_data[0] = _body_length & 0xFF ;
}

// decode_id()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void message::decode_id()
{
	_id = *(int*)id();
}

// encode_id(int id)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void message::encode_id( int id )
{
	this->id()[3] = ( id >> 24 ) & 0xFF;
	this->id()[2] = ( id >> 16 ) & 0xFF;
	this->id()[1] = ( id >> 8 ) & 0xFF;
	this->id()[0] = id & 0xFF;
}

// decode_username()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void message::decode_username()
{
	char buf[message::username_length] = { '\0' };
	for( unsigned i = 0; i < int( message::username_length ); ++i )
		buf[i] = username()[i];
	_username = buf;
}

// encode_username(string username)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void message::encode_username( string username )
{
	memset( this->username(), '\0', message::username_length );
	for( unsigned i = 0; i <= (int)message::username_length && i < username.length(); ++i )
		this->username()[i] = username[i];
}

// get_id()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
int message::get_id() const
{
	return _id;
}

// get_username()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
string message::get_username() const
{
	return _username;
}
