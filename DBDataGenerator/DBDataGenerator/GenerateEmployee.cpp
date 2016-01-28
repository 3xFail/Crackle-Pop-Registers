#include "GenerateEmployee.h"

GenerateEmployee::GenerateEmployee()
{
}

GenerateEmployee::GenerateEmployee(string usernameFile, string numberFile, string addressFile) :m_usernameFile(usernameFile), m_numberFile(numberFile), m_addressFile(addressFile)
{}

void GenerateEmployee::Process(int numberOfEmployees)
{
	ifstream usernames(m_usernameFile);
	if (usernames)
		cout << "Successfully opened username file" << endl;
	else
		cout << "Failed to open usernames file" << endl;

	ifstream numbers(m_numberFile);
	if (numbers)
		cout << "Successfully opened number file" << endl;
	else
		cout << "Failed to open number file" << endl;

	ifstream addresses(m_addressFile);
	if (addresses)
		cout << "Successfully opened address file" << endl;
	else
		cout << "Failed to open address file" << endl;


	ofstream output("output.txt");
	if (output)
		cout << "Successfully opened output file" << endl;
	else
		cout << "Failed to open output file" << endl;



	char *processing = new char[256];

	for (int idx = 0; idx < numberOfEmployees; idx++)
	{
		output << "EXECUTE [dbo].[AddUser] ";

		//Username
		usernames.getline(processing, 256, '\n');
		output << "\"" << processing << "\", ";

		//Phone number
		numbers.getline(processing, 256, '\n');
		output << processing << ", ";

		//Permissions (31 for now)
		output << "31, ";

		//Address-street
		addresses.getline(processing, 256, ',');
		output << "\"" << processing << ", ";

		//Address-city
		addresses.getline(processing, 256, ',');
		output << processing << ", ";

		//Address-state
		addresses.getline(processing, 256, ',');
		output << processing << ", ";

		//Address-zip
		addresses.getline(processing, 256, '\n');
		output << processing << "\"";

		//End line
		output << '\n';
		
	}
}

