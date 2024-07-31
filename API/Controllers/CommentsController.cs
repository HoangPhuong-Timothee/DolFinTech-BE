using API.DTOs.Comment;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;
        public CommentsController(IFMPService fMPService, IMapper mapper, ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _stockRepo = stockRepo;
            _commentRepo = commentRepo;
            _userManager = userManager;
            _fmpService = fMPService;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<CommentDTO>>> GetAllComments([FromQuery] CommentQueyObject query)
        {
            try
            {
                var comments = await _commentRepo.GetAllAsync(query);
                var response = _mapper.Map<List<CommentDTO>>(comments);
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetCommentById")]
        public async Task<ActionResult<CommentDTO>> GetCommentById([FromRoute] int id)
        {
            try
            {
                var comment = await _commentRepo.GetByIdAsync(id);
                if (comment == null)
                {
                    return NotFound("Comment not found.");
                }
                var response = _mapper.Map<CommentDTO>(comment);
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        [Route("{symbol:alpha}")]
        [Authorize]
        public async Task<ActionResult<CommentDTO>> CreateCommentAsync([FromRoute] string symbol, [FromBody] CreateCommentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var stock = await _stockRepo.GetBySymbolAsync(symbol);
                if (stock == null)
                {
                    stock = await _fmpService.FindStockBySymbolAsync(symbol);
                    if (stock == null)
                    {
                        return BadRequest("Stock does not exists");
                    }
                    else
                    {
                        await _stockRepo.CreateAsync(stock);
                    }
                }
                var userName = User.GetUserName();
                var user = await _userManager.FindByNameAsync(userName);
                var comment = _mapper.Map<Comment>(request);
                comment.AppUserId = user.Id;
                await _commentRepo.CreateAsync(comment);
                var response = _mapper.Map<CommentDTO>(comment);
                return CreatedAtAction("GetCommentById", new { id = comment.Id }, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult> UpdateCommentAsync([FromRoute] int id, [FromRoute] UpdateCommentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updateComment = await _commentRepo.UpdateAsync(id, request);
                if (updateComment == null)
                {
                    return NotFound("Comment not found.");
                }
                var response = _mapper.Map<CommentDTO>(updateComment);
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult> DeleteCommentAsync([FromRoute] int id)
        {
            try
            {
                var deleteComment = await _commentRepo.DeleteAsync(id);
                if (deleteComment == null)
                {
                    return NotFound("Comment not found.");
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