/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsClient
* Filename: connection_session.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#include "connection_session.h"

//Constructor connection_session(io_service& service, const string & host, const string & port, const string & username, const string & pass)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
connection_session::connection_session( io_service& service, const string & host, const string & port, const string & username, const string & pass ): _service( service ), _socket( service ), _id( -1 ), _username( username )
{
	tcp::resolver resolver( service );
	auto endpoint_iterator = resolver.resolve( { host, port } );
	connect( endpoint_iterator, pass );
}

// write(const message & msg)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void connection_session::write( const message & msg )
{
	_service.post( [this, msg]()
	{
		bool write_in_progress = !_msg_queue.empty();
		_msg_queue.push_back( msg );
		if( !write_in_progress )
			write();
	} );
}

// close()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void connection_session::close()
{
	_service.post( [this]() { _socket.close(); } );
}

// connect(tcp::resolver::iterator endpoint_iterator, const string & pass)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void connection_session::connect( tcp::resolver::iterator endpoint_iterator, const string & pass )
{
	//this lambda is called before each connection on the endpoint iterator
	//since we will always only connect to one location, we don't care about possible subsequent connections.
	//that's why we ignore the iterator variable
	async_connect( _socket, endpoint_iterator, [this, pass]( error_code ec, tcp::resolver::iterator )
	{
		//write the username and password to our connection attempt.
		_socket.write_some( buffer( _username + ' ' + pass ) );
		if( !ec )
			read_header();
	} );
}

// read_header()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void connection_session::read_header()
{
	async_read( _socket, buffer( _cur_msg.data(), message::header_length ), [this]( error_code ec, size_t len )
	{
		if( !ec && _cur_msg.decode_header() )
			read_id();
		else
			_socket.close();
	} );
}

// read_id()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void connection_session::read_id()
{
	async_read( _socket, buffer( _cur_msg.id(), message::id_length ), [this]( error_code ec, size_t len )
	{
		if( !ec )
		{
			_cur_msg.decode_id();
			read_body();
		}
		else
		{
			_socket.close();
		}
	} );
}

// read_body()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void connection_session::read_body()
{
	async_read( _socket, buffer( _cur_msg.body(), _cur_msg.body_length() ), [this]( error_code ec, size_t len )
	{
		if( !ec )
		{
			string message( _cur_msg.body(), _cur_msg.body_length() );
			parse_message( message );

			read_header();
		}
		else
		{
			_socket.close();
		}
	} );
}

// write()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void connection_session::write()
{
	_msg_queue.front().encode_id( _id ); //always attach our uniqueID to the message
	_msg_queue.front().encode_username( _username ); //always attach our undername to the end of the message
	async_write( _socket, buffer( _msg_queue.front().data(), _msg_queue.front().total_length() ), [this]( error_code ec, size_t len )
	{
		if( !ec )
		{
			_msg_queue.pop_front();
			if( !_msg_queue.empty() )
				write();
		}
		else
		{
			_socket.close();
		}
	} );
}

// parse_message(const string & message)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void connection_session::parse_message( const string & message )
{
	if( message == "valid_login" && _id == -1 )
		_id = _cur_msg.get_id();
	else if( message != "" )
		cout << message << '\n';
}
