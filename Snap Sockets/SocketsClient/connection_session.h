/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsClient
* Filename: connection_session.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma once
#include "../SharedCode/message.h"
#include <boost/asio.hpp>
using boost::asio::io_service;
using boost::asio::ip::tcp;
using boost::system::error_code;
using boost::asio::buffer;
using boost::asio::async_write;
using boost::asio::async_read;
using boost::asio::async_connect;
#include <deque>
using std::deque;
#include <iostream>
using std::cout;
using std::cin;
#include <string>
using std::string;

/************************************************************************
* Class: connection_session, inherits from: None
*
* Purpose:
*
* Manager functions:	
*
*	Constructor connection_session(io_service & service, const string & host, const string & port, const string & username, const string & pass)
*		Modifiers: public    
*		Description:
*
*
* Operator overloads:
*
*
* Methods:	
*	
*	void write(const message & msg)
*		Modifiers: public    
*		Description:
*
*	void close()
*		Modifiers: public    
*		Description:
*
*	void connect(tcp::resolver::iterator endpoint_iterator, const string & pass)
*		Modifiers: private    
*		Description:
*
*	void read_header()
*		Modifiers: private    
*		Description:
*
*	void read_id()
*		Modifiers: private    
*		Description:
*
*	void read_body()
*		Modifiers: private    
*		Description:
*
*	void parse_message(const string & message)
*		Modifiers: private    
*		Description:
*
*	void write()
*		Modifiers: private    
*		Description:
*
*
* Data Members:
*
*	io_service & _service 
*		modifiers: private  
*
*	tcp::socket _socket 
*		modifiers: private  
*
*	message _cur_msg 
*		modifiers: private  
*
*	deque<message> _msg_queue 
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
class connection_session
{
public:
	connection_session( io_service & service, const string & host, const string & port, const string & username, const string & pass );

	void write( const message & msg );
	void close();

private:
	void connect( tcp::resolver::iterator endpoint_iterator, const string & pass );

	void read_header();
	void read_id();
	void read_body();
	void parse_message( const string & message );

	void write();

	io_service & _service;
	tcp::socket _socket;
	message _cur_msg;
	deque<message> _msg_queue;

	int _id;
	string _username;
};
