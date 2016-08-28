using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Refactorings;
using StyleCopPlus.Test.Helpers;
using StyleCopPlus.Test.Verifiers;

namespace StyleCopPlus.Test.Refactorings
{
    [TestClass]
    public class IntroduceFieldRefactoringProviderTests : CodeRefactoringVerifier
    {
        [TestMethod]
        public void CreatesReadonlyField()
        {
            VerifyRefactoringWithResources(
                DataHelper.IntroduceFieldReadonlyField,
                DataHelper.IntroduceFieldReadonlyFieldGold,
                1);
        }

        protected override CodeRefactoringProvider CreateProvider()
        {
            return new IntroduceFieldRefactoringProvider();
        }
    }
}