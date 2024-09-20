﻿using WatchIt.Common.Model.Genres;
using WatchIt.Common.Model.Media;

namespace WatchIt.Website.Services.WebAPI.Media;

public interface IMediaWebAPIService
{
    Task GetMedia(long mediaId, Action<MediaResponse>? successAction = null, Action? notFoundAction = null);
    
    Task GetMediaGenres(long mediaId, Action<IEnumerable<GenreResponse>>? successAction = null, Action? notFoundAction = null);
    Task PostMediaGenre(long mediaId, long genreId, Action? successAction = null, Action? unauthorizedAction = null, Action? forbiddenAction = null, Action? notFoundAction = null);

    Task GetMediaRating(long mediaId, Action<MediaRatingResponse>? successAction = null, Action? notFoundAction = null);
    Task GetMediaRatingByUser(long mediaId, long userId, Action<short>? successAction = null, Action? notFoundAction = null);
    Task PutMediaRating(long mediaId, MediaRatingRequest body, Action? successAction = null, Action<IDictionary<string, string[]>>? badRequestAction = null, Action? unauthorizedAction = null, Action? notFoundAction = null);
    Task DeleteMediaRating(long mediaId, Action? successAction = null, Action? unauthorizedAction = null);
    
    Task GetPhotoMediaRandomBackground(long mediaId, Action<MediaPhotoResponse>? successAction = null, Action? notFoundAction = null);
    Task GetPhotoRandomBackground(Action<MediaPhotoResponse>? successAction = null, Action? notFoundAction = null);
    Task GetPoster(long mediaId, Action<MediaPosterResponse>? successAction = null, Action<IDictionary<string, string[]>>? badRequestAction = null, Action? notFoundAction = null);
    Task PutPoster(long mediaId, MediaPosterRequest data, Action? successAction = null, Action<IDictionary<string, string[]>>? badRequestAction = null, Action? unauthorizedAction = null, Action? forbiddenAction = null);
    Task DeletePoster(long mediaId, Action? successAction = null, Action? unauthorizedAction = null, Action? forbiddenAction = null);
}