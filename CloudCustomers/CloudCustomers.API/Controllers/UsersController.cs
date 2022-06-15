using CloudCustomers.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudCustomers.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    //private readonly ILogger<UsersController> _logger;

    public UsersController(IUsersService usersService)//ILogger<UsersController> logger)
    {
        _usersService = usersService;
        //_logger = logger;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> Get()
    {
        var users = await _usersService.GetAllUsers();

        if (users.Any())
        {
            return Ok(users);
        }

        return NotFound();
       
    }
}
