using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.DTOs;

namespace Presentation.Controllers;
[ApiController]
[Route("/api")]
public class RootController : ControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public RootController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
    {
        var links = GetLinks();
        return Ok(links);
    }

    private List<Link> GetLinks()
    {
        var links = new List<Link>
        {
            new()
            {
                Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), values: new{ })!,
                Rel = "self",
                Method = "GET"
            },
            new()
            {
                Href = _linkGenerator.GetUriByName(HttpContext, "GetCompanies", values: new{ })!,
                Rel = "companies",
                Method = "GET"
            },
            new()
            {
                Href = _linkGenerator.GetUriByName(HttpContext, "CreateCompany", values: new{ })!,
                Rel = "create_company",
                Method = "POST"
            },
        };
        return links;
    }
}
