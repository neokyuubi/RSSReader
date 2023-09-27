﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSSReader.Data;
using RSSReader.Models;

namespace RSSReader.Controllers
{
    public class FeedsController : Controller
    {
        private readonly DBContext dBContexto;

        public FeedsController(DBContext dBContext)
        {
            dBContexto = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var feeds = await dBContexto.Feeds.ToListAsync();
            return View(feeds);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Articles(Guid id)
        {
            var feed = await dBContexto.Feeds.FirstOrDefaultAsync(feed => feed.Id == id);

            // return await Task.Run(() => View("View", viewModel));
            return View("Articles", feed);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFeedViewModel addFeedRequest)
        {
            var feed = new Feed()
            {
                Id = Guid.NewGuid(),
                Name = addFeedRequest.Name,
                Url = addFeedRequest.Url,
            };

            await dBContexto.Feeds.AddAsync(feed);
            await dBContexto.SaveChangesAsync();

            //return View();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var feed = await dBContexto.Feeds.FirstOrDefaultAsync(feed => feed.Id == id);
            if (feed != null)
            {
                var viewModel = new UpdateFeedViewModel()
                {
                    Id = feed.Id,
                    Name = feed.Name,
                    Url = feed.Url,
                };
                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateFeedViewModel updateFeedViewRequest)
        {
            var feed = await dBContexto.Feeds.FindAsync(updateFeedViewRequest.Id);
            if (feed != null)
            {
                feed.Name = updateFeedViewRequest.Name;
                feed.Url = updateFeedViewRequest.Url;

                await dBContexto.SaveChangesAsync();
            }

            return RedirectToAction("Index"); // show error page if no feed is found
        }


        [HttpPost]
        public async Task<IActionResult> Delete(UpdateFeedViewModel updateFeedViewRequest)
        {
            var feed = await dBContexto.Feeds.FindAsync(updateFeedViewRequest.Id);
            if (feed != null)
            {
                dBContexto.Feeds.Remove(feed);
                await dBContexto.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index"); // show error page if no feed is found
        }
    }
}
