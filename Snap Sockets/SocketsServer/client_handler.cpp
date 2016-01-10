/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: client_handler.cpp
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#include "client_handler.h"

// join(shared_ptr<participant> _cl)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void client_handler::join( shared_ptr<participant> _cl )
{
	participants_.insert( _cl );
	for( auto msg : recent_msgs_ )
		_cl->deliver( msg );
}

// leave(shared_ptr<participant> _cl)
/*****************************************************************
* Purpose:
*
* Entry:
*
* Exit:
*
****************************************************************/
void client_handler::leave( shared_ptr<participant> _cl )
{
	participants_.erase( _cl );
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
void client_handler::deliver( const message & msg )
{
	if( msg.get_id() > 0 ) //only deliver messages with valid id's
	{
		recent_msgs_.push_back( msg );

		if( MAX_MESSAGES > 0 )
			while( recent_msgs_.size() > MAX_MESSAGES )
				recent_msgs_.pop_front();

		for( auto participant : participants_ )
			participant->deliver( msg );
	}
}
