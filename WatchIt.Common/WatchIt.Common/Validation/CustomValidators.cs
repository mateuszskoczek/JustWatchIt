using FluentValidation;

namespace WatchIt.Common.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, TProperty> CannotBeIn<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IEnumerable<TProperty> collection) => ruleBuilder.Must(x => !collection.Any(e => Equals(e, x)));
    public static IRuleBuilderOptions<T, TProperty> CannotBeIn<T, TProperty, TCollectionType>(this IRuleBuilder<T, TProperty> ruleBuilder, IEnumerable<TCollectionType> collection, Func<TCollectionType, TProperty> propertyFunc) => ruleBuilder.Must(x => !collection.Select(propertyFunc).Any(e => Equals(e, x)));
    public static IRuleBuilderOptions<T, TProperty> MustBeIn<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IEnumerable<TProperty> collection) => ruleBuilder.Must(x => collection.Any(e => Equals(e, x)));
    public static IRuleBuilderOptions<T, TProperty> MustBeIn<T, TProperty, TCollectionType>(this IRuleBuilder<T, TProperty> ruleBuilder, IEnumerable<TCollectionType> collection, Func<TCollectionType, TProperty> propertyFunc) => ruleBuilder.Must(x => collection.Select(propertyFunc).Any(e => Equals(e, x)));
}