using IlmHub04.Models;
using IlmHub04.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data.Context;
using Project.Data.Entities;

namespace IlmHub04.Controllers;

[Authorize]
public class ProjectController : Controller
{

        private readonly AppDbContext _context;
        private readonly UserProvider _userProvider;

        const string projectKey = "qwe1234";

        public ProjectController(AppDbContext context, UserProvider userProvider)
        {
            _context = context;
            _userProvider = userProvider;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(project => project.User)
                .ToListAsync();

            return View(projects);
        }

    [Authorize]
    [HttpGet]
    public IActionResult CreateProject(string? key)
    {
        if (key is not null && projectKey == key)
        {
            return View();
        }

        return RedirectToAction("CheckProjectKey");
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromForm] CreatProjectDto creatProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return View(creatProjectDto);
        }

        var project = new Project.Data.Entities.Project()
        {
            Name = creatProjectDto.Name,
            Description = creatProjectDto.Description,
            SourceCodeLink = creatProjectDto.SourceCodeLink,
            ProjectWebLink = creatProjectDto.ProjectWebLink,
            UserId = _userProvider.UserId
        };

        if (creatProjectDto.Photo != null)
        {
            project.PhotoUrl = await FileSaver.SaveSchoolFile(creatProjectDto.Photo);
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> GetById(Guid projectId)
    {
        var project = await _context.Projects
            .Include(p => p.User)
            .ThenInclude(c => c.Comments)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        return View(project);
    }

    public async Task<IActionResult> AddComment(Guid projectId, [FromForm] CreateComment commentDto)
    {
        var comment = new ProjectComment()
        {
            Comment = commentDto.Comment,
            UserId = _userProvider.UserId,
            ProjectId = projectId
        };

        _context.ProjectComments.Add(comment);

        await _context.SaveChangesAsync();

        return RedirectToAction("GetById",new {projectId});
    }
























    [HttpGet]
    public IActionResult CheckProjectKey()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CheckProjectKey(ProjectKeyDto projectKeyDto)
    {
        string key = projectKeyDto.ProjKey;
        if (!ModelState.IsValid )
        {
            ModelState.AddModelError("ProjKey", "invalid input");
            return View(projectKeyDto);
        }

        if (projectKeyDto.ProjKey == projectKey)
        {
            return RedirectToAction(nameof(CreateProject), new {key = key} );
        }


        ModelState.AddModelError("ProjKey", "Kalit noto'g'ri");
        return View(projectKeyDto);

    }

    public IActionResult Comment()
    {
        return View();
    }
}