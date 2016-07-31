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
        public void CreatesVariableForConstructorCall()
        {
            VerifyRefactoringWithResources(
                DataHelper.CreateVariableConstructorCall,
                DataHelper.CreateVariableConstructorCallGold);
        }

        [TestMethod]
        public void CreatesVariableForFluentApiCalls()
        {
            VerifyRefactoringWithResources(
                DataHelper.CreateVariableFluentApiCalls,
                DataHelper.CreateVariableFluentApiCallsGold);
        }

        [TestMethod]
        public void CreatesVariableForPropertyCall()
        {
            VerifyRefactoringWithResources(
                DataHelper.CreateVariablePropertyCall,
                DataHelper.CreateVariablePropertyCallGold);
        }

        [TestMethod]
        public void CreatesVariableForStaticCall()
        {
            VerifyRefactoringWithResources(
                DataHelper.CreateVariableStaticCall,
                DataHelper.CreateVariableStaticCallGold);
        }

        private void VerifyRefactoringWithResources(string testResource, string expectedResource)
        {
            int cursorPosition;
            string test = DataHelper.GetEmbeddedResource(testResource, out cursorPosition);
            string expected = DataHelper.GetEmbeddedResource(expectedResource);

            VerifyRefactoring(test, expected, TextSpan.FromBounds(cursorPosition, cursorPosition));
        }

        protected override CodeRefactoringProvider CreateProvider()
        {
            return new CreateVariableFromInvocationRefactoringProvider();
        }
    }
}