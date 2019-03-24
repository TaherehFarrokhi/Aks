using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Aks.ServiceProvider.Services
{
    public class BookService : IBookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IOptions<Connections> options)
        {
            var client = new MongoClient(options.Value.Mongo);
            var database = client.GetDatabase("Aks");
            _books = database.GetCollection<Book>("Books");
        }

        public List<Book> Get()
        {
            return _books.Find(book => true).ToList();
        }

        public Book Get(string id)
        {
            return _books.Find<Book>(book => book.Id == id).FirstOrDefault();
        }

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn)
        {
            _books.ReplaceOne(book => book.Id == id, bookIn);
        }

        public void Remove(Book bookIn)
        {
            _books.DeleteOne(book => book.Id == bookIn.Id);
        }

        public void Remove(string id)
        {
            _books.DeleteOne(book => book.Id == id);
        }
    }
}