namespace Docum.Lib.MongoDb
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    #endregion

    #region IMongoRepository

    /// <summary>
    /// The Repository interface contains common methods for all model classes.
    /// </summary>
    /// <typeparam name="T">
    /// T is model classes.
    /// </typeparam>
    public interface IMongoRepository<T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// This method deletes collection in database.
        /// </summary>
        /// <returns>
        /// If The deletion process performs successfully, it will return true. Or else will return false.
        /// </returns>
        bool Drop();

        /// <summary>
        /// This method gets all records in database.
        /// </summary>
        /// <returns>
        /// The All records in database.
        /// </returns>
        MongoCursor<T> GetAll();

        /// <summary>
        /// This method performs search by ObjectId in database.
        /// </summary>
        /// <param name="id">
        /// The id. The object Id you want to search.
        /// </param>
        /// <returns>
        /// The found records by The object id.
        /// </returns>
        T GetItemById(ObjectId id);

        /// <summary>
        /// This method performs search by the query that you made.
        /// </summary>
        /// <param name="queryComplete">
        /// The query complete. 
        /// </param>
        /// <returns>
        /// The found records by the query.
        /// </returns>
        MongoCursor<T> GetItemsByQuery(QueryComplete queryComplete);

        /// <summary>
        /// This method adds a new record to database.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// If it have a unique record in database, it will return null. Or else it will return added item.
        /// </returns>
        T Insert(T item);

        /// <summary>
        /// This method updates the existing object in database or adds a new record to database.
        /// </summary>
        /// <param name="item">
        /// The item. The updated object.
        /// </param>
        /// <returns>
        /// If it have a unique record in database, it will return null. Or else it will return added item.
        /// </returns>
        T Save(T item);

        #endregion
    }

    #endregion

    #region MongoRepository

    /// <summary>
    /// The Repository Class contains common methods for all model classes.
    /// </summary>
    /// <typeparam name="T">
    /// T is model classes.
    /// </typeparam>
    public class MongoRepository<T> : IMongoRepository<T>
        where T : class
    {
        #region Fields

        /// <summary>
        /// The collection object
        /// </summary>
        private readonly MongoCollection<T> _mongoCollection;

        /// <summary>
        /// The db object.
        /// </summary>
        private readonly MongoDatabase _mongoDatabase;

        /// <summary>
        /// The server object
        /// </summary>
        private readonly MongoServer _server;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{T}"/> class.
        /// </summary>
        public MongoRepository()
        {
            var con = new MongoConnectionStringBuilder(Helper.GetConString(Helper.DatabaseName, string.Empty));
            this._server = MongoServer.Create(con);
            this._mongoDatabase = this._server.GetDatabase(con.DatabaseName);
            this._mongoCollection = this._mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// This method enables us to search with Linq or Lambda Expression.
        /// </summary>
        /// <returns>
        /// The IQueryable
        /// </returns>
        public IQueryable<T> AsQueryable()
        {
            return this._mongoCollection.AsQueryable();
        }

        /// <summary>
        /// This method deletes collection in database.
        /// </summary>
        /// <returns>
        /// If The deletion process performs successfully, it will return true. Or else will return false.
        /// </returns>
        public bool Drop()
        {
            CommandResult res = this._mongoCollection.Drop();
            return res.Ok;
        }

        /// <summary>
        /// This method gets all records in database.
        /// </summary>
        /// <returns>
        /// The All records in database.
        /// </returns>
        public MongoCursor<T> GetAll()
        {
            return this._mongoCollection.FindAllAs<T>();
        }

        /// <summary>
        /// This method performs search by ObjectId in database.
        /// </summary>
        /// <param name="id">
        /// The id. The object Id you want to search.
        /// </param>
        /// <returns>
        /// The found records by The object id.
        /// </returns>
        public T GetItemById(ObjectId id)
        {
            return this._mongoCollection.FindOneById(id);
        }

        /// <summary>
        /// This method performs search by the query that you made.
        /// </summary>
        /// <param name="queryComplete">
        /// The query complete. 
        /// </param>
        /// <returns>
        /// The found records by the query.
        /// </returns>
        public MongoCursor<T> GetItemsByQuery(QueryComplete queryComplete)
        {
            return this._mongoCollection.FindAs<T>(queryComplete);
        }

        /// <summary>
        /// This method adds a new record to database.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// If it have a unique record in database, it will return null. Or else it will return added item.
        /// </returns>
        public T Insert(T item)
        {
            if (this.ExistsUniqueItem(item))
            {
                return null;
            }

            this._mongoCollection.Insert(item);
            return item;
        }

        /// <summary>
        /// This method updates the existing object in database or adds a new record to database.
        /// </summary>
        /// <param name="item">
        /// The item. The updated object.
        /// </param>
        /// <returns>
        /// If it have a unique record in database, it will return null. Or else it will return added item.
        /// </returns>
        public T Save(T item)
        {
            if (this.ExistsUniqueItem(item))
            {
                return null;
            }

            this._mongoCollection.Save(item);
            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method checks unique value in database.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// If have a unique value , it will return true. Or else will return false.
        /// </returns>
        internal bool ExistsUniqueItem(T item)
        {

            
            var queryList = new List<IMongoQuery>();
            PropertyInfo[] props = item.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in props)
            {
                object[] attr = propertyInfo.GetCustomAttributes(typeof(BsonUnique), false);
                if (attr.Length > 0)
                {
                    var self = (bool)attr[0].GetType().GetProperty("Self").GetValue(attr[0], null);
                    var names = (string[])attr[0].GetType().GetProperty("PropertyNames").GetValue(attr[0], null);
                    var temp = new List<IMongoQuery>();

                    if (self)
                    {
                        queryList.Add(Query.EQ(propertyInfo.Name, BsonValue.Create(propertyInfo.GetValue(item, null))));
                    }

                    if (names != null && names.Length > 0)
                    {
                        temp.Add(Query.EQ(propertyInfo.Name, BsonValue.Create(propertyInfo.GetValue(item, null))));
                        foreach (string name in names)
                        {
                            PropertyInfo prop = item.GetType().GetProperty(
                                name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                            temp.Add(Query.EQ(prop.Name, BsonValue.Create(prop.GetValue(item, null))));
                        }
                    }

                    if (temp.Count > 0)
                    {
                        queryList.Add(Query.And(temp.ToArray()));
                    }
                }
            }

            if (queryList.Count == 0)
            {
                return false;
            }

            MongoCursor<T> list = this.GetItemsByQuery(Query.Or(queryList.ToArray()));

            // The count must always be 1.
            if (list.Count() == 1)
            {
                PropertyInfo p1 = list.Single().GetType().GetProperties().Single(
                    x => x.PropertyType == typeof(ObjectId));
                PropertyInfo p2 = item.GetType().GetProperties().Single(x => x.PropertyType == typeof(ObjectId));
                object v1 = p1.GetValue(list.Single(), null);
                object v2 = p2.GetValue(item, null);
                if (!v1.Equals(v2))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }

    #endregion
}