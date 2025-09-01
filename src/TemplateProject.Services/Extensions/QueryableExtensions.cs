using System.Linq.Expressions;

namespace TemplateProject.Services.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IQueryable{T}"/> to enhance query composition.
/// </summary>
public static class QueryableExtensions
{
  /// <summary>
  /// Conditionally applies a filter to a sequence based on a specified condition.
  /// </summary>
  /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
  /// <param name="source">The source query to which the filter is applied.</param>
  /// <param name="condition">A boolean value that determines whether the filter should be applied.
  /// If true, the filter is applied; otherwise, the original sequence is returned.
  /// </param>
  /// <param name="predicate">An expression that represents the filter to apply to the sequence.
  /// This is only evaluated if <paramref name="condition"/> is true.</param>
  public static IQueryable<T> WhereIf<T>(
      this IQueryable<T> source,
      bool condition,
      Expression<Func<T, bool>> predicate)
  {
    return condition ? source.Where(predicate) : source;
  }
}
