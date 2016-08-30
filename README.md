# StyleCopPlus Analyzers and Refactorings

[![Build status](https://ci.appveyor.com/api/projects/status/k8pfm3e0miaqrada/branch/master?svg=true)](https://ci.appveyor.com/project/sergey-rybalkin/stylecopplus-analyzers/branch/master)

This project is an implementation of StyleCop+ rules using the .NET Compiler Platform (Roslyn) as well as some 
additional refactorings and analyzers. Original project is available on 
[CodePlex](https://stylecopplus.codeplex.com/).

## Installation
This project is available as a NuGet package and VSIX extension. Currently Roslyn does not support loading 
refactorings from NuGet packages so you need to install VSIX extension in order to get full functionality.

[Latest VSIX and NuGet Packages](https://ci.appveyor.com/project/sergey-rybalkin/stylecopplus-analyzers/build/artifacts)

## Analyzers

 - SP1131 (Unsafe Condition Analyzer) - validates that constant values are placed to the left of `==` and `!=` operators.
 - SP2002 (Last Line Empty Analyzer) - validates that files do not end with an empty line.
 - SP2100 (Line Too Long Analyzer) - validates that code lines do not exceed 110 symbols.
 - SP2101 (Method Too Long Analyzer) - validates that methods length do not exceed 50 lines.
 - SP2102 (Property Too Long Analyzer) - validates that property accessors do not exceed 40 lines.
 - SP2103 (File Too Long Analyzer) - validates that files length do not exceed 400 lines.

## Refactorings

 - Check Parameters - adds null check code for method or constructor parameters.
 - Create Variable From Invocation - saves result of method or property to local variable.
 - Duplicate Method - creates an exact copy of the method under cursor.
 - Introduce Field - creates and initializes class field from constructor parameter.