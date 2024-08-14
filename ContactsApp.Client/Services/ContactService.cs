using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace ContactsApp.Client.Services
{
    public class ContactService : IContactService
    {

        private readonly HttpClient _httpClient;


        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        //create
        public async Task<ContactDTO> CreateContactAsync(ContactDTO contact, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/contacts", contact);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ContactDTO>() ?? throw new HttpRequestException("Invalid JSON received from server");

        }


        //delete
        public async Task DeleteContactAsync(int contactId, string userId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/contacts/{contactId}");
            response.EnsureSuccessStatusCode();
        }


        //email
        public async Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/contacts/{contactId}/email", emailData);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        //GetContactById
        public async Task<ContactDTO?> GetContactByIdAsync(int Id, string userId)
        {
            return await _httpClient.GetFromJsonAsync<ContactDTO>($"api/contacts/{Id}");
        }


        //GetContacts
        public async Task<List<ContactDTO>> GetContactsAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<ContactDTO>>("api/contacts") ?? [];
        }



        //GetContatcsByCategory
        public async Task<IEnumerable<ContactDTO>> GetContactsByCategoryId(int categoryId, string userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/contacts?categoryId={categoryId}") ?? [];
        }



        //search
        public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/contacts/search?query={searchTerm}") ?? [];
        }



        //update
        public async Task UpdateContactAsync(ContactDTO contact, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/contacts/{contact.Id}", contact);
            response.EnsureSuccessStatusCode();
        }
    }
}
