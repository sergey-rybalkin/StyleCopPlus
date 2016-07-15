using System;
using System.IO;
using System.Reflection;

namespace StyleCopPlus.Test.Helpers
{
    internal static class DataHelper
    {
        internal static string GetEmbeddedResource(string fileName)
        {
            string resourceName = "StyleCopPlus.Test.Playground." + fileName;
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

        internal const string SP2101LongConstructor = "SP2101.LongConstructor.cs";
        internal const string SP2101LongFinalizer = "SP2101.LongFinalizer.cs";
        internal const string SP2101LongMethod = "SP2101.LongMethod.cs";
        internal const string SP2101LongStaticConstructor = "SP2101.LongStaticConstructor.cs";
        internal const string SP2101ValidClass = "SP2101.ValidClass.cs";

        internal const string SP2102LongGetter = "SP2102.LongGetter.cs";
        internal const string SP2102LongSetter = "SP2102.LongSetter.cs";
        internal const string SP2102ValidClass = "SP2102.ValidClass.cs";

        internal const string SP2103LongClass = "SP2103.LongClass.cs";
        internal const string SP2103ValidClass = "SP2103.ValidClass.cs";

        internal const string SP2002EmptyLine = "SP2002.EmptyLine.cs";
        internal const string SP2002EmptyLineFixed = "SP2002.EmptyLineFixed.cs";
        internal const string SP2002NonEmptyLine = "SP2002.NonEmptyLine.cs";

        internal const string SP1131IncorrectOperands = "SP1131.IncorrectOperands.cs";
        internal const string SP1131IncorrectOperandsFixed = "SP1131.IncorrectOperandsFixed.cs";
    }
}