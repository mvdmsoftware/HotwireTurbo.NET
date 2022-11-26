namespace HotwireTurbo.NET.Interfaces;

/// <summary>
/// Interface that can be added to Models that are used by Turbo to change dom elements based on an ID.
/// </summary>
public interface IHasDomId
{
    /// <summary>
    /// Retrieve the DOM ID from the model.
    /// </summary>
    /// <returns>The DOM ID provided by the model.</returns>
    string ToDomId();
}