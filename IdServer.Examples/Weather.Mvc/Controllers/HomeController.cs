using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Weather.Mvc.Models;
using Newtonsoft.Json;
using Weather.Mvc.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace Weather.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
        private readonly ITokenService _tokenService;

    public HomeController(ILogger<HomeController> logger, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Weather()
    {
        using var client = new HttpClient();

        //var token = await _tokenService.GetToken("weatherapi.read");
        var token = await HttpContext.GetTokenAsync("access_token");

        //client.SetBearerToken(token.AccessToken);
        client.SetBearerToken(token);


        var result = await client.GetAsync("https://localhost:5705/weatherforecast");

        if (result.IsSuccessStatusCode)
        {
            var model = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<WeatherData>>(model);

            return View(data);
        }

        throw new Exception("Unable to get content");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
