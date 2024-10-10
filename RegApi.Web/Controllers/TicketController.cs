using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;
using RegApi.Shared.Constants;
using System.Security.Claims;

namespace RegApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// Adds a new ticket.
        /// </summary>
        /// <param name="ticketModel">The model containing ticket details.</param>
        /// <returns>Returns a status indicating the result of the operation.</returns>
        [HttpPost]
        [Authorize(Policy = "OnlyAdminUsers")]
        public async Task<IActionResult> AddTicket([FromBody] TicketModel ticketModel)
        {
            var ticketId = await _ticketService.AddTicket(ticketModel);
            return StatusCode(201);
        }

        /// <summary>
        /// Retrieves tickets based on the user's role.
        /// </summary>
        /// <returns>Returns a list of tickets for the authenticated user.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = new List<TicketModel>();
            if (User.IsInRole(RoleNames.Admin))
            {
                tickets = (await _ticketService.GetAllAsync()).ToList();
            }
            else if (User.IsInRole(RoleNames.Visitor))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                tickets = (await _ticketService.GetForUserAsync(userId)).ToList();
            }

            return Ok(tickets);
        }

        /// <summary>
        /// Updates the status of a ticket.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to update.</param>
        /// <returns>Returns a status indicating the result of the operation.</returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateStatus([FromQuery] int ticketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _ticketService.UpdateStatus(ticketId, userId);
            return Ok(201);
        }
    }
}
