using System;
using Docum.Lib.Models;
using Docum.Lib.MongoDb;
using Docum.Lib.Services;
using NUnit.Framework;

namespace Docum.Lib.Tests
{
    [TestFixture]
    public class DocoUserServiceTest
    {

        public static DocoUser CreateUser(string userName)
        {
            var user = new DocoUser
            {
                Email = "teloglu@gmail.com",
                Name = "Ali Derman",
                Surname = "TELOGLU",
                 UserName = userName,
                UserFolder = new DocoFolder()
                {
                    CreatedAt = DateTime.Now,
                    SavedAt = DateTime.Now,
                }
            };
            return user;
        }

        [Test]
        public void AddNewUser()
        {
            var user = CreateUser("ateloglu");
            var usrSrv = new DocoUserService(new MongoRepository<DocoUser>());
            usrSrv.Insert(user);
            Assert.IsTrue(usrSrv.ValidationResult.IsValid);
            
        }

        [Test]
        public void DeleteUser()
        {
            var user = CreateUser("ateloglu2");
            var usrSrv = new DocoUserService(new MongoRepository<DocoUser>());
            usrSrv.Insert(user);
            usrSrv.Delete(user);
            var user2 = usrSrv.SelectById(user.UserId.ToString());
            Assert.IsTrue(user2.IsDeleted);

        }
    }
}