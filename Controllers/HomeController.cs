#pragma warning disable CS8618
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ushtrimi_pergjithshem.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ushtrimi_pergjithshem.Controllers;
   public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        // Check to see if we got back null
        if(userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("SignIn", "Home", null);
        }
    }
}
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
       public IActionResult Index()
    {
        return View();
    }

    [HttpPost("Create")]
    public IActionResult CreateUser(User userFromView){
        if (ModelState.IsValid)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();   
            // Updating our newUser's password to a hashed version         
            userFromView.Password = Hasher.HashPassword(userFromView, userFromView.Password);
         _context.Add(userFromView);
         _context.SaveChanges();
         return RedirectToAction("SignIn");   
        }
        return View("Index");
    }
    [HttpGet("SignIn")]
    public IActionResult SignIn(){
        
        return View("Index");
    }
    [HttpPost("Login")]
    public IActionResult Login(SignIn userSubmission){

        if (ModelState.IsValid)
        {
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.SEmail);        
        // If no user ex
        if (userInDb == null)
        {   
            ModelState.AddModelError("Email", "Invalid Email");            
            return View("Index"); 
        }
        PasswordHasher<SignIn> hasher = new PasswordHasher<SignIn>();                    
        // Verify provided password against hash stored in db        
        var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.SPassword);   
         if(result == 0)        
        {            
             ModelState.AddModelError("Password", "Invalid Password");            
            return View("Index");       
        }
        int UserId = userInDb.UserId;
        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
        return RedirectToAction("Person");

            
        }
        

        return View("Index"); 
    }
    public IActionResult Person()
    {
        List<Person> people = _context.People
        .Include(p => p.Subscriptions)
            .ThenInclude(s => s.Magazine)
        .ToList();
        return View(people);
    }

    [HttpGet("magazine/view")]
    public IActionResult Magazine()
    {
        var magazines = _context.Magazines
        .Include(m => m.Readers)
            .ThenInclude(s => s.Person)
        .ToList();
        return View("Magazine", magazines);
    }

    [HttpGet("subscription/view")]
    public IActionResult Subscription()
    {
        var subscriptions = _context.Subscriptions
         .Include(s => s.Person)
         .Include(s => s.Magazine)
         .ToList();
        return View("Subscription", subscriptions);
    }

    [HttpGet("register/magazine")]
    public IActionResult RegisterMagazine(){
        return View();
    }
    [HttpGet("register/person")]
    public IActionResult Registerppl(){
        return View();
    }

    [HttpPost("add/person")]
     public IActionResult AddPerson(Person person){
        if(ModelState.IsValid){
            _context.Add(person);
            _context.SaveChanges();
            return RedirectToAction("Person");
        }
        else{
            return View ("Registerppl", person);
        }
    }
    [HttpGet("delete/person/{id}")]
    public IActionResult DeletePerson(int id){
        Person? person = _context.People.FirstOrDefault(e=> e.PersonId == id);
        _context.Remove(person);
        _context.SaveChanges();
        return RedirectToAction("Person");
    }


    [HttpPost("add/magazine")]
    public IActionResult AddMagazine(Magazine magazine){
        if(ModelState.IsValid){
            _context.Add(magazine);
            _context.SaveChanges();
            return RedirectToAction("Magazine");
        }
        else{
            return View ("RegisterMagazine",magazine);
        }
    }

    [HttpGet("delete/magazine/{id}")]
    public IActionResult DeleteMagazine(int id){
        Magazine? magazine = _context.Magazines.FirstOrDefault(e=> e.MagazineId == id);
        _context.Remove(magazine);
        _context.SaveChanges();
        return RedirectToAction("Magazine");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
