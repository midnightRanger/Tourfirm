using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tourfirm.DAL.Interfaces;

namespace Tourfirm.Controllers;

[Authorize(Roles="ADMIN,MODERATOR,MANAGER")]
public class CountryController : ControllerBase
{
    private readonly ILogger<CountryController> _logger;
    private readonly ICountry _countryRepository;

}