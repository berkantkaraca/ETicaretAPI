using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _signInManager;

        public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(request.UsernameOrEmail);

            if (user == null) 
                throw new NotFoundUserException("Kullanıcı adı veya şifre hatalı");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                //Yetki kontrölü değerlendirilecek
            }

            return new();
           
        }
    }
}
