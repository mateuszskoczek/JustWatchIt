using WatchIt.Common.Model.Media.Medium;
using WatchIt.WebAPI.Services.Controllers.Common;

namespace WatchIt.WebAPI.Services.Controllers.Series;

public interface ISeriesControllerService
{
    Task<RequestResult> GetSeries(MediumSeriesResponseQueryParameters query);
    Task<RequestResult> GetSeries(long id);
    Task<RequestResult> PostSeries(MediumSeriesRequest data);
    Task<RequestResult> PutSeries(long id, MediumSeriesRequest data);
    Task<RequestResult> DeleteSeries(long id);
    Task<RequestResult> GetSeriesViewRank(int first, int days);
}