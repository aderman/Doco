﻿namespace Docum.Lib.Models
{
    #region Using

    using System;
    using System.Collections.Generic;

    using Docum.Lib.MongoDb;

    using FluentValidation;
    using FluentValidation.Results;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    #endregion

    #region DocoUser

    /// <summary>
    /// DocoUser is docouser table in database. It stores the users.
    /// </summary>
    public class DocoUser : AbstractValidator<DocoUser>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocoUser"/> class. 
        /// This creates root folder as default. Its name is 'Root'.
        /// </summary>
        public DocoUser()
        {
            this.DocumentList = new List<DocoDocument>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Email. The user's email address. This must have a valid email address. 
        /// </summary>
        [BsonUnique]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsDeleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets Name. The user's name.  This cannot be empty.
        /// </summary>
        [BsonUnique(false, "surname")] 
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Surname. The user's surname.  This cannot be empty.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets UserId. This is Primary Key as Hash.
        /// </summary>
        [BsonId]
        public ObjectId UserId { get; private set; }

        /// <summary>
        /// Gets or sets UserName.  The user's username. This cannot be empty.
        /// </summary>
        [BsonUnique]
        public string UserName { get; set; }

        public List<DocoDocument> DocumentList { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The validations of user object.
        /// </summary>
        /// <returns>
        /// If user object is validated, it returns true.
        /// </returns>
        public ValidationResult Validation()
        {
            this.RuleFor(x => x.UserName).NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty();
            this.RuleFor(x => x.Surname).NotEmpty();
            this.RuleFor(x => x.Email).EmailAddress();
            return this.Validate(this);
        }

        #endregion
    }

    #endregion

    #region DocoDocument

    /// <summary>
    /// This depends on the user's folders. It stores the user's documents.
    /// </summary>
    [Serializable]
    public class DocoDocument : HistoricalObject 
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocoDocument"/> class.
        /// </summary>
        public DocoDocument()
        {
            this.DocId = ObjectId.GenerateNewId();
            this.EnabledUsers = new List<DocoAccess>();
            this.Keywords = new List<string>();
        }

        #endregion

        #region Enums

        /// <summary>
        /// It stores document types.
        /// </summary>
        public enum DocType
        {
            /// <summary>
            /// For word documents.
            /// </summary>
            Word = 0
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets DType. This is document type. Such as 'Word' etc.
        /// </summary>
        public DocType DType { get; set; }

        /// <summary>
        /// Gets or sets DocContent. This is document content.
        /// </summary>
        public string DocContent { get; set; }

        /// <summary>
        /// Gets DocId. This is Primary Key as Hash.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId DocId { get; private set; }

        public string Id { get { return DocId.ToString(); } }

        /// <summary>
        /// Gets or sets DocVersion. This is document version.
        /// </summary>
        public Version DocVersion { get; set; }

        /// <summary>
        /// Gets or sets EnabledUsers. List of users that have access to documents.
        /// </summary>
        public List<DocoAccess> EnabledUsers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsCurrent.
        /// </summary>
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsPublic.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets Keywords. The keywords are used during search in documents.
        /// </summary>
        public List<string> Keywords { get; set; }

        /// <summary>
        /// Gets or sets Name. This is document name or title. This cannot be empty.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The validations of document object.
        /// </summary>
        /// <returns>
        /// If document object is validated, it returns true.
        /// </returns>
 
        public ValidationResult Validation()
        {
            //this.RuleFor(x => x.Name).NotEmpty();
            //return this.Validate(this);
            return null;
        }

        #endregion
    }

    #endregion

    

    #region HistoricalClass

    /// <summary>
    /// This class contains the common properties of the model classes.
    /// </summary>
    /// <typeparam name="T">
    /// The Model Class
    /// </typeparam>
    public class HistoricalObject //: AbstractValidator<T>
       //  where T : class
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets CreatedAt.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets LastSavedBy.
        /// </summary>
        public string LastSavedBy { get; set; }

        /// <summary>
        /// Gets or sets SavedAt.
        /// </summary>
        public DateTime? SavedAt { get; set; }

        #endregion
    }

    #endregion

    #region DocoAccess

    /// <summary>
    /// This class stores the user to who have access to the documents.
    /// </summary>
    public class DocoAccess
    {
        #region Enums

        /// <summary>
        /// This is access type of users to access documents. e.g. Read, Write etc.
        /// </summary>
        public enum AccessType
        {
            /// <summary>
            /// Read Access
            /// </summary>
            Read = 0, 

            /// <summary>
            /// Write Access
            /// </summary>
            Write = 1, 

            /// <summary>
            /// Read and Write Access
            /// </summary>
            ReadAndWrite = 2
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets AType. The access type of user who will access document.
        /// </summary>
        public AccessType AType { get; set; }

        /// <summary>
        /// Gets or sets UserId. The name of the user who will access document.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets UserName. The name of the user who will access document.
        /// </summary>
        public string UserName { get; set; }

        #endregion
    }

    #endregion

    #region Version

    /// <summary>
    /// This class contains information about the document version.
    /// </summary>
    public class Version
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        public Version()
        {
            this.Major = 0;
            this.Minor = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Major. 
        /// </summary>
        public int Major { get; private set; }

        /// <summary>
        /// Gets Minor.
        /// </summary>
        public int Minor { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// This is used to increase version.
        /// </summary>
        /// <returns>
        /// Returns new Version.
        /// </returns>
        public Version IncreaseVersion()
        {
            this.Minor++;
            if (this.Minor > 10)
            {
                this.Minor = 0;
                this.Major++;
            }

            return this;
        }

        /// <summary>
        /// This is used to convert version to string.
        /// </summary>
        /// <returns>
        /// Returns Version. Major.Minor.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}", this.Major, this.Minor);
        }

        #endregion
    }

    #endregion

    #region ActivityLog

    /// <summary>
    /// This contains the activities of all process to store log.
    /// </summary>
    public class ActivityLog : AbstractValidator<ActivityLog>
    {
        #region Enums

        /// <summary>
        /// This specifies the types of performed activity.
        /// </summary>
        public enum ActivityType
        {
            /// <summary>
            /// Add a new folder
            /// </summary>
            AddNewFolder = 0, 

            /// <summary>
            /// Add a new document
            /// </summary>
            AddNewDocument = 1, 

            /// <summary>
            /// Update folder name
            /// </summary>
            UpdateFolderName = 2, 

            /// <summary>
            /// Update document name
            /// </summary>
            UpdateDocumentName = 3
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets AType. The type of performed activity.
        /// </summary>
        public ActivityType AType { get; set; }

        /// <summary>
        /// Gets or sets LogContent. The content of performed activity.
        /// </summary>
        public string LogContent { get; set; }

        /// <summary>
        /// Gets LogId.
        /// </summary>
        [BsonId]
        public ObjectId LogId { get; private set; }

        /// <summary>
        /// Gets or sets LogTime. The date of performed activity.
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// Gets or sets UserId. The name of the user who performed the activity.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets UserName. The name of the user who performed the activity.
        /// </summary>
        public string UserName { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The validations of activity object.
        /// </summary>
        /// <returns>
        /// If activity object is validated, it will return true.
        /// </returns>
        public ValidationResult Validation()
        {
            RuleFor(x => x.UserName).NotEmpty();
            return this.Validate(this);
        }

        #endregion
    }

    #endregion
}