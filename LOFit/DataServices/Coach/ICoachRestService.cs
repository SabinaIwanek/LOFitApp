using LOFit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOFit.DataServices.Coach
{
    public interface ICoachRestService
    {
        Task<List<CoachModel>> GetAll();
    }
}
