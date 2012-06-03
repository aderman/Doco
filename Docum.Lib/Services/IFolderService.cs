using System;
using Docum.Lib.Models;
using MongoDB.Bson;

namespace Docum.Lib.Services
{
    public interface IFolderService : IMongoService
    {
        DocoFolder AddNewFolder(DocoUser user, string folderName, DocoFolder parent = null);
        DocoDocument AddNewDocument(DocoUser user, string folderId);
        void UpdateName(DocoUser user, string folderId, string folderName);
    }

    public class FolderService : MongoService, IFolderService
    {
        private readonly IUserService _userService;
        private readonly IActivitiyLogService _activitiyLog;

        public FolderService(IUserService userService,IActivitiyLogService activitiyLog)
        {
            _userService = userService;
            _activitiyLog = activitiyLog;
        }

        public DocoFolder AddNewFolder(DocoUser user, string folderName, DocoFolder parent = null)
        {
            var folder = new DocoFolder()
                             {
                                 CreatedAt = DateTime.Now,
                                 SavedAt = DateTime.Now,
                                 FolderName = folderName,
                                 OwnerId = user.UserId.ToString()
                             };
            if (parent == null)
                parent = user.UserFolder;

            parent.Folders.Add(folder);
            _userService.Update(user);
            return folder;
        }

        private void UpdateRecursive(DocoFolder folder, string folderId, Action<DocoFolder> action)
        {
            if (folder.FolderId.ToString() == folderId)
            {
                action(folder);

                return;
            }
            foreach (var docoFolder in folder.Folders)
            {
                UpdateRecursive(docoFolder, folderId, action);
                if (docoFolder.Folders.Count == 0)
                {
                    return;
                }

            }
        }

        public void UpdateName(DocoUser user, string folderId, string folderName)
        {
            Traverse<DocoFolder>(user.UserFolder, folderId, (x, y) => x.FolderId.ToString() == folderId,(x) => x.FolderName = folderName );
            _userService.Update(user);
        }


        public DocoDocument AddNewDocument(DocoUser user, string folderId)
        {
            var document = new DocoDocument
                               {
                                   CreatedAt = DateTime.Now,
                                   DocVersion = new Models.Version(),
                                   DType = DocoDocument.DocType.Word,
                                   IsPublic = false,
                                   IsCurrent = true,
                                   Name = "New Document"
                               };

            Traverse<DocoFolder>(user.UserFolder,folderId, (x,y)=> x.FolderId.ToString() == folderId ,
                                                            x => x.ListOfDocuments.Add(document) 
                );

            
            _userService.Update(user);
            _activitiyLog.Message(ActivityLog.ActivityType.AddNewDocument,"New Document");
            return document;
        }
    }
}