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
        Task<CoachModel> GetOne(int id);
        Task<string> Update(CoachModel model);
        Task<List<CoachModel>> GetAll();
        Task<List<CoachModel>> GetMy(int type);
    }
}
