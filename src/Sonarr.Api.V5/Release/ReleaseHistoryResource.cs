using NzbDrone.Core.History;
using NzbDrone.Core.Parser.Model;

namespace Sonarr.Api.V5.Release;

public class ReleaseHistoryResource
{
    public DateTime? Grabbed { get; set; }
    public DateTime? Failed { get; set; }
}

public static class ReleaseHistoryResourceMapper
{
    public static ReleaseHistoryResource? ToResource(this ReleaseInfo release, List<EpisodeHistory> history)
    {
        var grabbedHistory = history.FirstOrDefault(h => h.EventType == EpisodeHistoryEventType.Grabbed &&
                                                         h.Data.TryGetValue("guid", out var guid) &&
                                                         guid == release.Guid);

        if (grabbedHistory != null)
        {
            var resource = new ReleaseHistoryResource
            {
                Grabbed = grabbedHistory.Date,
            };

            var failedHistory = history.FirstOrDefault(h => h.EventType == EpisodeHistoryEventType.DownloadFailed &&
                                                            h.DownloadId == grabbedHistory.DownloadId);

            if (failedHistory != null)
            {
                resource.Failed = failedHistory.Date;
            }

            return resource;
        }

        return null;
    }
}
