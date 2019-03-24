using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;


namespace Aks.ServiceProvider.Services
{
    public class BookServiceDecorator : IBookService
    {
        private const string Key = "books";

        private readonly BookService _bookService;
        private readonly IDistributedCache _cache;

        public BookServiceDecorator(BookService bookService, IDistributedCache cache)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        public List<Book> Get()
        {

            var fromCache = _cache.Get(Key);
            if (fromCache != null)
            {
                var fromCacheContent = Encoding.UTF8.GetString(fromCache);
                var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(fromCacheContent);

                return books.ToList();
            }

            var list = _bookService.Get();
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list));
            _cache.Set(Key, bytes);

            return list;
        }

        public Book Get(string id)
        {
            return _bookService.Get(id);
        }

        public Book Create(Book book)
        {
            _cache.Remove(Key);
            return _bookService.Create(book);
        }

        public void Update(string id, Book bookIn)
        {
            _cache.Remove(Key);
            _bookService.Update(id, bookIn);
        }

        public void Remove(Book bookIn)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}