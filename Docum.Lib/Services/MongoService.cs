using System;
using System.Collections.Generic;
using Docum.Lib.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Docum.Lib.Services  
{
    public interface IMongoService
    {
        //private readonly IActivitiyLogService _activitiyLogService;  {Buraya tasinabilir.}
        ValidationResult ValidationResult { get; set; }
        void Traverse<T>(DocoFolder folder, string id, Func<T, string, bool> map, Action<T> reduce) where T : class;
    }

    public class MongoService : IMongoService 
    {
        public ValidationResult ValidationResult { get; set; }

        public  void Traverse<T>(DocoFolder folder, string id, Func<T, string, bool> map, Action<T> reduce) where T : class
        {
            if (typeof(T) == typeof(DocoFolder))
            {
                var obj = folder as T;
                if (map(obj, id))
                {
                    reduce(obj);
                    return;
                }
            }
            else
            {
                foreach (var document in folder.ListOfDocuments)
                {
                    var obj = document as T;
                    if (map(obj, id))
                    {
                        reduce(obj);
                        return;
                    }
                }
            }

            foreach (var f in folder.Folders)
            {
                Traverse(f, id, map, reduce);
                if (f.Folders.Count == 0)
                {
                    return;
                }
            }
            return;
        }
    }

    public class ActivityLogHelper
    {
        private static Dictionary<ActivityLog.ActivityType, string> _messages;

        public static  Dictionary<ActivityLog.ActivityType, string> Messages
        {
            get
            {
                if(_messages == null )
                {
                    _messages = new Dictionary<ActivityLog.ActivityType, string>();
                    BuildMessages();
                }
                return _messages;
            }
        }

        private static void BuildMessages()
        {
            _messages.Add(ActivityLog.ActivityType.AddNewDocument,"A new document added to {0}");
            _messages.Add(ActivityLog.ActivityType.AddNewFolder, "A new folder added to {0}");
            _messages.Add(ActivityLog.ActivityType.UpdateFolderName, "Folder named as {0}");
        }

        public static  string ApplyMessage(ActivityLog.ActivityType act, params object []  p )
        {
            return String.Format(Messages[act], p);
        }

    }
}