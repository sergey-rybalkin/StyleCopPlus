namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// Configuration settings provider storage.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Maximum code line length for SP2100 rule.
        /// </summary>
        public const int SP2100MaxLineLength = 110;

        /// <summary>
        /// Maximum number of lines in file.
        /// </summary>
        public const int SP2103MaxFileLength = 400;

        /// <summary>
        /// Maximum number of lines in property accessor.
        /// </summary>
        public const int SP2102MaxPropertyAccessorLength = 40;

        /// <summary>
        /// Maximum number of lines in method.
        /// </summary>
        public const int SP2101MaxMethodLength = 50;
    }
}