using BlazingBlog.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services
{
    public class BlogPostService
    {
        private readonly BlogContext _context;
        public BlogPostService(BlogContext context)
        
        {
            _context = context;
        }
        public async Task<IEnumerable<BlogPost>> GetPostsAsync()=>
            await _context.BlogPosts
                            .AsNoTracking()
                            .ToListAsync();

        public async Task<MethodResult> SaveAsync(BlogSaveModel post, int userId) //id who making a reqeust and func SaveAsync สำหรับ save blog
        {
            var entity = post.ToBlogEntity(userId);
            
            if(entity.Id ==0)
            {
                //createing a new blog post
                entity.Slug = entity.Slug.Slugify();

                entity.CreatedOn = DateTime.Now;
                if(entity.IsPublished)
                {
                    entity.PublishedOn = DateTime.Now;
                }

                await _context.AddAsync(entity);
            }
            else
            {
                // updating an existing blog post
            }
            
            try
            {
                if(await _context.SaveChangesAsync()>0)
                {
                    return MethodResult.Succes();
                }
                else
                    return MethodResult.Failure("Unknow error while saving the blog post");
            }
            catch (Exception ex)
            {
                return MethodResult.Failure(ex.Message);
                throw;
            }

        }
    }
}