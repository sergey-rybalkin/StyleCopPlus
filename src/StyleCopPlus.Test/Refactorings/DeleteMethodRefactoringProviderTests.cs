using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Refactorings;
using StyleCopPlus.Test.Helpers;
using StyleCopPlus.Test.Verifiers;

namespace StyleCopPlus.Test.Refactorings
{
    [TestClass]
    public class DeleteMethodRefactoringProviderTests : CodeRefactoringVerifier
    {
        [TestMethod]
        public void DeletesSingleMethod()
        {
            VerifyRefactoringWithResources(
                DataHelper.DeleteMethodDeleteMethodFromIdentifier,
                DataHelper.DeleteMethodDeleteMethodFromIdentifierGold,
                0);
        }

        [TestMethod]
        public void DeletesInnerMethod()
        {
            VerifyRefactoringWithResources(
                DataHelper.DeleteMethodDeleteMethodFromParams,
                DataHelper.DeleteMethodDeleteMethodFromParamsGold,
                0);
        }

        protected override CodeRefactoringProvider CreateProvider()
        {
            return new DeleteMethodRefactoringProvider();
        }
    }
}
