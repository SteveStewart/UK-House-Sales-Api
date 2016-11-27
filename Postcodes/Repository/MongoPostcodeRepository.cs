using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postcodes.Domain;
using MongoDB.Driver;

namespace Postcodes.Repository
{
    /// <summary>
    /// A simple mongo postcode repository
    /// </summary>
    public class MongoPostcodeRepository : IPostcodeRepository
    {
        private readonly IMongoCollection<Postcode> _collection;

        public MongoPostcodeRepository(IMongoCollection<Postcode> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection is null");

            _collection = collection;
        }

        public Postcode GetById(int id)
        {
            var filter = Builders<Postcode>.Filter.Eq("_id", id.ToString());

            return _collection.Find(filter).FirstOrDefault();
        }

        public void Insert(IEnumerable<Postcode> items)
        {
            if (items == null)
                throw new ArgumentNullException("items is null");

            _collection.InsertMany(items);
        }

        public void Insert(Postcode item)
        {
            if (item == null)
                throw new ArgumentNullException("item is null");

            _collection.InsertOne(item);
        }

        public IEnumerable<Postcode> GetAll()
        {
            var filter = Builders<Postcode>.Filter.Empty;

            return _collection.Find(filter).ToList();
        }
    }
}
