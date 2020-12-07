using System.Threading.Tasks;
using Refit;
using CargaImagensNASA.Models;

namespace CargaImagensNASA.HttpClients
{
    public interface IImagemDiariaAPI
    {
        [Get("/apod")]
        Task<InfoImagemNASA> GetInfoAsync(string api_key, string date);
    }
}