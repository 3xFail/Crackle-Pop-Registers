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
                connection_session connection = new connection_session("FRPC", 6119, "a", "a");
                    
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }
            //FIGURE OUT WHY THIS ISN'T WORKING

        }
    }
}
