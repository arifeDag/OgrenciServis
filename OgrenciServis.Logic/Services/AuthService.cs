using OgrenciServis.DataAccess;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models.DTO;

namespace OgrenciServis.Logic.Services
{
    public class AuthService : IAuthService
    {
        private readonly OkulContext context;
        private readonly IJwtTokenService jwtTokenService;

        public AuthService(OkulContext context, IJwtTokenService jwtTokenService)
        {
            this.context = context;
            this.jwtTokenService = jwtTokenService;
        }
        public LoginResponseDto? Login(LoginRequestDto loginRequest)
        {
            //TOdo ogrenci dolduracak
            var user = this.context.Users.FirstOrDefault(u => u.UserName == loginRequest.Username);

            //var user =(from u in this.context.Users
            //           where u.UserName==loginRequest.Username
            //           select u).FirstOrDefault();

            if (user == null)
                return null;

            //sife dogrulaması
            if (user.Password != loginRequest.Password)
                return null;


            var token = this.jwtTokenService.GenerateToken(user);



            return new LoginResponseDto
            {
                Token = token,
                Username = user.UserName,
                Role = user.Role,
                Expiresat = DateTime.UtcNow.AddHours(1),
            };

            throw new NotImplementedException();
        }
    }
}
