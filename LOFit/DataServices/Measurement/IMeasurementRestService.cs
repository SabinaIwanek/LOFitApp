using LOFit.Models;

namespace LOFit.DataServices.Measurement
{
    public interface IMeasurementRestService
    {
        Task<MeasurementModel> Get(DateTime date);
        Task<List<MeasurementModel>> GetWeek(DateTime date);
        Task<string> Add(MeasurementModel form);
        Task<string> Update(MeasurementModel form);
    }
}
