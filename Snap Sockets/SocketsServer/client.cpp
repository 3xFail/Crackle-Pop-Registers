/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: client.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#include "client.h"

//Constructor client(tcp::socket socket, client_handler & room, int id, string user)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
client::client( tcp::socket socket, client_handler & room, int id, string user ): _socket( std::move( socket ) ), _connected_clients( room ), _id( id ), _username( user )
{
}

// start()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void client::start()
{
	_connected_clients.join( shared_from_this() );
	read_header();
}

// deliver(const message & msg)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void client::deliver( const message & msg )
{
	if( msg.get_id() == _id ) //only deliver a message to the client that's expecting it
	{
		bool empty = _msg_queue.empty();
		_msg_queue.push_back( msg );
		if( empty ) //Otherwise the preceeding message in the queue will process this one.
			write();
	}
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
void client::read_header()
{
	async_read( _socket, buffer( _cur_msg.data(), message::header_length ), [this]( error_code ec, size_t len )
	{
		if( !ec && _cur_msg.decode_header() )
			read_id();
		else
			_connected_clients.leave( shared_from_this() );
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
void client::read_id()
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
			_connected_clients.leave( shared_from_this() );
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
void client::read_body()
{
	async_read( _socket, buffer( _cur_msg.body(), _cur_msg.body_length() ), [this]( error_code ec, size_t len )
	{
		if( !ec )
			read_username();
		else
			_connected_clients.leave( shared_from_this() );
	} );
}

// read_username()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void client::read_username()
{
	async_read( _socket, buffer( _cur_msg.username(), message::username_length ), [this]( error_code ec, size_t len )
	{
		if( !ec )
		{
			_cur_msg.decode_username();
			parse_message();
			read_header();
		}
		else
		{
			_connected_clients.leave( shared_from_this() );
		}
	} );
}

// parse_message()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void client::parse_message()
{
	if( _cur_msg.get_id() == _id && _cur_msg.get_username() == _username ) //wow real secure jacob
	{
		string body( _cur_msg.body(), _cur_msg.body_length() );

		stringstream total;
		if( body != "" )
		{
			cout << _cur_msg.get_username() << ": " << body << '\n';

			//print out SQL Query Result
			try
			{
				cppdb::result r = sql_singleton::instance() << body;
				while( r.next() ) //for each row in the result set
				{
					for( int i = 0; i < r.cols(); ++i ) //for each column in each row
					{
						string t;
						r >> t; //write the value to our stringstream
						total << t << ' ';
					}
					total << '\n';
				}
			}
			catch( cppdb::cppdb_error & e )
			{
				cout << e.what() << '\n';
			}

			_cur_msg.write( total.str().c_str() );
			cout << total.str();
			_connected_clients.deliver( _cur_msg );

		}
	}
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
void client::write()
{
	async_write( _socket, buffer( _msg_queue.front().data(), _msg_queue.front().total_length() ), [this]( error_code ec, size_t length )
	{
		if( !ec )
		{
			_msg_queue.pop_front();
			if( !_msg_queue.empty() )
				write();
		}
		else
		{
			_connected_clients.leave( shared_from_this() );
		}
	} );
}
