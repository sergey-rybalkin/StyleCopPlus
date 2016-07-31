using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace StyleCopPlus.Test.Helpers
{
    /// <summary>
    /// Contains helper methods for accessing sample data for unit tests.
    /// </summary>
    public static partial class DataHelper
    {
        private const string Marker = "/*C*/";

        internal static string GetEmbeddedResource(string fileName)
        {
            StreamReader reader = GetEmbeddedResourceStream(fileName);

            using (reader)
            {
                return reader.ReadToEnd().Replace(Marker, string.Empty);
            }
        }

        /// <summary>
        /// Gets specified embedded resource as string, also returns position of the marker if any.
        /// </summary>
        /// <param name="fileName">Name of the file to return.</param>
        /// <param name="markerLine">[out] Line number of the marker position.</param>
        /// <param name="markerColumn">[out] Column number of the marker position.</param>
        internal static string GetEmbeddedResource(string fileName, out int markerLine, out int markerColumn)
        {
            StreamReader reader = GetEmbeddedResourceStream(fileName);
            StringBuilder retVal = new StringBuilder(1024);
            string line = reader.ReadLine();
            int lineCounter = 0;
            markerLine = 0;
            markerColumn = 0;

            do
            {
                lineCounter++;
                int col = line.IndexOf(Marker);

                if (col >= 0)
                {
                    markerLine = lineCounter;
                    markerColumn = col + 1; // Visual Studio is using 1-based column indexes
                    retVal.AppendLine(line.Remove(col, Marker.Length));
                }
                else
                    retVal.AppendLine(line);

                line = reader.ReadLine();
            }
            while (null != line);

            return retVal.ToString();
        }

        /// <summary>
        /// Gets specified embedded resource as string, also returns position of the marker if any.
        /// </summary>
        /// <param name="fileName">Name of the file to return.</param>
        /// <param name="markerPositon">[out] Position of the marker in the buffer.</param>
        internal static string GetEmbeddedResource(string fileName, out int markerPositon)
        {
            StreamReader reader = GetEmbeddedResourceStream(fileName);
            string retVal = reader.ReadToEnd();
            markerPositon = retVal.IndexOf(Marker);

            if (-1 == markerPositon)
                throw new InvalidOperationException($"Could not find marker in file {fileName}");

            return retVal.Remove(markerPositon, Marker.Length);
        }

        private static StreamReader GetEmbeddedResourceStream(string fileName)
        {
            string resourceName = "StyleCopPlus.Test.Playground." + fileName;
            Assembly assembly = Assembly.GetCallingAssembly();
            Stream stream = assembly.GetManifestResourceStream(resourceName);

            if (null == stream)
                throw new InvalidOperationException($"Embedded resource not found: {fileName}");

            return new StreamReader(stream);
        }
    }
}