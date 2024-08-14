using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using ContactsApp.Components.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        private string _userId => User.GetUserId()!;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContacts([FromQuery] int? categoryId)
        {
            try
            {
                IEnumerable<ContactDTO> contacts = [];

                if (categoryId == null || categoryId == 0)
                {
                    contacts = await _contactService.GetContactsAsync(_userId);
                }
                else
                {
                    contacts = await _contactService.GetContactsByCategoryId(categoryId.Value, _userId);
                }
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ContactDTO>> GetContactById([FromRoute] int id)
        {
            try
            {
                ContactDTO? contact = await _contactService.GetContactByIdAsync(id, _userId);

                return contact == null ? NotFound() : Ok(contact);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }


        [HttpPost]
        public async Task<ActionResult<ContactDTO>> CreateContact([FromBody] ContactDTO contactDTO)
        {
            try
            {
                ContactDTO createdContact = await _contactService.CreateContactAsync(contactDTO, _userId);
                return CreatedAtAction(nameof(GetContactById), new { id = createdContact.Id }, createdContact);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateContact([FromRoute] int id, [FromBody] ContactDTO contact)
        {
            if (id != contact.Id) { return BadRequest(); }

            try
            {
                await _contactService.UpdateContactAsync(contact, _userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteContact([FromRoute] int id)
        {
            try
            {
                await _contactService.DeleteContactAsync(id, _userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }


        [HttpPost("{id:int}/email")]
        public async Task<ActionResult> EmailContact([FromRoute] int id, [FromBody] EmailData emailData)
        {
            try
            {
                bool success = await _contactService.EmailContactAsync(id, emailData, _userId);
                 
                return success ? Ok() : BadRequest();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> SearchContact([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("A search term is required!");
            }

            try
            {
                IEnumerable<ContactDTO> contacts = await _contactService.SearchContactsAsync(query, _userId);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }
    }
}
