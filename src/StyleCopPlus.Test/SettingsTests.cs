using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace StyleCopPlus.Test
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void Parse_GivenValidConfiguration_IsAbleToParseIt()
        {
            // Arrange
            string settings = @"{
""$schema"": """",
        ""settings"": {
            ""indentation"": {
                ""useTabs"": false
            },
""orderingRules"": {
                ""usingDirectivesPlacement"": ""outsideNamespace""
},
""layoutRules"": {
                ""newlineAtEndOfFile"": ""require""
},
""styleCopPlusRules"": {
                ""maxLineLength"": 1,
    ""maxFileLength"": 4,
    ""maxPropertyAccessorLength"": 3,
    ""maxMethodLength"": 2
}
        }
}";
            // Act
            Settings result = Settings.Parse(settings);

            // Assert
            result.SP2100MaxLineLength.Should().Be(1);
            result.SP2101MaxMethodLength.Should().Be(2);
            result.SP2102MaxPropertyAccessorLength.Should().Be(3);
            result.SP2103MaxFileLength.Should().Be(4);
        }

        [TestMethod]
        public void Parse_GivenInvalidConfiguration_UsesDefaultValues()
        {
            // Arrange
            string settings = @"{
""$schema"": """",
        ""settings"": {
            ""indentation"": {
                ""useTabs"": false
            },
""orderingRules"": {
                ""usingDirectivesPlacement"": ""outsideNamespace""
},
""layoutRules"": {
                ""newlineAtEndOfFile"": ""require""
},
""styleCopPlusRules"": {
                ""maxLineLength"": ""string"",
    ""maxPropertyAccessorLength"": 111111111111111111111111111111111111111
}
        }
}";
            // Act
            Settings result = Settings.Parse(settings);

            // Assert
            result.SP2100MaxLineLength.Should().Be(Settings.SP2100MaxLineLengthDefault);
            result.SP2101MaxMethodLength.Should().Be(Settings.SP2101MaxMethodLengthDefault);
            result.SP2103MaxFileLength.Should().Be(Settings.SP2103MaxFileLengthDefault);
            result.SP2102MaxPropertyAccessorLength
                  .Should()
                  .Be(Settings.SP2102MaxPropertyAccessorLengthDefault);
        }
    }
}
