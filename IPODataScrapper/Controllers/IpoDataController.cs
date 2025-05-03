using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace IPODataScrapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IpoDataController : ControllerBase
    {
        private readonly ILogger<IpoDataController> _logger;

        public IpoDataController(ILogger<IpoDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetIpoData")]
        public IEnumerable<IpoGmpInfo> Get()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run headless if needed
            using var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.investorgain.com/report/live-ipo-gmp/331/all/");

            Thread.Sleep(5000); // Give time for JavaScript to load content

            var table = driver.FindElement(By.CssSelector("div.table-responsive table"));
            var rows = table.FindElements(By.CssSelector("tbody tr"));

            var ipoList = new List<IpoGmpInfo>();

            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count >= 13)
                {
                    var ipo = new IpoGmpInfo
                    {
                        IPO = cells[0].Text.Trim(),
                        Status = cells[1].Text.Trim(),
                        Price = cells[2].Text.Trim(),
                        GMP = cells[3].Text.Trim(),
                        EstListing = cells[4].Text.Trim(),
                        FireRating = cells[5].Text.Trim(),
                        IPOSize = cells[6].Text.Trim(),
                        Lot = cells[7].Text.Trim(),
                        Open = cells[8].Text.Trim(),
                        Close = cells[9].Text.Trim(),
                        BoADt = cells[10].Text.Trim(),
                        Listing = cells[11].Text.Trim(),
                        GMPUpdated = cells[12].Text.Trim()
                    };

                    ipoList.Add(ipo);
                }
            }

            driver.Quit();

            return ipoList;
        }

    }

    public class IpoGmpInfo
    {
        public string IPO { get; set; }
        public string Status { get; set; }
        public string Price { get; set; }
        public string GMP { get; set; }
        public string EstListing { get; set; }
        public string FireRating { get; set; }
        public string IPOSize { get; set; }
        public string Lot { get; set; }
        public string Open { get; set; }
        public string Close { get; set; }
        public string BoADt { get; set; }
        public string Listing { get; set; }
        public string GMPUpdated { get; set; }
    }
}