using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using CSharp.Models;
using CSharp.Services.Interfaces;

namespace CSharp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class Orders : ControllerBase
	{ 
		IOrderService _service;

		public Orders(IOrderService orderService)
		{	
			_service = orderService;
		}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();

            return Ok(result);
        }

		[HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByOrderID(int orderId)
		{
            var result = await _service.Get(orderId);
			if (result == null)
			{
				return NotFound();
			}
			else
			{
				return Ok(result);
			}
		}

	    [HttpPost] //Create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(Order order)
		{
			//TODO: validation and return bad request if not
			//if(!order.isValid()
			// {
			// 	return BadRequest();
			// }
            var result = await _service.Create(order);

            return CreatedAtAction(
                nameof(GetByOrderID), 
                new { orderId = result.Id }, result);
         }

        [HttpPut]
		[Route ("{orderID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody]Order order, int orderID)
        {
			//TODO: validation and return bad request if not
			//if(!order.isValid()
			// {
			// 	return BadRequest();
			// }
			//TODO: less than trivial stuff to be idempotent
			if (order.Id != orderID)
			{
				return BadRequest();
			}
		
			var result = await _service.Update(order);
			return Ok(result);

        }
	
        [HttpGet("Customer/{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllByCustomerId(int customerId)
		{
			var result = await _service.GetAllByCustomerId(customerId);
			if (result.Count() == 0)
			{
				return NotFound();
			}
			else
			{
				return Ok(result);
			}
        }
		
		[HttpDelete("{OrderId}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int OrderId)
        {
			//Soft delete actually cancels the order?
            var result = await _service.Delete(OrderId);
			if (result)
			{
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

	}    

}
