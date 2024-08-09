using ContactsApp.Client.Models;

namespace ContactsApp.Client.Services.Interfaces
{
    public interface IContactService
    {
        Task<ContactDTO> CreateContactAsync(ContactDTO contact, string userId);
        /// <summary>
        /// Returns a ContactDTO that has been selected by a contactID
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ContactDTO?> GetContactByIdAsync(int Id, string userId);

        Task<List<ContactDTO>> GetContactsAsync(string userId);

        /// <summary>
        /// Updates a contact that belongs to the user
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateContactAsync(ContactDTO contact, string userId);

        /// <summary>
        /// Gets a list of contacts for a given category Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<ContactDTO>> GetContactsByCategoryId(int categoryId, string userId);

        /// <summary>
        /// Uses a search term to filter contacts from the database by that search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId);

    }
}
