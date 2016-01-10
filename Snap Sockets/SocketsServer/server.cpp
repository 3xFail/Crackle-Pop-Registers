/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: server.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#include "server.h"

//Constructor server(io_service & service, unsigned short port)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
server::server( io_service & service, unsigned short port ): _acceptor( service, tcp::endpoint( tcp::v4(), port ) ), _socket( service )
{
	accept();
}

// accept()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void server::accept()
{
	_acceptor.async_accept( _socket, [this]( error_code ec )
	{
		if( !ec )
		{
			char buf[message::max_body_length] = { '\0' };
			_socket.read_some( buffer( buf, message::max_body_length ) );

			string user = strtok( buf, " " );
			string pass = strtok( nullptr, " " );
			//string ip = _socket.remote_endpoint().address().to_string();

			if( auth::valid_login( user, pass ) )
			{
				send_id(); //when a client connects initially, send them their ID

				std::make_shared<client>( std::move( _socket ), _room, cur_id, user )->start(); //set the socket in an asynchronous waiting state
				cout << user << " (id: " << cur_id << ") has connected to the server\n";
				cur_id++;
			}
			else
			{
				send_fail_auth();
				_socket.close();
			}

		}
		accept();
	} );
}

// send_id()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void server::send_id()
{
	message msg( "valid_login" );
	msg.encode_id( cur_id );

	_socket.write_some( buffer( msg.data(), msg.total_length() ) );
}

// send_fail_auth()
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void server::send_fail_auth()
{
	message msg( "invalid_login" );
	msg.encode_id( -1 );

	_socket.write_some( buffer( msg.data(), msg.total_length() ) );
}
