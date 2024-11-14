using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Photos.Photo;

public class PhotoResponseQueryParameters : QueryParameters<PhotoResponse>
{
    #region CONSTANTS
    
    public override IDictionary<string, Func<PhotoResponse, IComparable?>>? OrderKeys => _orderKeys;
    protected static readonly IDictionary<string, Func<PhotoResponse, IComparable?>> _orderKeys = new Dictionary<string, Func<PhotoResponse, IComparable?>>
    {
        { "id", x => x.Id },
        { "medium_id", x => x.MediumId },
        { "mime_type", x => x.MimeType },
        { "is_background", x => x.Background is not null },
        { "is_universal_background", x => x.Background is not null && x.Background.IsUniversal }
    };
    
    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "media_id")]
    public long? MediaId { get; set; }
    
    [FromQuery(Name = "mime_type")]
    public string? MimeType { get; set; }
    
    [FromQuery(Name = "is_background")]
    public bool? IsBackground { get; set; }
    
    [FromQuery(Name = "is_universal_background")]
    public bool? IsUniversalBackground { get; set; }

    [FromQuery(Name = "upload_date")]
    public DateOnly? UploadDate { get; set; }

    [FromQuery(Name = "upload_date_from")]
    public DateOnly? UploadDateFrom { get; set; }

    [FromQuery(Name = "upload_date_to")]
    public DateOnly? UploadDateTo { get; set; }

    #endregion
    
    
    
    #region PRIVATE METHODS

    protected override bool IsMeetingConditions(PhotoResponse item) =>
    (
        Test(item.MediumId, MediaId)
        &&
        TestStringWithRegex(item.MimeType, MimeType)
        &&
        Test(item.Background is not null, IsBackground)
        &&
        Test(item.Background is not null && item.Background.IsUniversal, IsUniversalBackground)
        &&
        TestComparable(item.UploadDate, UploadDate, UploadDateFrom, UploadDateTo)
    );

    #endregion
}