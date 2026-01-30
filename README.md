# StyleCopPlus Analyzers and Refactorings

[![CI Build](https://github.com/sergey-rybalkin/StyleCopPlus/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/sergey-rybalkin/StyleCopPlus/actions/workflows/ci.yml)
[![NuGet Version](https://img.shields.io/nuget/v/StyleCopPlus)](https://www.nuget.org/packages/StyleCopPlus)

This project is an implementation of StyleCop+ rules using the .NET Compiler Platform (Roslyn) as well as some
additional opinionated refactorings and analyzers. Original project is available on
[CodePlex](https://stylecopplus.codeplex.com/).

## Installation
This project is available as a NuGet package. Visual Studio extension is no longer supported.

[NuGet Packages](https://www.nuget.org/packages/StyleCopPlus/)

## Analyzers

 - SP1001 (Invalid Exception Message) - validates exception message to match best practices. Inspired by [Microsoft guidelines](https://docs.microsoft.com/en-us/dotnet/api/system.exception.message?view=netcore-3.1#remarks) and [StackOverflow discussion](https://stackoverflow.com/questions/1136829/do-you-end-your-exception-messages-with-a-period/34136055).
 - SP1002 (Cancellation token name) - validates that parameters of type `CancellationToken` are named `ct` unless specified in base or interface method.
 - SP1003 (Separate return with empty line) - validates that return statement in block is separated with empty line.
 - SP1131 (Unsafe Condition Analyzer) - validates that constant pattern matching is used instead of `==` operator to avoid typos like `if (flag = true)`, also suggests using negated not pattern instead of `!=` operator.
 - SP2100 (Line Too Long Analyzer) - validates that code lines do not exceed 110 symbols.
 - SP2101 (Method Too Long Analyzer) - validates that methods length do not exceed 50 lines.
 - SP2102 (Property Too Long Analyzer) - validates that property accessors do not exceed 40 lines.
 - SP2103 (File Too Long Analyzer) - validates that files length do not exceed 400 lines.

## Configuration
Analyzer line limits can be configured through [StyleCop configuration file](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/Configuration.md). Add the following snippet with configured values
to the end of the file:

    "styleCopPlusRules": {
        "maxLineLength": 110,
        "maxFileLength": 400,
        "maxPropertyAccessorLength": 40,
        "maxMethodLength": 50
    }

## Refactorings

 - Duplicate Method - creates an exact copy of the method under cursor.
 - Delete Method - deletes method and its xml doc comments under cursor.
