using Docum.Lib.Models;
using Docum.Lib.MongoDb;
using Docum.Lib.Services;
using NUnit.Framework;

namespace Docum.Lib.Tests
{
    [TestFixture]
    public class DocoFolderServiceTests
    {
        private DocoUserService _docoUserService;
        private FolderService _folderService;
        private DocoUser _user;

        [SetUp]
        public void SetUp()
        {
            _docoUserService = new DocoUserService(new MongoRepository<DocoUser>());
            _folderService = new FolderService(_docoUserService, new ActivitiyLogService(new MongoRepository<ActivityLog>()));
            _docoUserService.Drop();
            _user = TestModel.CreateDocoUser("sakyazici");
            _docoUserService.Insert(_user);
        }

       

        [Test]
        public void UpdateFolderName_True()
        {

            _folderService.AddNewFolder(_user, "testFolder");
            var folder = _user.UserFolder.Folders[0];
            _folderService.UpdateName(_user, folder.FolderId.ToString(), folder.FolderName + "update");
        }

        [Test]
        public void AddNewDocumentByFolderId()
        {
            _folderService.AddNewFolder(_user, "testFolder");
            var folder = _user.UserFolder.Folders[0];
            _folderService.AddNewDocument(_user, folder.FolderId.ToString());
        }

        
    }
}