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
        public async Task<IEnumerable<BlogPost>>GetPostsAsync(bool publishedOnly = false)
        {
            var query = _context.BlogPosts
                            .Include(bp => bp.Category)
                            .AsNoTracking();
            if(publishedOnly)
            {
                query = query.Where(bp => bp.IsPublished);
            }                
                           
            return await query.ToListAsync();
        }
            

         public async Task<BlogSaveModel?>GetPostAsync(int blogId)=>
            await _context.BlogPosts
                            .Include(bp=> bp.Category)
                            .AsNoTracking()
                            .Select(BlogSaveModel.Selector)
                            .FirstOrDefaultAsync(bp=> bp.Id == blogId);

        public async Task<MethodResult> SaveAsync(BlogSaveModel post, int userId) //id who making a reqeust and func SaveAsync สำหรับ save blog
        {
            
            
            if(post.Id ==0)
            {
                //createing a new blog post
                var entity = post.ToBlogEntity(userId);
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
                BlogPost? entity = await _context.BlogPosts
                                    .FirstOrDefaultAsync(bp=> bp.Id == post.Id);
                if (entity is not null)
                {
                    var wasBlogPostPublished = entity.IsPublished;
                    entity = post.Merge(entity);

                    entity.ModifiedOn = DateTime.Now;

                    if(entity.IsPublished)
                    {
                        if(wasBlogPostPublished)
                        {
                            //do nothing
                        }
                        else
                        {
                            //The blog post was not published in the db
                            // but user published it from the ui now
                            entity.PublishedOn = DateTime.Now;
                        }
                    }
                    else if(wasBlogPostPublished)
                    {
                        //This blog post waw published earlier in the db
                        //but user now nu-published it from the ui
                        entity.PublishedOn = null;
                    }
                    
                }
                else
                {
                    return MethodResult.Failure("This blog post does not exist");
                }
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