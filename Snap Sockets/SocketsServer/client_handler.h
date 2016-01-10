/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: client_handler.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma once
#include "participant.h"
#include "../SharedCode/message.h"

#include <memory>
using std::shared_ptr;
#include <set>
using std::set;
#include <deque>
using std::deque;

/************************************************************************
* Class: client_handler, inherits from: None
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
*	void join(shared_ptr<participant> participant)
*		Modifiers: public    
*		Description:
*
*	void leave(shared_ptr<participant> participant)
*		Modifiers: public    
*		Description:
*
*	void deliver(const message & msg)
*		Modifiers: public    
*		Description:
*
*
* Data Members:
*
*	set<shared_ptr<participant>> participants_ 
*		modifiers: private  
*
*	deque<message> recent_msgs_ 
*		modifiers: private  
*
*	int MAX_MESSAGES = 50; //if this value less than 1, there are unlimited messages allowed in the queue.
*		modifiers: private static const
*
*
*************************************************************************/
class client_handler
{
public:
	void join( shared_ptr<participant> participant );
	void leave( shared_ptr<participant> participant );
	void deliver( const message & msg );

private:
	set<shared_ptr<participant>> participants_;
	deque<message> recent_msgs_;

	static const int MAX_MESSAGES = 50; //if this value less than 1, there are unlimited messages allowed in the queue.
};
