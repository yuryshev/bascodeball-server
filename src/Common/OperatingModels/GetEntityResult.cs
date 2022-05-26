using System.Runtime.Serialization;

namespace Common.OperatingModels;
/// <summary>Generic result for getting entity</summary>
/// <typeparam name="T">The entity type.</typeparam>
public struct GetEntityResult<T>
{
  private GetEntityResult(GetEntityResult<T>.ResultType status, T entity = default)
  {
    this.Status = status;
    this.Entity = entity;
  }

  /// <summary>Gets the entity.</summary>
  /// <value>The entity.</value>
  public T Entity { get; }

  /// <summary>Gets the type of the status result.</summary>
  /// <value>The type of the status result.</value>
  public GetEntityResult<T>.ResultType Status { get; }

  /// <summary>
  /// Gets a value indicating whether this instance is success.
  /// </summary>
  /// <value>
  ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
  /// </value>
  public bool IsSuccess => this.Status == GetEntityResult<T>.ResultType.Found;

  /// <summary>From the not found.</summary>
  /// <returns></returns>
  public static GetEntityResult<T> FromNotFound() => new GetEntityResult<T>(GetEntityResult<T>.ResultType.NotFound);

  /// <summary>From the found.</summary>
  /// <param name="entity">The entity.</param>
  /// <returns></returns>
  /// <exception cref="T:System.ArgumentNullException">entity</exception>
  public static GetEntityResult<T> FromFound(T entity) => (object) entity != null ? new GetEntityResult<T>(GetEntityResult<T>.ResultType.Found, entity) : throw new ArgumentNullException(nameof (entity));

  /// <summary>Creates result when DB error occured.</summary>
  public static GetEntityResult<T> FromDbError() => new GetEntityResult<T>(GetEntityResult<T>.ResultType.DatabaseError);

  /// <summary>Creates result when unexpected error occured.</summary>
  public static GetEntityResult<T> FromUnexpectedError() => new GetEntityResult<T>(GetEntityResult<T>.ResultType.UnexpectedError);

  /// <summary>Result of entity reading operation</summary>
  public enum ResultType
  {
    /// <summary>Entity was found</summary>
    Found,
    /// <summary>Entity was not found</summary>
    NotFound,
    /// <summary>The DB error.</summary>
    [EnumMember(Value = "DB_ERROR")] DatabaseError,
    /// <summary>The unexpected error.</summary>
    [EnumMember(Value = "UNEXPECTED_ERROR")] UnexpectedError,
  }
}