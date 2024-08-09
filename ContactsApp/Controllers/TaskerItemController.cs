﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContactsApp.Client.Services.Interfaces;
using ContactsApp.Components.Account;
using ContactsApp.Client.Models;
using System.Collections;

namespace ContactsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskerItemController : ControllerBase
    {
        private readonly ITaskerItemService _taskerItemService;
        private string _userId => User.GetUserId()!;


        public TaskerItemController(ITaskerItemService taskerItemService)
        {
            _taskerItemService = taskerItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskerItemDTO>>> GetTaskerItemsAsync()
        {
            var taskerItems = await _taskerItemService.GetTaskerItemsAsync(_userId);
            return Ok(taskerItems);
        }

        [HttpPost]
        public async Task<ActionResult<TaskerItemDTO>> PostTaskerItemAsync([FromBody] TaskerItemDTO taskerItem)
        {
            TaskerItemDTO createdTaskerItem = await _taskerItemService.CreateTaskerItemAsync(taskerItem, _userId);
            return Ok(createdTaskerItem);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskerItemDTO>> GetTaskerItemByIdAsync([FromRoute] Guid id)
        {
            TaskerItemDTO? taskerItem = await _taskerItemService.GetTaskerItemByIdAsync(id, _userId);
            
            if(taskerItem == null)
            {
                return NotFound();
            }
            
            return Ok(taskerItem);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateTaskerItemAsync([FromRoute] Guid id, [FromBody] TaskerItemDTO taskerItem)
        {
            if(id != taskerItem.Id) { return BadRequest(); }

            if(await _taskerItemService.GetTaskerItemByIdAsync(id, _userId) == null)
            {
                NotFound();
            }

            await _taskerItemService.UpdateTaskerItemAsync(taskerItem, _userId);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteTaskerItemByIdAsync([FromRoute] Guid id)
        {
            await _taskerItemService.DeleteTaskerItemByIdAsync(id, _userId);
            return NoContent();
        }
    }
}
