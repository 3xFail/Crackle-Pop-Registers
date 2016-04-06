using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSales.Permissions;

namespace AccessLibraryTest
{
	[TestClass]
	public class EmployeeTest
	{

        [TestMethod]
        public void HasPermissionCheck()
        {
            ulong permission = 0;
            Permissions.SetPermission( ref permission, true, Permissions.CanProcessRefunds );
            Assert.IsTrue( Permissions.CheckPermissions( permission, Permissions.CanProcessRefunds ) );
        }
    }
}
