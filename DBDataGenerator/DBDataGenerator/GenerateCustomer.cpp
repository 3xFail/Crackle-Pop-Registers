#include "GenerateCustomer.h"

GenerateCustomer::GenerateCustomer()
{
	cout << "Enter name file name: ";
	cin >> m_nameFile;
}

GenerateCustomer::GenerateCustomer(string nameFile, string addressFile, string numberFile ):m_nameFile(nameFile), m_addressFile(addressFile), m_numberFile(numberFile)
{}

void GenerateCustomer::Process(int numberOfCustomers)
{
	ifstream names(m_nameFile);
	if (names)
		cout << "Successfully opened names file" << endl;
	else
		cout << "Failed to open names file" << endl;

	ifstream addresses(m_addressFile);
	if (addresses)
		cout << "Successfully opened addresses file" << endl;
	else
		cout << "Failed to open addresses file" << endl;

	ifstream numbers(m_numberFile);
	if (numbers)
		cout << "Successfully opened numbers file" << endl;
	else
		cout << "Failed to open numbers file" << endl;


	ofstream output("output.txt");

	if (output)
		cout << "Successfully opened output file" << endl;
	else
		cout << "Failed to open output file" << endl;




	char *processing = new char[256];

	for (int idx = 0; idx < numberOfCustomers; idx++)
	{
		


		output << "EXECUTE [dbo].[AddCust] \"";
		//First name
		names.getline(processing, 256, ',');
		output << processing << " ";
		
		//Last name
		names.getline(processing, 256, '\n');
		output << processing << "\", ";
		

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
		output << processing << "\", ";


		//Phone number
		numbers.getline(processing, 256, '\n');
		output << " " << processing << ", ";

		//User ID & end line
		output << "NULL\n";
	}
}

//void GenerateCustomer::generateNames(int numberOfNames)
//{
//	ifstream firstNames(m_firstNameFile);
//
//	if (firstNames)
//		cout << "Successfully opened firstnames file" << endl;
//	else
//		cout << "Failed to open firstnames file" << endl;
//
//	ifstream lastNames(m_lastNameFile);
//
//	if (lastNames)
//	{
//		cout << "Successfully opened lastnames file" << endl;
//	}
//	else
//		cout << "Failed to open lastnames file" << endl;
//
//
//
//	deque<string> allFirstNames;
//
//	while (!firstNames.eof())
//	{
//		
//	}
//
//}


