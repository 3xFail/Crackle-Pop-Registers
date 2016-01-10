/*****************************************************************
* Author: Jacob Asmuth
* Project: SocketsServer
* Filename: participant.h
* Date Created: 12/2/2015
* Modifications:
*
****************************************************************/
#pragma once
#include "../SharedCode/message.h"

/************************************************************************
* Class: participant, inherits from: None
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
*	void deliver(const message & msg)
*		Modifiers: public   virtual 
*		Description:
*
*
* Data Members:
*
*
*************************************************************************/
//I know it's tempting to remove this class.
//Don't. The inheritance is needed to keep the shared pointers happy.
class participant
{
public:
	virtual void deliver( const message & msg ) = 0;
};
