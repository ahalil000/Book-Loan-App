using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

using Microsoft.Extensions.Logging;
using BookLoan.Data;
using BookLoan.Services;

using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData.Routing;


namespace BookLoan.Catalog.API.Controllers
{
    public class BookDataController: ODataController
    {
        ApplicationDbContext _db;
        IBookService _bookService;
        private readonly ILogger _logger;

        public BookDataController(ApplicationDbContext db,
            ILogger<BookDataController> logger,
            IBookService bookService)
        {
            _db = db;
            _logger = logger;
            _bookService = bookService;
        }

        [ODataRoute("Books")]
        [EnableQuery]
        public IActionResult GetBooks()
        {
            return Ok(_db.Books);
        }

        [ODataRoute]
        [EnableQuery] 
        public IActionResult Get()
        {
            return Ok(_db.Books);
        }

        [ODataRoute("Books({id})")]
        [EnableQuery]
        public IActionResult Get(int id)
        {
            return Ok(_db.Books.Where(b => b.ID == id));
        }


        public async Task<IActionResult> Patch2(int Id, Delta<BookViewModel> patch)
        {
            object id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (patch.GetChangedPropertyNames().Contains("Id") && patch.TryGetPropertyValue("Id", out id) && (int)id != Id)
            {
                return BadRequest("The key from the url must match the key of the entity in the body");
            }
            BookViewModel originalEntity = await this._db.Books.FindAsync(Id);
            if (originalEntity == null)
            {
                return NotFound();
            }
            else
            {
                patch.Patch(originalEntity);
                await this._db.SaveChangesAsync();
            }
            return Updated(originalEntity);
        }


        //[AcceptVerbs("PATCH", "MERGE")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<BookViewModel> patch)
        {
            object id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (patch.GetChangedPropertyNames().Contains("Id") && patch.TryGetPropertyValue("Id", out id) && (int)id != key)
            {
                return BadRequest("The key from the url must match the key of the entity in the body");
            }
            BookViewModel originalEntity = await this._db.Books.FindAsync(key);
            if (originalEntity == null)
            {
                return NotFound();
            }
            else
            {
                patch.Patch(originalEntity);
                await this._db.SaveChangesAsync();
            }
            return Updated(originalEntity);
        }
    }
}
