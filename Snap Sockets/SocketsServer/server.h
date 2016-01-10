/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: server.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma once

#include "client.h"
#include "auth.h"
#include <boost/asio.hpp>
using boost::asio::ip::address;
using boost::asio::io_service;
using boost::asio::ip::tcp; //tcp master race
using boost::system::error_code;
using boost::asio::buffer;

#include <cppdb\frontend.h>

#include "db_conn_str.h"

#include <vector>
using std::vector;
#include <string>
using std::string;

/************************************************************************
* Class: server, inherits from: None
*
* Purpose:
*
* Manager functions:	
*
*	Constructor server(io_service & io_service, unsigned short port)
*		Modifiers: public    
*		Description:
*
*
* Operator overloads:
*
*
* Methods:	
*	
*	void accept()
*		Modifiers: private    
*		Description:
*
*	void send_id()
*		Modifiers: private    
*		Description:
*
*	void send_fail_auth()
*		Modifiers: private    
*		Description:
*
*
* Data Members:
*
*	tcp::acceptor _acceptor 
*		modifiers: private  
*
*	tcp::socket _socket 
*		modifiers: private  
*
*	client_handler _room 
*		modifiers: private  
*
*	int cur_id = 1
*		modifiers: private  
*
*
*************************************************************************/
class server
{
public:
	server( io_service & io_service, unsigned short port );

private:
	void accept();
	void send_id();
	void send_fail_auth();

	tcp::acceptor _acceptor;
	tcp::socket _socket;
	client_handler _room;
	int cur_id = 1;
};
