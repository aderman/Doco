using Docum.Lib.Models;
using Docum.Lib.MongoDb;
using Docum.Lib.Services;
using NUnit.Framework;

namespace Docum.Lib.Tests
{
    [TestFixture]
    public class DocoUserServiceTest
    {

        [Test]
        public void AddNewUser()
        {
            var user = TestModel.CreateDocoUser("ateloglu");
            var usrSrv = new DocoUserService(new MongoRepository<DocoUser>());
            usrSrv.Insert(user);
            Assert.IsTrue(usrSrv.ValidationResult.IsValid);
        }

        [Test]
        public void DeleteUser()
        {
            var user = TestModel.CreateDocoUser("ateloglu2");
            var usrSrv = new DocoUserService(new MongoRepository<DocoUser>());
            usrSrv.Insert(user);
            usrSrv.Delete(user);
            var user2 = usrSrv.SelectById(user.UserId.ToString());
            Assert.IsTrue(user2.IsDeleted);
        }
    }
}