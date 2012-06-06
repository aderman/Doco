namespace Docum.Lib.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    // Todo : Unit Test

    /// <summary>
    /// The Repository interface contains common methods for all model classes.
    /// </summary>
    /// <typeparam name="T">
    /// T is model classes.
    /// </typeparam>
    public interface IMongoRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// This method adds a new record to database.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// If it have a unique record in database, it returns null. Or else it returns added item.
        /// </returns>
        T Insert(T item);
        T Save(T item, bool existsControl = false);
        T GetItemById(ObjectId id);
        void Drop();
        MongoCursor<T> GetAll();
        MongoCursor<T> GetItemsByQuery(QueryComplete queryComplete);
    }

    /// <summary>
    /// The Repository Class contains common methods for all model classes.
    /// </summary>
    /// <typeparam name="T">
    /// T is model classes.
    /// </typeparam>
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        /// <summary>
        /// The server object
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// The db object.
        /// </summary>
        private readonly MongoDatabase _mongoDatabase;

        /// <summary>
        /// The collection object
        /// </summary>
        private readonly MongoCollection<T> _mongoCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{T}"/> class.
        /// </summary>
        public MongoRepository()
        {
            var con = new MongoConnectionStringBuilder(Helper.GetConString(Helper.DataBaseName, string.Empty));
            _server = MongoServer.Create(con);
            _mongoDatabase = _server.GetDatabase(con.DatabaseName);
            _mongoCollection = _mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
        }

        public T Insert(T item)
        {
            // Todo : Cache Object Infos 
            if (this.ExistsControl(item))
            {
                return null;
            }

            _mongoCollection.Insert(item);
            return item;
        }

        private bool ExistsControl(T item)
        {
            var queryList = new List<IMongoQuery>();
            var props = item.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {
                var flag = Attribute.GetCustomAttribute(propertyInfo, typeof(BsonExists));
                if (flag != null)
                {
                    queryList.Add(Query.EQ(propertyInfo.Name, BsonValue.Create(propertyInfo.GetValue(item, null))));
                }
            }

            var list = this.GetItemsByQuery(Query.And(queryList.ToArray()));
            if (list == null)
            {
                return false;
            }

            if (list.Count() > 1)
            {
                return true;
            }

            if (list.Count() == 1)
            {
                var p1 = list.Single().GetType().GetProperties().Single(x => x.PropertyType == typeof(ObjectId));
                var p2 = item.GetType().GetProperties().Single(x => x.PropertyType == typeof(ObjectId));
                var v1 = p1.GetValue(list.Single(), null);
                var v2 = p2.GetValue(item, null);
                if (!v1.Equals(v2))
                {
                    return true;
                }
            }

            return false;
        }

        public T GetItemById(ObjectId id)
        {
            return _mongoCollection.FindOneById(id);
        }

        public MongoCursor<T> GetItemsByQuery(QueryComplete queryComplete)
        {
            return _mongoCollection.FindAs<T>(queryComplete);
        }

        public T Save(T item, bool existsControl = false)
        {
            if (existsControl)
            {
                if (this.ExistsControl(item))
                {
                    return null;
                }
            }

            _mongoCollection.Save(item);
            return item;
        }

        public void Drop()
        {
            _mongoCollection.Drop();
        }

        public MongoCursor<T> GetAll()
        {
            return _mongoCollection.FindAllAs<T>();
        }


    }
}