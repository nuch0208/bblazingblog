using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services
{
    public class CategoryService
    {
        private readonly BlogContext _context;

        public CategoryService(BlogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync() =>
            await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        public async Task <MethodResult>SaveCategoryAsync(Category model)
        {
            try
            {
                
                if(model.Id > 0)
            {
                //update category
            }
            else
            {
                //create category
                model.Slug = model.Slug.Slugify();
                await _context.Categories.AddAsync(model);
            }
            await _context.SaveChangesAsync();
            return MethodResult.Succes();
            }
            catch(Exception)
            {
                //log exception
                return MethodResult.Failure(null);
            }
        }

    
       
    }
}
