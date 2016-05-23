using System;
using System.IO;
using System.Reflection;

namespace StyleCopPlus.Analyzers.Test.Helpers
{
    internal static class DataHelper
    {
        internal static string GetEmbeddedResource(string fileName)
        {
            string resourceName = "StyleCopPlus.Analyzers.Test.Playground." + fileName;
            Assembly assembly = Assembly.GetCallingAssembly();
            Stream stream = assembly.GetManifestResourceStream(resourceName);

            if (null == stream)
                throw new InvalidOperationException("Embedded resource not found: " + fileName);

            StreamReader reader = new StreamReader(stream);

            using (reader)
            {
                return reader.ReadToEnd();
            }
        }

        internal const string SP2100ConstructorDefinition = "SP2100.ConstructorDefinition.cs";
        internal const string SP2100ConstructorInvocation = "SP2100.ConstructorInvocation.cs";
        internal const string SP2100FluentApi = "SP2100.FluentApi.cs";
        internal const string SP2100MethodDefinition = "SP2100.MethodDefinition.cs";
        internal const string SP2100MethodInvocation = "SP2100.MethodInvocation.cs";
        internal const string SP2100MethodInvocationWithAssignment =
            "SP2100.MethodInvocationWithAssignment.cs";

        internal const string SP2100ConstructorDefinitionFixed = "SP2100.ConstructorDefinitionFixed.cs";
        internal const string SP2100ConstructorInvocationFixed = "SP2100.ConstructorInvocationFixed.cs";
        internal const string SP2100FluentApiFixed = "SP2100.FluentApiFixed.cs";
        internal const string SP2100MethodDefinitionFixed = "SP2100.MethodDefinitionFixed.cs";
        internal const string SP2100MethodInvocationFixed = "SP2100.MethodInvocationFixed.cs";
        internal const string SP2100MethodInvocationWithAssignmentFixed =
            "SP2100.MethodInvocationWithAssignmentFixed.cs";
    }
}