#include <string>
#include <iostream>
#include <fstream>


using std::string;
using std::cout;
using std::cin;
using std::ofstream;
using std::ifstream;
using std::endl;

#ifndef DBDATAGENERATOR_GENERATECUSTOMER_H
#define DBDATAGENERATOR_GENERATECUSTOMER_H




class GenerateCustomer
{
public:
	GenerateCustomer();
	GenerateCustomer(string nameFile, string addressFile, string numberFile);

	void Process(int numberOfCustomers);

	//void generateNames(int numberOfNames);


private:

	string m_nameFile;
	string m_addressFile;
	string m_numberFile;


};



#endif // !DBDATAGENERATOR_GENERATENAMES_H
