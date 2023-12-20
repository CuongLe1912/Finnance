using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace URF.Core.Controller
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly DbContext _context;

        BaseController(DbContext context)
        {
            _context = context;
        }
    }
}
