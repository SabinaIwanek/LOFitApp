using LOFit.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOFit.DataServices.Login
{
    public interface ILoginRestService
    {
        Task<string> Login(LoginModel model);
        Task<string> SendMail(string email);
    }
}
