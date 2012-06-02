using Docum.Lib.Models;
using Docum.Lib.MongoDb;
using Docum.Lib.Services;
using NUnit.Framework;

namespace Docum.Lib.Tests
{
    [TestFixture]
    public class DocumentServiceTests
    {

        [Test]
        public void UpdateDocument()
        {
            var userRepo = new DocoUserService(new MongoRepository<DocoUser>());
            var folderService = new FolderService(userRepo, new ActivitiyLogService(new MongoRepository<ActivityLog>()));
            var documentService = new DocumentService(userRepo, new ActivitiyLogService(new MongoRepository<ActivityLog>()));
            var user = DocoUserServiceTest.CreateUser("adt");
            userRepo.Drop();
            
            userRepo.Insert(user);

            var f1  = folderService.AddNewFolder(user, "f1");

            var doc1 = folderService.AddNewDocument(user,f1.FolderId.ToString());
            var doc2 = folderService.AddNewDocument(user,f1.FolderId.ToString());
            var doc3 = folderService.AddNewDocument(user,f1.FolderId.ToString());
            var doc4 = folderService.AddNewDocument(user,f1.FolderId.ToString());

            doc3.DocContent = "ccccc";
            documentService.UpdateDocument(user, doc3);


        }
    }
}