/*
 * Copyright (c) 2022. Gavin Pease and contributors.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT
 * OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

#nullable disable
using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    public class PostController : Controller
    {
        private readonly PostDbContext _context;

        public PostController(PostDbContext context)
        {
            _context = context;
        }

        // GET: Post
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(new List<Posts>(await _context.Posts.ToListAsync()));
        }

        // GET: Post/View/5
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }
            // Check if the post is not a draft, and the user is not an admin
            if (postModel.IsDraft && !User.IsInRole("Admin"))
            {
                return NotFound();
            }
            ViewData.Model = postModel;
            return View(postModel);
        }

        // GET: Post/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Content,IsDraft")] Posts posts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(posts);
                await _context.SaveChangesAsync();
                return RedirectToAction("View", new { id = posts.Id });
            }

            return View(posts);
        }

        // GET: Post/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts.FindAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,IsDraft")] Posts posts)
        {
            if (id != posts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostModelExists(posts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("View", new { id = id });
            }

            return View(posts);
        }

        // GET: Post/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postModel = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(postModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostModelExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}