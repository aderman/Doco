namespace Docum.Lib.Tests
{
    using System;
    using System.Linq;

    using Docum.Lib.MongoDb;

    using MongoDB.Driver.Builders;

    using NUnit.Framework;

    [TestFixture]
    public class MongoRepositoryTests
    {
        [Test]
        public void Insert_IfDontHaveTheSameRecord_ReturnItem()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Drop();
            var item = TestModel.CreateInstance();
            var res = repository.Insert(item);
            Assert.IsNotNull(res);
        }

        [Test]
        public void Insert_IfHaveTheSameRecord_ReturnNull()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Drop();
            var item = TestModel.CreateInstance();
            var item2 = TestModel.CreateInstance();
            var res = repository.Insert(item);
            var res2 = repository.Insert(item2);
            Assert.IsNull(res2);
        }

        [Test]
        public void Save_IfDontHaveTheSameRecord_ReturnItem()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Drop();
            var item = TestModel.CreateInstance();
            var res = repository.Insert(item);
            item.StringValue2 = "sinan";
            var res2 = repository.Save(item);
            Assert.IsNotNull(res2);
        }

        [Test]
        public void Save_IfHaveTheSameRecord_ReturnNull()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Drop();
            var item = TestModel.CreateInstance();
            repository.Insert(item);
            repository.Insert(new TestModel { StringValue = "sinan" });
            item.StringValue = "sinan";
            var res3 = repository.Save(item);
            Assert.IsNull(res3);
        }

        [Test]
        public void GetItemById_SendObjectId_ReturnItem()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Drop();
            var item = TestModel.CreateInstance();
            repository.Insert(item);
            var res = repository.GetItemById(item.Id);
            Assert.IsNotNull(res);
        }

        [Test]
        public void Drop_IfHaveCollection_ReturnTrue()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Insert(TestModel.CreateInstance());
            var result = repository.Drop();
            Assert.IsTrue(result);
        }

        [Test]
        public void Drop_IfDontHaveCollection_ReturnFalse()
        {
            var repository = new MongoRepository<TestModel>();
            var result = repository.Drop();
            Assert.IsFalse(result);
        }

        [Test]
        public void GetAll_InserTwoItem_ReturnTwo()
        {
            var repository = new MongoRepository<TestModel4>();
            repository.Drop();
            repository.Insert(new TestModel4());
            repository.Insert(new TestModel4());
            var list = repository.GetAll();
            Assert.AreEqual(2, list.Count());
        }

        [Test]
        public void GetItemsByQuery_InserThreeItem_ReturnTwo()
        {
            var repository = new MongoRepository<TestModel4>();
            repository.Drop();
            repository.Insert(new TestModel4 { StringValue = "sinan" });
            repository.Insert(new TestModel4 { StringValue = "sinan" });
            repository.Insert(new TestModel4 { StringValue = "ali" });
            var list = repository.GetItemsByQuery(Query.EQ("StringValue", "sinan"));
            Assert.AreEqual(2, list.Count());
        }

        [Test]
        public void AsQueryable_InserThreeItem_ReturnTwo()
        {
            var repository = new MongoRepository<TestModel4>();
            repository.Drop();
            repository.Insert(new TestModel4 { StringValue = "sinan" });
            repository.Insert(new TestModel4 { StringValue = "sinan" });
            repository.Insert(new TestModel4 { StringValue = "ali" });
            var list = repository.AsQueryable().Where(x => x.StringValue == "sinan");
            Assert.AreEqual(2, list.Count());
        }

        [Test]
        public void ExistsUniqueItem_OnlySelf_ReturnNull()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Drop();
            var item = new TestModel { StringValue = "sinan" };
            var item2 = new TestModel { StringValue = "sinan" };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsTrue(result);
        }

        [Test]
        public void ExistsUniqueItem_OnlySelf_ReturnItem()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Drop();
            var item = new TestModel { StringValue = "sinan" };
            var item2 = new TestModel { StringValue = "sinana" };
            var item3 = new TestModel { StringValue = "sinanx" };
            repository.Insert(item);
            repository.Insert(item2);
            var result = repository.ExistsUniqueItem(item3);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistsUniqueItem_WithOtherProperties_ReturnNull()
        {
            var repository = new MongoRepository<TestModel2>();
            repository.Drop();
            var item = new TestModel2 { StringValue = "sinan", StringValue2 = "akyazıcı", IntValue = 5 };
            var item2 = new TestModel2 { StringValue = "sinan", StringValue2 = "akyazıcı", IntValue = 5 };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsTrue(result);
        }

        [Test]
        public void ExistsUniqueItem_WithOtherProperties_ReturnItem()
        {
            var repository = new MongoRepository<TestModel2>();
            repository.Drop();
            var item = new TestModel2 { StringValue = "sinan", StringValue2 = "akyazıcı", IntValue = 5 };
            var item2 = new TestModel2 { StringValue = "sinan", StringValue2 = "akyazıcı", IntValue = 3 };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistsUniqueItem_WithOtherProperties_ReturnItem2()
        {
            var repository = new MongoRepository<TestModel2>();
            repository.Drop();
            var item = new TestModel2 { StringValue = "sinan", StringValue2 = "akyazıcı", IntValue = 5 };
            var item2 = new TestModel2 { StringValue = "sinan", StringValue2 = "akyazıcıc", IntValue = 5 };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistsUniqueItem_WithOtherProperties_ReturnItem3()
        {
            var repository = new MongoRepository<TestModel2>();
            repository.Drop();
            var item = new TestModel2 { StringValue = "sinan", StringValue2 = "akyazıcı", IntValue = 5 };
            var item2 = new TestModel2 { StringValue = "sinanx", StringValue2 = "akyazıcı", IntValue = 5 };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistsUniqueItem_SelfAndWithOtherProperties_ReturnNull()
        {
            var repository = new MongoRepository<TestModel3>();
            repository.Drop();
            var item = new TestModel3 { StringValue = "sinan", StringValue2 = "akyazıcı", DateTimeValue = DateTime.Parse("01/01/1967") };
            var item2 = new TestModel3 { StringValue = "sinan", StringValue2 = "akyazıcı", DateTimeValue = DateTime.Parse("01/01/1967") };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsTrue(result);
        }

        [Test]
        public void ExistsUniqueItem_SelfAndWithOtherProperties_ReturnNull2()
        {
            var repository = new MongoRepository<TestModel3>();
            repository.Drop();
            var item = new TestModel3 { StringValue = "sinan", StringValue2 = "akyazıcı", DateTimeValue = DateTime.Parse("01/01/1967") };
            var item2 = new TestModel3 { StringValue = "sinan", StringValue2 = "akyazıcıx", DateTimeValue = DateTime.Parse("02/01/1967") };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsTrue(result);
        }

        [Test]
        public void ExistsUniqueItem_SelfAndWithOtherProperties_ReturnItem()
        {
            var repository = new MongoRepository<TestModel3>();
            repository.Drop();
            var item = new TestModel3 { StringValue = "sinan", StringValue2 = "akyazıcı", DateTimeValue = DateTime.Parse("01/01/1967") };
            var item2 = new TestModel3 { StringValue = "sinanx", StringValue2 = "akyazıcıx", DateTimeValue = DateTime.Parse("02/01/1967") };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistsUniqueItem_WithOutBsonUnique_ReturnItem()
        {
            var repository = new MongoRepository<TestModel4>();
            repository.Drop();
            var item = new TestModel4 { StringValue = "sinan", StringValue2 = "akyazıcı", DateTimeValue = DateTime.Parse("01/01/1967") };
            var item2 = new TestModel4 { StringValue = "sinan", StringValue2 = "akyazıcıx", DateTimeValue = DateTime.Parse("02/01/1967") };
            repository.Insert(item);
            var result = repository.ExistsUniqueItem(item2);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistsUniqueItem_Own_ReturnItem()
        {
            var repository = new MongoRepository<TestModel>();
            repository.Drop();
            var item = TestModel.CreateInstance();
            repository.Insert(item);
            item.IntValue = 5;
            var result = repository.ExistsUniqueItem(item);
            Assert.IsFalse(result);
        }
    }
}