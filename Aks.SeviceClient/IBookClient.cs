using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Aks.ServiceClient
{
    public interface IBookClient
    {
        [Get("/api/books")]
        Task<List<Book>> Get();
    }
}