using API.DTOs.Stock;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        //private readonly IUnitOfWork _unitOfWork
        private readonly IMapper _mapper;
        private readonly IStockRepository _stockRepo;
        public StocksController(IStockRepository stockRepo, IMapper mapper) //IUnitOfWork unitOfWork
        {
            //_unitOfWork = unitOfWork;
            _stockRepo = stockRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<StockDTO>>> GetAllStocksAsync([FromQuery] QueryObject query)
        {
            try
            {
                var stocks = await _stockRepo.GetAllAsync(query);
                var response = _mapper.Map<List<StockDTO>>(stocks).ToList();
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetStockById")]
        public async Task<ActionResult<StockDTO>> GetStockByIdAsync([FromRoute] int id)
        {
            try
            {
                var stock = await _stockRepo.GetByIdAsync(id);
                if (stock == null)
                {
                    return NotFound("Stock not found.");
                }
                var response = _mapper.Map<StockDTO>(stock);
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewStockAsync([FromBody] CreateStockRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var stock = _mapper.Map<Stock>(request);
                await _stockRepo.CreateAsync(stock);
                var response = _mapper.Map<StockDTO>(stock);
                return CreatedAtAction("GetStockById", new { id = stock.Id }, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<StockDTO>> UpdateStockByIdAsync([FromRoute] int id, [FromBody] UpdateStockRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updateStock = await _stockRepo.UpdateAsync(id, request);
                if (updateStock == null)
                {
                    return NotFound("Stock not found.");
                }
                var response = _mapper.Map<StockDTO>(updateStock);
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> DeleteStockByIdAsync([FromRoute] int id)
        {
            try
            {
                var deleteStock = await _stockRepo.DeleteAsync(id);
                if (deleteStock == null)
                {
                    return NotFound("Stock not found.");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}