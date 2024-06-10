using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
namespace BlazingBlog.Authentication
{
    public class AuthenticationService
    {
        private readonly UserService _userService;
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        public AuthenticationService(UserService userServer, ProtectedLocalStorage protectedLocalStorage)
        {
            _userService = userServer;
            _protectedLocalStorage = protectedLocalStorage;
        }
        public async Task<LoggedInUser?>GetUserAsync(LoginModel loginModel)
        {
            var loggedInUser = await _userService.LoginAsync(loginModel); //loggedInUser มีค่า null หรือ login user ถ้าข้อมูลถูกก็ reture ค่าใน return loggedInUser;
            if (loggedInUser is not null) //เมื่อ user loging เช็ค if is not null ให้
            {
                await SaveUserToBrowserStorageAsync(loggedInUser.Value); //save ใน browser storage
            }
            return loggedInUser;    //หากมีข้อมูลก็ให้ return
            }
        private const string UserStorageKey = "blg_user";
        private JsonSerializerOptions _jsonSerializrOptions = new JsonSerializerOptions
        {

        };
        public async Task SaveUserToBrowserStorageAsync(LoggedInUser user) =>
            await _protectedLocalStorage.SetAsync(UserStorageKey, JsonSerializer.Serialize(user, _jsonSerializrOptions));

        public async Task<LoggedInUser?> GetUserFromBrowserStorageAsync() //อ่านข้อมูลจาก browser
        {
            try
            {
                var result = await _protectedLocalStorage.GetAsync<string>(UserStorageKey);
                if(result.Success && !string.IsNullOrWhiteSpace(result.Value))
                {
                var loggedInUser = JsonSerializer.Deserialize<LoggedInUser>(result.Value, _jsonSerializrOptions);
                return loggedInUser;
                }
            }
            catch(InvalidOperationException)
            {
                // Eat out this exception
                // as we know this will occure when this method is being class from server
                // Where there is no Browser and LocalStorage
                // Don't worry about this, as this will be called from client side(Browser's side) after this
                // So we will have the data there
                // So we are good to ignore this excetion
            }
            return null;
        }       
        
        public async Task RemoveUserFromBroserStorageAsync()=>
            await _protectedLocalStorage.DeleteAsync(UserStorageKey);
    }
}

//เมื่อ user login  เราต้องการ save user ใน SaveuserToBorwserStorage 