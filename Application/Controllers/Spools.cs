using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class SpoolsController(IInputHandler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> UpdateAll() =>
        Ok(await handler.HandleAsync(new UpdateAllSpoolsInput()));

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateSpoolInput input) =>
        Ok(await handler.HandleAsync(input));
}
