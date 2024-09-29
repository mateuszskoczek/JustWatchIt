﻿using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Movies;

public class MovieQueryParameters : QueryParameters<MovieResponse>
{
    #region PROPERTIES

    [FromQuery(Name = "title")]
    public string? Title { get; set; }

    [FromQuery(Name = "original_title")]
    public string? OriginalTitle { get; set; }

    [FromQuery(Name = "description")]
    public string? Description { get; set; }

    [FromQuery(Name = "release_date")]
    public DateOnly? ReleaseDate { get; set; }

    [FromQuery(Name = "release_date_from")]
    public DateOnly? ReleaseDateFrom { get; set; }

    [FromQuery(Name = "release_date_to")]
    public DateOnly? ReleaseDateTo { get; set; }

    [FromQuery(Name = "length")]
    public short? Length { get; set; }

    [FromQuery(Name = "length_from")]
    public short? LengthFrom { get; set; }

    [FromQuery(Name = "length_to")]
    public short? LengthTo { get; set; }

    [FromQuery(Name = "budget")]
    public decimal? Budget { get; set; }

    [FromQuery(Name = "budget_from")]
    public decimal? BudgetFrom { get; set; }

    [FromQuery(Name = "budget_to")]
    public decimal? BudgetTo { get; set; }

    [FromQuery(Name = "rating_average")]
    public double? RatingAverage { get; set; }

    [FromQuery(Name = "rating_average_from")]
    public double? RatingAverageFrom { get; set; }

    [FromQuery(Name = "rating_average_to")]
    public double? RatingAverageTo { get; set; }

    [FromQuery(Name = "rating_count")]
    public double? RatingCount { get; set; }

    [FromQuery(Name = "rating_count_from")]
    public double? RatingCountFrom { get; set; }

    [FromQuery(Name = "rating_count_to")]
    public double? RatingCountTo { get; set; }

    #endregion
    
    
    
    #region PUBLIC METHODS

    public override bool IsMeetingConditions(MovieResponse item) =>
    (
        TestStringWithRegex(item.Title, Title)
        &&
        TestStringWithRegex(item.OriginalTitle, OriginalTitle)
        &&
        TestStringWithRegex(item.Description, Description)
        &&
        TestComparable(item.ReleaseDate, ReleaseDate, ReleaseDateFrom, ReleaseDateTo)
        &&
        TestComparable(item.Length, Length, LengthFrom, LengthTo)
        &&
        TestComparable(item.Budget, Budget, BudgetFrom, BudgetTo)
        &&
        TestComparable(item.Rating.Average, RatingAverage, RatingAverageFrom, RatingAverageTo)
        &&
        TestComparable(item.Rating.Count, RatingCount, RatingCountFrom, RatingCountTo)
    );

    #endregion
}