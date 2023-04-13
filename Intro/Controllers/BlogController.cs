using Intro.Data;
using Intro.Models;
using Intro.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Intro.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public BlogVM BlogVM { get; set; }
        public BlogController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment= webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Blog> objlist = _db.Blog;
            foreach(var obj in objlist)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            };
            return View(objlist);
        }


        //Get - Upsert
        public IActionResult Upsert(int? id)
        {   
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            BlogVM blogVM = new BlogVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value),
                Blog = new Blog(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                })
            };

            if(id == null)
            {
                return View(blogVM);
            }
            else
            {
                blogVM.Blog = _db.Blog.Find(id);
                if (blogVM.Blog == null) 
                {
                    return NotFound();
                }
            }
            return View(blogVM);
        }

        //Post - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BlogVM blogVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if(blogVM.Blog.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    blogVM.Blog.Image = fileName + extension;

                    _db.Blog.Add(blogVM.Blog);
                }
                else
                {
                    //Updating
                    var objFromDb = _db.Blog.AsNoTracking().FirstOrDefault(u => u.Id == blogVM.Blog.Id);
                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        var oldFile = Path.Combine(upload,objFromDb.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        blogVM.Blog.Image = fileName + extension;
                    }
                    else
                    {
                        blogVM.Blog.Image = objFromDb.Image;
                    }
                    _db.Blog.Update(blogVM.Blog);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            blogVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            return View(blogVM);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Blog blog= _db.Blog.Include(u => u.Category).FirstOrDefault(u => u.Id == id);
            //blog.Category = _db.Category.Find(blog.CategoryId);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        //Post - Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Blog.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Blog.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
        }
    }
}