namespace Docum.Lib.MongoDb
{
    using System;

    /// <summary>
    /// This attribute uses to gain unique value. Use in only property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BsonUnique : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonUnique"/> class.
        /// </summary>
        /// <param name="self">
        /// The self. If you want to check only self unique value, you should set true.
        /// </param>
        /// <param name="propertyNames">
        /// The property names. This is 
        /// </param>
        public BsonUnique(bool self = false, params string[] propertyNames)
        {
            this.Self = self;
            this.PropertyNames = propertyNames;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonUnique"/> class.
        /// </summary>
        public BsonUnique()
        {
            this.Self = true;
        }

        /// <summary>
        /// Gets a value indicating whether Self.
        /// </summary>
        public bool Self { get; private set; }

        /// <summary>
        /// Gets PropertyNames. This is the names of the other properties that are required to obtain a unique value.
        /// </summary>
        public string[] PropertyNames { get; private set; }
    }
}