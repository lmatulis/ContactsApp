using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using ContactsApp.Helpers;
using ContactsApp.Models;
using ContactsApp.Services.Interfaces;

namespace ContactsApp.Services
{
    public class ContactService : IContactService
    {

        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId)
        {
            //transform from DTO To nonDTO
            Contact newContact = new Contact()
            {
                FirstName = contactDTO.FirstName,
                LastName = contactDTO.LastName,
                Email = contactDTO.Email,
                PhoneNumber = contactDTO.PhoneNumber,
                Address1 = contactDTO.Address1,
                Address2 = contactDTO.Address2,
                City = contactDTO.City,
                State = contactDTO.State,
                ZipCode = contactDTO.ZipCode,
                BirthDate = contactDTO.BirthDate,
                Created = DateTimeOffset.Now,
                AppUserId = userId
            };

            //add image
            if (contactDTO.ImageURL!.StartsWith("data:"))
            {
                newContact.Image = UploadHelper.GetImageUpload(contactDTO.ImageURL);
            }
            //save to database
            Contact createdContact = await _repository.CreateContactAsync(newContact);

            //add categories
            IEnumerable<int> categoryIds = contactDTO.Categories.Select(c => c.Id);

            //save these categories for the user to the database
            await _repository.AddCategoriesToContactAsync(createdContact.Id, categoryIds, userId);

            //return newContact
            return createdContact.ToDTO();
        }

        public async Task<ContactDTO?> GetContactByIdAsync(int Id, string userId)
        {
            Contact? contact = await _repository.GetContactByIdAsync(Id, userId);
            return contact!.ToDTO();
        }

        public async Task<List<ContactDTO>> GetContactsAsync(string userId)
        {
            List<Contact> contacts = await _repository.GetContactsAsync(userId);
            return contacts.Select(c => c.ToDTO()).ToList();
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsByCategoryId(int categoryId, string userId)
        {
            IEnumerable<Contact> contacts = await _repository.GetContactsByCategoryId(categoryId, userId);

            return contacts.Select(c =>c.ToDTO()).ToList();
        }

        public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
        {
            IEnumerable<Contact> contacts = await _repository.SearchContactsAsync(searchTerm, userId);

            return contacts.Select(c => c.ToDTO());
        }

        public async Task UpdateContactAsync(ContactDTO contactDTO, string userId)
        {
            Contact? contact = await _repository.GetContactByIdAsync(contactDTO.Id, userId);
            if(contact is not null)
            {
                contact.FirstName = contactDTO.FirstName;
                contact.LastName = contactDTO.LastName;
                contact.Email = contactDTO.Email;
                contact.PhoneNumber = contactDTO.PhoneNumber;
                contact.Address1 = contactDTO.Address1;
                contact.Address2 = contactDTO.Address2;
                contact.City = contactDTO.City;
                contact.State = contactDTO.State;
                contact.ZipCode = contactDTO.ZipCode;
                contact.BirthDate = contactDTO.BirthDate;
            }

            if (contactDTO.ImageURL.StartsWith("data:"))
            {
                contact.Image = UploadHelper.GetImageUpload(contactDTO.ImageURL);
            }
            else 
            {
                contact.Image = null;
            }

            contact.Categories.Clear();

            await _repository.UpdateContactAsync(contact, userId);

            //remove the old categories from the database
            await _repository.DeleteCategoriesFromContactAsync(contact.Id, userId);

            //Add back the new categories
            IEnumerable<int> selectedCategoryIds = contactDTO.Categories.Select(c => c.Id);
            await _repository.AddCategoriesToContactAsync(contact.Id, selectedCategoryIds, userId);

        }
    }
}
