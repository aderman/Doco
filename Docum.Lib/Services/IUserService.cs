using Docum.Lib.Models;
using Docum.Lib.MongoDb;
using FluentValidation;
using MongoDB.Bson;

namespace Docum.Lib.Services
{
    public interface IUserService : IMongoService
    {
        DocoUser Insert(DocoUser user);

        bool Update(DocoUser user);

        bool Delete(DocoUser user);

        DocoUser SelectById(string id);

    }

    public class DocoUserService : MongoService, IUserService
    {
        private readonly IMongoRepository<DocoUser> _docoDb;

        public DocoUserService(IMongoRepository<DocoUser> docoDb)
        {
            _docoDb = docoDb;
        }

        public DocoUser Insert(DocoUser user)
        {

            ValidationResult = user.ValidateObject();
            if ( !ValidationResult.IsValid)
            {
                return null;
            }

            _docoDb.Insert(user);
            return user;
        }

        public bool Update(DocoUser user)
        {
            ValidationResult = user.ValidateObject();
            if (!ValidationResult.IsValid)
            {
                return false;
            }
            _docoDb.Save(user);
            return true;
        }

        public DocoUser SelectById(string id)
        {
            var objectId = ObjectId.Parse(id);
            return _docoDb.GetItemById(objectId);
        }

        public bool Delete(DocoUser user)
        {
            user.IsDeleted = true;
            return Update(user);
        }

        public void Drop()
        {
            _docoDb.Drop();
        }
    }

    public class DocoUserValidator : AbstractValidator<DocoUser>
    {
        public DocoUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
