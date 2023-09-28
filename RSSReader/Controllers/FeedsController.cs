using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSSReader.Data;
using RSSReader.Models;
using System.ServiceModel.Syndication;
using System.Xml;

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
            var feedObj = await dBContexto.Feeds.FirstOrDefaultAsync(feed => feed.Id == id);
            IEnumerable<Article>? articlesFeed = null;

            if (feedObj != null)
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetStringAsync(feedObj.Url);
                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse
                    };
                    using (var stringReader = new StringReader(response))
                    {
                        using (var xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
                        {
                            var feed = SyndicationFeed.Load(xmlReader);
                            if (feed != null)
                            {
                                var articles = feed.Items.Select(item => new Article
                                {
                                    Title = item.Title.Text,
                                    PubDate = item.PublishDate.DateTime.ToString(),
                                }).ToList();
                                articlesFeed = articles;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return View("Articles", articlesFeed);
                }

            }

            return View("Articles", articlesFeed);
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
        public async Task<IActionResult> Details(Guid id)
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
                return await Task.Run(() => View("Details", viewModel));
            }

            return RedirectToAction("Index");
        }

        // For Edit
        //[HttpPost]
        //public async Task<IActionResult> View(UpdateFeedViewModel updateFeedViewRequest)
        //{
        //    var feed = await dBContexto.Feeds.FindAsync(updateFeedViewRequest.Id);
        //    if (feed != null)
        //    {
        //        feed.Name = updateFeedViewRequest.Name;
        //        feed.Url = updateFeedViewRequest.Url;

        //        await dBContexto.SaveChangesAsync();
        //    }

        //    return RedirectToAction("Index"); // show error page if no feed is found
        //}


        [HttpPost]
        public async Task<IActionResult> Delete(UpdateFeedViewModel updateFeed, string[] checkboxes)
        {
            var feed = await dBContexto.Feeds.FindAsync(updateFeed.Id);
            if (feed != null)
            {
                dBContexto.Feeds.Remove(feed);
                await dBContexto.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index"); // show error page if no feed is found
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll(string[] selectedItems)
        {
            var selectedGuids = selectedItems.Select(Guid.Parse).ToArray();

            var feedsToDelete = await dBContexto.Feeds
                .Where(f => selectedGuids.Contains(f.Id))
                .ToListAsync();


            if (feedsToDelete != null && feedsToDelete.Any())
            {
                dBContexto.Feeds.RemoveRange(feedsToDelete);
                await dBContexto.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index"); // show error page if no feed is found
        }
    }
}
