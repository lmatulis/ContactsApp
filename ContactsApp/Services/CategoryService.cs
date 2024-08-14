using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using ContactsApp.Models;
using ContactsApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ContactsApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IEmailSender _emailSender;

        //constructor
        public CategoryService(ICategoryRepository repository, IContactRepository contactRepository, IEmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
        }

        //create category
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            Category newCategory = new Category()
            {
                Name = category.Name,
                AppUserId = userId,
            };

            Category createdCategory = await _repository.CreateCategoryAsync(newCategory);
            return createdCategory.ToDTO();
        }

        public async Task DeleteCategoryAsync(int id, string userId)
        {
            await _repository.DeleteCategoryAsync(id, userId);
        }

        public async Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId)
        {
            try
            {
                Category? category = await _repository.GetCategoryByIdAsync(categoryId, userId);
                if (category == null || category.Contacts.Count < 1) { return false; }

                string recipients = string.Join(";", category.Contacts.Select(c => c.Email));

                await _emailSender.SendEmailAsync(recipients, emailData.Subject!, emailData.Message!);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        //get all categories
        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            IEnumerable<Category> categories = await _repository.GetCategoriesAsync(userId);
            //'select' acts like a foreach loop 
            IEnumerable<CategoryDTO> categoryDTOs = categories.Select(c => c.ToDTO());
            return categoryDTOs;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int Id, string userId)
        {
            Category? category = await _repository.GetCategoryByIdAsync(Id, userId);
            return category!.ToDTO();
        }

        public async Task UpdateCategoryAsync(CategoryDTO categoryDTO, string userId)
        {
            Category? category = await _repository.GetCategoryByIdAsync(categoryDTO.Id, userId);
            if (category != null) 
            {
                category.Contacts.Clear();
                category.Name = categoryDTO.Name;
                await _repository.UpdateCategoryAsync(category!, userId);
            }


        }
    }
}
