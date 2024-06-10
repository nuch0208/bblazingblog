using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services
{
    public class UserService
    {
        private readonly BlogContext _context;
        public UserService(BlogContext context)
        {
            _context = context;
        }

        public async Task<LoggedInUser?>LoginAsync(LoginModel model)
        {
            var dbUser = await _context.Users
                            .AsNoTracking() //เพื่อไม่ให้ _context เก็บค่า object เอาไว้ติดตามในระบบ
                            .FirstOrDefaultAsync(u => u.Email == model.Username); //ยังไม่เข้าใจ แต่ตอนนี้ เดาๆว่าน่าจะเป็นการค่าหาค่าจากidที่ทำการส่งเข้าไป
            if (dbUser is not null)               
            {
                //login success
                return new LoggedInUser(dbUser.Id, $"{dbUser.FirstName} {dbUser.LastName}".Trim());

            }
            else
            {
                //login failed
                return null;
            }
                            
                            
        }
    }
}