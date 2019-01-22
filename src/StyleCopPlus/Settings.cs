using System.Text.RegularExpressions;

namespace StyleCopPlus
{
    /// <summary>
    /// Configuration settings provider storage.
    /// </summary>
    public struct Settings
    {
        // Avoid dependencies on JSON libraries by using regular expressions based settings parser.
        private static readonly Regex LineLengthExpression =
            new Regex(@"""maxLineLength""\D+(\d+)", RegexOptions.Compiled);

        private static readonly Regex FileLengthExpression =
            new Regex(@"""maxFileLength""\D+(\d+)", RegexOptions.Compiled);

        private static readonly Regex PropertyAccessorLengthExpression =
            new Regex(@"""maxPropertyAccessorLength""\D+(\d+)", RegexOptions.Compiled);

        private static readonly Regex MethodLengthExpression =
            new Regex(@"""maxMethodLength""\D+(\d+)", RegexOptions.Compiled);

        public const int SP2100MaxLineLengthDefault = 110;

        public const int SP2103MaxFileLengthDefault = 400;

        public const int SP2102MaxPropertyAccessorLengthDefault = 40;

        public const int SP2101MaxMethodLengthDefault = 50;

        /// <summary>
        /// Gets or sets maximum code line length for SP2100 rule.
        /// </summary>
        public int SP2100MaxLineLength { get; set; }

        /// <summary>
        /// Gets or sets maximum number of lines in file.
        /// </summary>
        public int SP2103MaxFileLength { get; set; }

        /// <summary>
        /// Gets or sets maximum number of lines in property accessor.
        /// </summary>
        public int SP2102MaxPropertyAccessorLength { get; set; }

        /// <summary>
        /// Gets or sets maximum number of lines in method.
        /// </summary>
        public int SP2101MaxMethodLength { get; set; }

        /// <summary>
        /// Creates settings instance with default values.
        /// </summary>
        public static Settings GetDefault()
        {
            return new Settings()
            {
                SP2100MaxLineLength = SP2100MaxLineLengthDefault,
                SP2103MaxFileLength = SP2103MaxFileLengthDefault,
                SP2102MaxPropertyAccessorLength = SP2102MaxPropertyAccessorLengthDefault,
                SP2101MaxMethodLength = SP2101MaxMethodLengthDefault
            };
        }

        /// <summary>
        /// Parses specified configuration options.
        /// </summary>
        /// <param name="settings">Serialized configuration options to parse.</param>
        public static Settings Parse(string settings)
        {
            var retVal = new Settings();

            int ExtractConfigurationValue(string content, Regex expression, int defaultValue)
            {
                Match match = expression.Match(content);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int val))
                    return val;
                else
                    return defaultValue;
            }

            retVal.SP2100MaxLineLength = ExtractConfigurationValue(
                settings,
                LineLengthExpression,
                SP2100MaxLineLengthDefault);
            retVal.SP2101MaxMethodLength = ExtractConfigurationValue(
                settings,
                MethodLengthExpression,
                SP2101MaxMethodLengthDefault);
            retVal.SP2102MaxPropertyAccessorLength = ExtractConfigurationValue(
                settings,
                PropertyAccessorLengthExpression,
                SP2102MaxPropertyAccessorLengthDefault);
            retVal.SP2103MaxFileLength = ExtractConfigurationValue(
                settings,
                FileLengthExpression,
                SP2103MaxFileLengthDefault);

            return retVal;
        }
    }
}
