using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentStuff.Identity.Infrastructure.Persistence.Model;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using RentStuff.Identity.Infrastructure.Services.Email;

namespace RentStuff.Identity.Application.Account
{
    public class AccountApplicationService : IAccountApplicationService
    {
        private IEmailService _emailService;
        private IAuthRepository _authRepository;

        public AccountApplicationService(IEmailService emailService, IAuthRepository authRepository)
        {
            _emailService = emailService;
            _authRepository = authRepository;
        }


        public bool Register()
        {
            throw new NotImplementedException();
        }

        public bool SendActivationEmail(string email, string name)
        {

            // ToDo: activation link
            //_emailService.SendEmail(email, EmailConstants.ActivationEmailSubject, EmailConstants.ActivationEmailMessage(name, activationLink));
            return false;
        }

        public async Task<CustomIdentityUser> FindUser(string userName, string password)
        {
            CustomIdentityUser user = await _authRepository.FindUser(userName, password);

            return user;
        }
    }
}
