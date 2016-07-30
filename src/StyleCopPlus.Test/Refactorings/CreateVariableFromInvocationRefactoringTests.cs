using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Refactorings;
using StyleCopPlus.Test.Helpers;
using StyleCopPlus.Test.Verifiers;

namespace StyleCopPlus.Test.Refactorings
{
    [TestClass]
    public class CreateVariableFromInvocationRefactoringTests : CodeRefactoringVerifier
    {
        [TestMethod]
        public void CreatesVariableForMethodCall()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.RefactoringsCreateVariable);
            string expected = DataHelper.GetEmbeddedResource(DataHelper.RefactoringsCreateVariableNew);

            VerifyRefactoring(test, expected, TextSpan.FromBounds(210, 210));
        }

        protected override CodeRefactoringProvider CreateProvider()
        {
            return new CreateVariableFromInvocationRefactoringProvider();
        }
    }
}