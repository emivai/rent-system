using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Exceptions;

namespace RentSystem.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController() 
        {
        
        }

        protected int GetUserId() 
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (userId == null)
            {
                throw new ForbiddenException("Access token is missing required \"UserId\" field.");
            }

            return int.Parse(userId);
        }
    }
}
