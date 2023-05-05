using LOFit.Models.Menu;

namespace LOFit.DataServices.Measurement
{
    public interface IMeasurementRestService
    {
        Task<MeasurementModel> Get(DateTime date);
        Task<MeasurementModel> GetLast(int id);
        Task<List<MeasurementModel>> GetWeek(DateTime date, int id);
        Task<string> Add(MeasurementModel form);
        Task<string> Update(MeasurementModel form);
    }
}
