using ContactsApp.Data;
using ContactsApp.Models;
using ContactsApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Services
{
    public class ContactRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IContactRepository
    {
        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();
            return contact;
        }

        public async Task AddCategoriesToContactAsync(int contactId, IEnumerable<int> categoryIds, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            Contact? contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (contact is not null) 
            {
                foreach (int categoryId in categoryIds) 
                {
                    Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.AppUserId == userId);

                    if (category is not null) 
                    {
                        contact.Categories.Add(category);
                    }
                }

                await context.SaveChangesAsync();

            }
        }

        public async Task DeleteCategoriesFromContactAsync(int contactId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (contact is not null) 
            {
                contact.Categories.Clear();
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteContactAsync(int id, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.AppUserId == userId);

            if(contact != null)
            {
                context.Contacts.Remove(contact);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Contact>> GetContactsByCategoryId(int categoryId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            List<Contact> contacts = new List<Contact>();

            contacts = await context.Contacts.Include(c => c.Categories).Where(c => c.AppUserId == userId && c.Categories.Any(cat => cat.Id == categoryId)).ToListAsync();

            return contacts;
        }

        public async Task<Contact?> GetContactByIdAsync(int Id, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.Id == Id && c.AppUserId == userId);

            return contact;
        }

        public async Task<List<Contact>> GetContactsAsync(string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            List<Contact> contacts = await context.Contacts
                .Where(c => c.AppUserId == userId)
                .Include(c => c.Categories)
                .ToListAsync();
            
            return contacts;
        }

        public async Task<List<Contact>> SearchContactsAsync(string searchTerm, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            string normalizedSerachTerm = searchTerm.Trim().ToLower();
            List<Contact> contacts = await context.Contacts
                .Where(c => c.AppUserId == userId)
                .Include(c => c.Categories)
                .Where(c => string.IsNullOrEmpty(normalizedSerachTerm)
                    || c.FirstName!.ToLower().Contains(normalizedSerachTerm)
                    || c.LastName!.ToLower().Contains(normalizedSerachTerm)
                    || c.Categories.Any(cat => cat.Name!.ToLower().Contains(normalizedSerachTerm))
                ).ToListAsync();
            return contacts;
        }

        public async Task UpdateContactAsync(Contact contact, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool contactExists = await context.Contacts.AnyAsync(c => c.Id == contact.Id && c.AppUserId == userId);

            if (contactExists)
            {
                ImageUpload? oldImage = null;
                if (contact.Image is not null)
                {
                    //save the new image
                    context.Images.Add(contact.Image);

                    //check for the old image
                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == contact.ImageId);

                    //fix the foreign key or relink to the new contact image
                    contact.ImageId = contact.Image.Id;
                }
                context.Contacts.Update(contact);
                await context.SaveChangesAsync();

                if (oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
