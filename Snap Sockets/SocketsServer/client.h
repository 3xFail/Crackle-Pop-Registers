/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: client.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma once
#pragma region includes
#include "../SharedCode/message.h"
#include "participant.h"
#include "client_handler.h"
#include <boost/asio.hpp>
using boost::asio::ip::tcp;
using boost::system::error_code;
using boost::asio::buffer;
using boost::asio::async_read;
using boost::asio::async_write;

#include <cppdb\frontend.h>

#include <iostream>
using std::cout;
#include <string>
using std::string;
using std::stringstream;
#include <deque>
using std::deque;
#include <memory>
using std::shared_ptr;
#include <set>
using std::set;
#include "sql_singleton.h"
#pragma endregion

/************************************************************************
* Class: client, inherits from: public participant, public std::enable_shared_from_this<client>
*
* Purpose:
*
* Manager functions:	
*
*	Constructor client(tcp::socket socket, client_handler & room, int id, string user)
*		Modifiers: public    
*		Description:
*
*
* Operator overloads:
*
*
* Methods:	
*	
*	void start()
*		Modifiers: public    
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
*	void read_username()
*		Modifiers: private    
*		Description:
*
*	void write()
*		Modifiers: private    
*		Description:
*
*	void parse_message()
*		Modifiers: private    
*		Description:
*
*
* Data Members:
*
*	tcp::socket _socket 
*		modifiers: private  
*
*	client_handler& _connected_clients 
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
class client: public participant, public std::enable_shared_from_this<client>
{
public:
	client( tcp::socket socket, client_handler & room, int id, string user );

	void start();
	void deliver( const message & msg ) override;

private:
	void read_header();
	void read_id();
	void read_body();
	void read_username();
	void write();
	void parse_message();

	tcp::socket _socket;
	client_handler& _connected_clients;
	message _cur_msg;
	deque<message> _msg_queue;
	int _id;
	string _username;
};
