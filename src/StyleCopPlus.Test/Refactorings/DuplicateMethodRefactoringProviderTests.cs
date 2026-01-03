using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Refactorings;
using StyleCopPlus.Test.Helpers;
using StyleCopPlus.Test.Verifiers;

namespace StyleCopPlus.Test.Refactorings
{
    [TestClass]
    public class DuplicateMethodRefactoringProviderTests : CodeRefactoringVerifier
    {
        [TestMethod]
        public void DuplicatesFirstMethod()
        {
            VerifyRefactoringWithResources(
                DataHelper.DuplicateMethodDuplicateFirstMethod,
                DataHelper.DuplicateMethodDuplicateFirstMethodGold,
                0);
        }

        [TestMethod]
        public void DuplicatesInnerMethod()
        {
            VerifyRefactoringWithResources(
                DataHelper.DuplicateMethodDuplicateInnerMethod,
                DataHelper.DuplicateMethodDuplicateInnerMethodGold,
                0);
        }

        protected override CodeRefactoringProvider CreateProvider()
        {
            return new DuplicateMethodRefactoringProvider();
        }
    }
}
