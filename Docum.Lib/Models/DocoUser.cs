using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Docum.Lib.Models
{

    #region DocoUser

    public class DocoUser
    {
        [BsonId]
        public ObjectId UserId { get; set; }

        [BsonRequired]
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public bool IsDeleted { get; set; }

        public DocoFolder UserFolder { get; set; }

        public DocoUser()
        {

            UserFolder = new DocoFolder {FolderName = "Root"};
        }
    }

    #endregion

    public class DocoDocument : HistoricalObject
    {
        [BsonId]
        public ObjectId DocId { get; set; }

        public DocType DType { get; set; }

        public Version DocVersion { get; set; }

        public enum DocType
        {
            Word
        }

        public bool IsPublic { get; set; }

        public bool IsCurrent { get; set; }

        public List<DocoAccess> EnabledUsers { get; set; }

        public List<string> Keywords { get; set; }

        public string Name
        {
            get;
            set;
        }

        public string DocContent{ get; set; }

        public DocoDocument()
        {
            EnabledUsers = new List<DocoAccess>();
            Keywords = new List<string>();
        }
    }

    public class DocoFolder : HistoricalObject
    {
        [BsonId]
        public ObjectId FolderId { get; set; }

        public string FolderName { get; set; }

        public string OwnerId { get; set; }

        public List<DocoDocument> ListOfDocuments { get; set; }

        public List<DocoFolder> Folders { get; set; }

        public DocoFolder()
        {
            Folders = new List<DocoFolder>();

            ListOfDocuments = new List<DocoDocument>();
        }
    }

    public class HistoricalObject
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? SavedAt { get; set; }
        public string LastSavedBy { get; set; }
    }


    public class DocoAccess
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public AccessType AType { get; set; }

        public enum AccessType
        {
            Read, Write
        }
    }

    public class Version
    {
        public int Major { get; set; }

        public int Minor { get; set; }

        public Version()
        {
            Major = 0;
            Minor = 0;
        }

        public Version(int major,int minor)
        {
            Major = major;
            Minor = minor;
        }

        public Version(Version v,bool justMinor)
        {
            Minor = v.Minor + 1;
            if (!justMinor)
                Major = v.Major + 1;
        }

        public override string ToString()
        {
            return String.Format("{0}.{1}", Major, Minor);
        }
    }

    public class ActivityLog
    {
        [BsonId]
        public ObjectId LogId { get; set; }

        public ActivityType AType { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string LogContent { get; set; }
        public DateTime LogTime { get; set; }

        public enum ActivityType
        {
            AddNewFolder,
            AddNewDocument,
            UpdateFolderName,
            UpdateDocumentName
        }

    }
}