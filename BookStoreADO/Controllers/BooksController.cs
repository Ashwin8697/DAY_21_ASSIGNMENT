using BookStoreADO.Models;
using BookStoreADO.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreADO.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // READ
        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();

            return View(books);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookService.AddBook(book);

                return RedirectToAction("Index");
            }

            return View(book);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            _bookService.DeleteBook(id);

            return RedirectToAction("Index");
        }
    }
}