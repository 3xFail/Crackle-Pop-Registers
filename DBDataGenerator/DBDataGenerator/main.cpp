#include <iostream>
#include <fstream>
#include <string>
#include "GenerateCustomer.h"
#include "GenerateEmployee.h"

using std::cout;
using std::cin;
using std::endl;
using std::string;



int main()
{
	/*string delimiter;



	cout << "Enter delimiter: ";
	cin >> delimiter;
	
*/
	//GenerateCustomer customerGenerator(string("nameList.csv"), string("addressList.csv"), string("numberList.csv"));
	//customerGenerator.Process(1000);

	GenerateEmployee employeeGenerator(string("usernameList.csv"), string("numberList.csv"), string("addressList.csv"));
	employeeGenerator.Process(100);

	
	//generator.generateNames(5);






	//system("pause");
	return 0;
}