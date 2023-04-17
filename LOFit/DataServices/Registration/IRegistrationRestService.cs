using LOFit.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOFit.DataServices.Registration
{
    public interface IRegistrationRestService
    {
        Task<string> CreateUser(RegistrationModel form);
        Task<string> CreateCoach(RegistrationModel form);
    }
}
