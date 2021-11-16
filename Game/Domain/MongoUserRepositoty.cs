using System;
using MongoDB.Driver;

namespace Game.Domain
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> userCollection;
        public const string CollectionName = "users";

        public MongoUserRepository(IMongoDatabase database)
        {
            userCollection = database.GetCollection<UserEntity>(CollectionName);
            var options = new CreateIndexOptions { Unique = true };
            userCollection.Indexes.CreateOne(Builders<UserEntity>.IndexKeys.Ascending(u => u.Login), options);
        }

        public UserEntity Insert(UserEntity user)
        {
            //TODO: Ищи в документации InsertXXX.
            userCollection.InsertOne(user);
            return user;
        }

        public UserEntity FindById(Guid id)
        {
            //TODO: Ищи в документации FindXXX
            return userCollection.Find(ue => ue.Id == id).SingleOrDefault();
        }

        public UserEntity GetOrCreateByLogin(string login)
        {
            //TODO: Это Find или Insert

            var userEntity = userCollection.FindSync(ue => ue.Login == login).FirstOrDefault();
            if (userEntity == null)
            {
                var newUser = new UserEntity(Guid.NewGuid()) { Login = login };
                Insert(newUser);
                return newUser;
            }
            return userEntity;
        }

        public void Update(UserEntity user)
        {
            //TODO: Ищи в документации ReplaceXXX
            userCollection.ReplaceOne(ue => ue.Id == user.Id, user);
        }

        public void Delete(Guid id)
        {
            userCollection.DeleteOne(ue => ue.Id == id);
        }

        // Для вывода списка всех пользователей (упорядоченных по логину)
        // страницы нумеруются с единицы
        public PageList<UserEntity> GetPage(int pageNumber, int pageSize)
        {
            //TODO: Тебе понадобятся SortBy, Skip и Limit
            var totalCount = userCollection.CountDocuments(ue => true);
            var users = userCollection
                .Find(ue => true)
                .SortBy(ue => ue.Login)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToList();
            return new PageList<UserEntity>(
                users, totalCount, pageNumber, pageSize);
        }

        // Не нужно реализовывать этот метод
        public void UpdateOrInsert(UserEntity user, out bool isInserted)
        {
            throw new NotImplementedException();
        }
    }
}