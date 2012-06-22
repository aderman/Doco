using System;
using Docum.Lib.Models;

namespace Docum.Lib.Services
{
    public interface IDocumentService : IMongoService
    {
        void UpdateDocument(DocoUser user, DocoDocument document);
    }

    public class DocumentService : MongoService, IDocumentService
    {
        private readonly IActivitiyLogService _activitiyLogService;
        private readonly IUserService _userService;

        public DocumentService(IUserService userService, IActivitiyLogService activitiyLogService)
        {
            _userService = userService;
            _activitiyLogService = activitiyLogService;
        }

        public void UpdateDocument(DocoUser user, DocoDocument document)
        {
            //var validatorResult = document.Validation();
            //if (validatorResult.IsValid)
            //{
            //    Traverse<DocoDocument>(user.UserFolder,document.DocId.ToString(),(x,y)=>x.DocId.ToString() == y,x=>
            //        {
            //            x.DocContent=document.DocContent;
            //            x.LastSavedBy=user.UserName;
            //            x.SavedAt=DateTime.Now;
            //            x.DocVersion = x.DocVersion.IncreaseVersion();
            //        }
            //    );
            //    _userService.Update(user);
            //    _activitiyLogService.AddNewLogRecord(
            //        new ActivityLog
            //            {
            //                AType = ActivityLog.ActivityType.UpdateDocumentName,
            //                LogContent = "Updated Document Content",
            //                LogTime = DateTime.Now,
            //                UserName = user.UserName,
            //                UserId = user.UserId.ToString()
            //            });
            //}
        }
    }
}
