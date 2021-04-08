using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CSharp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class test : ControllerBase
	{ 
		[HttpGet]
		public string Get(string customerId = null)
		{
			return string.IsNullOrEmpty(customerId) ? "Success!" : "Customerid = " + customerId;
		}
	}
}
