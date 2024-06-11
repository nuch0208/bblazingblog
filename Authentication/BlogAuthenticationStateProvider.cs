using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;

namespace BlazingBlog.Authentication
{
    public class BlogAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
    {
        private const String BlogAuthenticationType = "blog-auth"; //เพิ่มหลังจากสร้าง claimPrinciple แล้ว
        private readonly AuthenticationService _authenticationService;
        public BlogAuthenticationStateProvider(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            AuthenticationStateChanged += BlogAuthenticationStateProvider_AuthenticationStateChanged;
        }
         private async void BlogAuthenticationStateProvider_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            var authState = await task;  //await get user โดยใช้คำสั่ง
            if (authState is not null) //เช็ค AuthState
            {
                var userId = Convert.ToInt32(authState.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var displayName = authState.User.FindFirstValue(ClaimTypes.Name);
                LoggedInUser = new(userId, displayName!);
            }
        }
        public LoggedInUser LoggedInUser { get; private set; } = new(0, string.Empty);//stage chaning แจ้ง user เมื่อเรา login stage เปลี่ยน พอ logout stage เปลี่ยน 
        public override async Task<AuthenticationState> GetAuthenticationStateAsync() //we check it stage ของ Authenticate ซึ่ง require ClaimsPrincipal
        
        
        {
            var claimsPrincipal = new ClaimsPrincipal(); //สร้าง claimprinciple *claimsPrincipal เป็นคลาส จ้า เพิ่ม using ด้วย เหมือนการอ้างอิงสิทธิ์การ login ว่าเป็น user จริง ๆนะ 
            var user = await _authenticationService.GetUserFromBrowserStorageAsync(); //get user from browser stroage ก็ใช้ Function Read... ที่กำหนดใหหน้า AuthenticatinService
            if (user is not null)// check หากเราได้รับ user นั่นหมายความว่า user loggedin แล้ว และเก็บค่าใน browser storage
            {
                var identity = new ClaimsIdentity( //require และ identity เป็น type ของการ Authentication
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Value.UserId.ToString()), //*claimsPrincipal เป็นคลาส จึงต้องมีการอ้างถึงแต่ละรายการ
                    new Claim(ClaimTypes.Name, user.Value.DisplayName) //user.Value.Displayname)
                },
                 BlogAuthenticationType ); // parameter
                claimsPrincipal = new(identity);
            }
          return new AuthenticationState(claimsPrincipal);
        }

        public async Task<string?>LoginAsync(LoginModel loginModel)
        {
          var loggedInUser = await _authenticationService.LoginUserAsync(loginModel); //try to loginAsync
          if(loggedInUser is  null) //loggedInUser is null
            {
                
                return "Invalid credentials";
            }
            
          return null;
         }
         public void Dispose() =>
            AuthenticationStateChanged -= BlogAuthenticationStateProvider_AuthenticationStateChanged;
    }
}