using AutoMapper;
using Core.Models;
using Data.UnitWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoteManagmentApp.UI.Settings;
using NoteManagmentApp.UI.ViewModels;

namespace NoteManagmentApp.UI.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly UserManager<AppUser> _userManager;
        private readonly string _imagesPath;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NoteController(IUnitOfWork unitOfWork,
                              ILogger<HomeController> logger,
                              IMapper mapper,
                              LinkGenerator linkGenerator,
                              UserManager<AppUser> userManager,
                              IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}"; ;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = Convert.ToInt32(_userManager.GetUserId(HttpContext.User));
                var userNotes = await _unitOfWork.NoteRepository.GetAllAsync(userId);
                var model = _mapper.Map<IEnumerable<NoteVM>?>(userNotes);
                return View(model);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception occured: {ex.Message}");
                return View("Error");
            }
           
        }
        [HttpGet]
        public  IActionResult AddNote()
        {
            try
            {
                CreateNoteVM model = new();
                return View(model);
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                return View("Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(CreateNoteVM model)
        {
          
               if (!ModelState.IsValid)
                   return View(model);

            // save image 
             var image = await SaveImage(model.Image);
            // save entity 
            var note = _mapper.Map<Note>(model);
                var userId  = _userManager.GetUserId(HttpContext.User);
                note.AppUserId = Convert.ToInt32(userId);
                note.ImageUrl = image;
                await _unitOfWork.NoteRepository.AddAsync(note);

                if (await _unitOfWork.SaveAsync())
                {
                   

                    return RedirectToAction(nameof(Index));
                }
                return View("Error");

            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var note = await _unitOfWork.NoteRepository.GetByIdAsync(id);
                if (note == null)
                    return NotFound();
                var model = _mapper.Map<UpdateNoteVM>(note);
                return View(model);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception occured: {ex.Message}");
                return View("Error");
            }
        }
        [AllowAnonymous]
        public async  Task<IActionResult> View(int id)
        {
            try
            {
                var note = await _unitOfWork.NoteRepository.GetByIdAsync(id);
                if (note == null)
                    return NotFound();
                var model = _mapper.Map<UpdateNoteVM>(note);
                return View(model);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception occured: {ex.Message}");
                return View("Error");
            }
        }
            [HttpGet]
        public  IActionResult GenerateLink(int id)
        {
            try
            {
                var location =_linkGenerator.GetUriByAction(HttpContext,"View", "Note", values: new { id = id });
                ViewBag.link = location;
                return View();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception occured: {ex.Message}");
                return View("Error");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateNoteVM model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                var note = await _unitOfWork.NoteRepository.GetByIdAsync(model.Id);
                if (note is null) return NotFound();

                _mapper.Map(model, note);

                var hasNewCover = model.Image is not null;
                var oldCover = note.ImageUrl;
                if (hasNewCover)
                {
                    note.ImageUrl = await SaveImage(model.Image!);
                }
                 _unitOfWork.NoteRepository.Update(note);
                if (await _unitOfWork.SaveAsync())
                {
                    if (hasNewCover && !string.IsNullOrEmpty(oldCover))
                    {
                        var cover = Path.Combine(_imagesPath, oldCover);
                        System.IO.File.Delete(cover);
                    }


                }
                else
                {
                    var cover = Path.Combine(_imagesPath, note.ImageUrl!);
                    System.IO.File.Delete(cover);
                    return View("Error");

                }
                

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception occured: {ex.Message}");
                return View("Error");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var note = await _unitOfWork.NoteRepository.GetByIdAsync(id);
                if (note is null) return BadRequest();

                 _unitOfWork.NoteRepository.Delete(note);

                if (await _unitOfWork.SaveAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                BadRequest();
            }
                return BadRequest();

        }
        #region helper Method
        private async Task<string> SaveImage(IFormFile? cover)
        {
            if (cover != null)
            {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = System.IO.File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;

            }
            return string.Empty;
        }
        #endregion
    }
}
