namespace Gihan.Storage.Core.Enums
{
    /// <summary>
    /// Specifies what to do if a file or folder with the specified name already exists
    ///     in the current folder when you copy, move, or rename a file or folder.
    /// </summary>
    public enum NameCollisionOption
    {
        /// <summary>
        /// Automatically append a number to the base of the specified name if the file or folder already exists.
        /// </summary>
        GenerateUniqueName,
        /// <summary>
        /// Replace the existing item if the file or folder already exists.
        /// </summary>
        ReplaceExisting,
        /// <summary>
        /// Raise an exception of type <see cref="System.Exception"/> if the file or folder already exists.
        /// </summary>
        FailIfExists,
    }
}