using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpClient;
namespace SOCKET_TESTING
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                connection_session connection = new connection_session("newgenstudios.duckdns.org", 6119, "a", "a");
                connection.write(string.Format("GetEmployee_Username \"{0}\"", "a"));
                
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }
            //FIGURE OUT WHY THIS ISN'T WORKING

        }
    }
}
