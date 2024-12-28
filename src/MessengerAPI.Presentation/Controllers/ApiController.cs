using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Controllers;

[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
    protected ActionResult Problem(BaseApiError error)
    {
        return BadRequest(ApiErrorSchema.FromApiError(error));
    }
}

