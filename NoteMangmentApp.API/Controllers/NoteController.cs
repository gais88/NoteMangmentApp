using AutoMapper;
using Azure;
using Core.Models;
using Data.UnitWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteMangmentApp.API.Errors;
using NoteMangmentApp.API.Extensions;
using NoteMangmentApp.API.ViewModels;

namespace NoteMangmentApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NoteController> _logger;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public NoteController(IUnitOfWork unitOfWork, ILogger<NoteController> logger, IMapper mapper, LinkGenerator linkGenerator)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }
        // GET: api/<CityController>
        [HttpGet("GetAll")]
        public async Task<ActionResult<NoteVM[]>> GetAll()
        {
            var result = await _unitOfWork.NoteRepository.GetAllAsync(User.GetUserId());
            if (result == null)
            {
                return NotFound(new ApiResponse(404, "No Notes Found!"));
            }
            // replace for auto mapper in futuer 
            return _mapper.Map<NoteVM[]>(result);
        }
        [HttpPost]
        public async Task<ActionResult<CreateNoteVM>> Post(CreateNoteVM createVM)
        {
            var user = await _unitOfWork.AppUserRepository.GetByIdAsync(User.GetUserId());
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401, "User Not Found"));
            }
            var note = _mapper.Map<Note>(createVM);
            note.AppUserId = User.GetUserId();
            await _unitOfWork.NoteRepository.AddAsync(note);

            if (await _unitOfWork.SaveAsync())
            {

                return Ok(_mapper.Map<NoteVM>(note));
            }

            return BadRequest(new ApiResponse(400, "Failed to Add Note!"));
        }
        [HttpPut("{NoteId:int}")]
        public async Task<ActionResult<UpdateNoteVM>> Put(int NoteId, UpdateNoteVM updateNoteVM)
        {
            var user = await _unitOfWork.AppUserRepository.GetByIdAsync(User.GetUserId());
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401, "User Not Found"));
            }
            var note = await _unitOfWork.NoteRepository.GetByIdAsync(NoteId);
            if (note == null)
            {
                return BadRequest(new ApiResponse(400, "note Not Found!"));
            }

            _mapper.Map(updateNoteVM, note);
           
            _unitOfWork.NoteRepository.Update(note);

            if (await _unitOfWork.SaveAsync())
            {
                return _mapper.Map<UpdateNoteVM>(note);
            }

            return BadRequest(new ApiResponse(400, "Failed to Update Note!"));
        }

        [HttpDelete("{notetId:int}")]
        public async Task<ActionResult> Delete(int notetId)
        {
            var note = await _unitOfWork.NoteRepository.GetByIdAsync(notetId);
            if (note == null)
            {
                return BadRequest(new ApiResponse(400, "note Not Found!"));
            }

            _unitOfWork.NoteRepository.Delete(note);

            if (await _unitOfWork.SaveAsync())
            {
                return Ok("Deleted Successfully.");
            }

            return BadRequest(new ApiResponse(400, "Failed to Delete note!"));
        }
    }
}
