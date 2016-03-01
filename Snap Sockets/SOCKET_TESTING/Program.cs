using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpClient;
using System.Threading;
namespace SOCKET_TESTING
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                connection_session connection = new connection_session("192.168.1.31\nnewgenstudios.duckdns.org\nlocalhost", 6119, "jacob.asmuth", "jacob.asmuth1234");
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
