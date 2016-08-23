using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Refactorings;
using StyleCopPlus.Test.Helpers;
using StyleCopPlus.Test.Verifiers;

namespace StyleCopPlus.Test.Refactorings
{
    [TestClass]
    public class CheckParametersRefactoringProviderTests : CodeRefactoringVerifier
    {
        [TestMethod]
        public void ChecksGenericParameter()
        {
            VerifyRefactoringWithResources(
                DataHelper.CheckParametersGenericParameter,
                DataHelper.CheckParametersGenericParameterGold);
        }

        [TestMethod]
        public void ChecksReferenceParameter()
        {
            VerifyRefactoringWithResources(
                DataHelper.CheckParametersReferenceParameter,
                DataHelper.CheckParametersReferenceParameterGold);
        }

        [TestMethod]
        public void IgnoresRefParameters()
        {
            VerifyNoRefactoring(DataHelper.CheckParametersRefParameter);
        }

        [TestMethod]
        public void IgnoresOutParameters()
        {
            VerifyNoRefactoring(DataHelper.CheckParametersOutParameter);
        }

        [TestMethod]
        public void IgnoresValueTypeParameters()
        {
            VerifyNoRefactoring(DataHelper.CheckParametersValueTypeParameter);
        }

        protected override CodeRefactoringProvider CreateProvider()
        {
            return new CheckParametersRefactoringProvider();
        }
    }
}