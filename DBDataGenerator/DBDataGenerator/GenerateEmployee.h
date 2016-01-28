#include <string>
#include <iostream>
#include <fstream>


using std::string;
using std::cout;
using std::cin;
using std::ofstream;
using std::ifstream;
using std::endl;

#ifndef DBDATAGENERATOR_GENERATEEMPLOYEE_H
#define DBDATAGENERATOR_GENERATEEMPLOYEE_H



class GenerateEmployee
{
public:
	GenerateEmployee();
	GenerateEmployee(string usernameFile, string numberFile, string addressFile);
	
	void Process(int numberOfEmployees);




private:
	string m_usernameFile;
	string m_numberFile;
	string m_addressFile;
};











#endif // !DBDATAGENERATOR_GENERATEEMPLOYEE_H
