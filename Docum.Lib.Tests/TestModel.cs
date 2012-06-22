namespace Docum.Lib.Tests
{
    using System;
    using System.Collections.Generic;

    using Docum.Lib.Models;
    using Docum.Lib.MongoDb;

    using FluentValidation;
    using FluentValidation.Results;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class TestModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonUnique]
        public string StringValue { get; set; }

        public string StringValue2 { get; set; }

        public int IntValue { get; set; }

        public long LongValue { get; set; }

        public double DoubleValue { get; set; }

        public float FloatValue { get; set; }

        public decimal DecimalValue { get; set; }

        public DateTime DateTimeValue { get; set; }

        public bool BoolValue { get; set; }

        public char CharValue { get; set; }

        public byte ByteValue { get; set; }

        public List<string> StringList { get; set; }

        public TestModel()
        {
            StringList = new List<string>();
        }

        public static TestModel CreateInstance()
        {
            var item = new TestModel
                {
                    StringValue = "test",
                    StringValue2 = "test2",
                    IntValue = 1,
                    LongValue = 4294967296L,
                    DoubleValue = 1.7E+3, // 1700
                    FloatValue = 4.5f,
                    DecimalValue = 120m,
                    DateTimeValue = DateTime.Now,
                    BoolValue = true,
                    CharValue = 'S',
                    ByteValue = 255
                };

            return item;
        }

        public static DocoUser CreateDocoUser(string userName)
        {
            var user = new DocoUser
            {
                Email = "teloglu@gmail.com",
                Name = "Ali Derman",
                Surname = "TELOGLU",
                UserName = userName,
                };
            return user;
        }
    }

    public class TestModel2
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonUnique(false, "stringvalue2", "intvalue")]
        public string StringValue { get; set; }

        public string StringValue2 { get; set; }

        public int IntValue { get; set; }

        public long LongValue { get; set; }
    }

    public class TestModel3
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonUnique(true, "stringvalue2", "datetimevalue")]
        public string StringValue { get; set; }

        public string StringValue2 { get; set; }

        public DateTime DateTimeValue { get; set; }
    }

    public class TestModel4
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string StringValue { get; set; }

        public string StringValue2 { get; set; }

        public DateTime DateTimeValue { get; set; }
    }
}