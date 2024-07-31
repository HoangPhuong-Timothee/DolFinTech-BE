using API.DTOs.Stock;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace API.Services
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public readonly IMapper _mapper;
        public FMPService(HttpClient httpClient, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _httpClient = httpClient;
            _mapper = mapper;
        }
        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var fmpStock = tasks[0];
                    if (fmpStock != null)
                    {
                        var response = _mapper.Map<Stock>(fmpStock);
                        return response;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}