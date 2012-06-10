using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Docum.Lib.Models;
using Docum.Lib.MongoDb;
using FluentValidation;

namespace Docum.Lib.Services
{
    public interface IActivitiyLogService : IMongoService
    {
        void AddNewLogRecord(ActivityLog log);
        void Message(ActivityLog.ActivityType type,params  object[] p);
    }

    public class ActivitiyLogService : MongoService, IActivitiyLogService
    {
        private readonly IMongoRepository<ActivityLog> _mongoDb;

        public ActivitiyLogService(IMongoRepository<ActivityLog> mongoDb )
        {
            _mongoDb = mongoDb;
        }

        public void AddNewLogRecord(ActivityLog log)
        {
            ValidationResult = log.Validation();
            if ( ValidationResult.IsValid)
                _mongoDb.Insert(log);
        }

        public void Message(ActivityLog.ActivityType type,params object [] p )
        {
            var log = new ActivityLog()
                          {
                              LogContent = ActivityLogHelper.ApplyMessage(type, p),
                              AType = type,
                              LogTime = DateTime.Now, 
                              UserName = "Empty"
                          };
            AddNewLogRecord(log);
        }
    }
}
