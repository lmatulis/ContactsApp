using ContactsApp.Models;

namespace ContactsApp.Services.Interfaces
{
    public interface IContactRepository
    {
        //create
        Task<Contact> CreateContactAsync(Contact contact);

        //read
        Task<List<Contact>> GetContactsAsync(string userId);
        Task<Contact?> GetContactByIdAsync(int Id, string userId);
        Task<IEnumerable<Contact>> GetContactsByCategoryId(int categoryId, string userId);
        Task<List<Contact>> SearchContactsAsync(string searchTerm, string userId);

        //update
        Task UpdateContactAsync(Contact contact, string userId);
        Task DeleteCategoriesFromContactAsync(int contactId, string userId);
        Task AddCategoriesToContactAsync(int contactId, IEnumerable<int> categoryIds, string userId);
        Task DeleteContactAsync(int Id, string userId);
    }
}
