using Microsoft.CodeAnalysis.CodeRefactorings;
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

        [TestMethod]
        public void CreatesVariableForConstructorCallTyped()
        {
            VerifyRefactoringWithResources(
                DataHelper.CreateVariableConstructorCall,
                DataHelper.CreateVariableConstructorCallGoldTyped,
                1);
        }

        [TestMethod]
        public void CreatesVariableForFluentApiCallsTyped()
        {
            VerifyRefactoringWithResources(
                DataHelper.CreateVariableFluentApiCalls,
                DataHelper.CreateVariableFluentApiCallsGoldTyped,
                1);
        }

        [TestMethod]
        public void CreatesVariableForPropertyCallTyped()
        {
            VerifyRefactoringWithResources(
                DataHelper.CreateVariablePropertyCall,
                DataHelper.CreateVariablePropertyCallGoldTyped,
                1);
        }

        [TestMethod]
        public void CreatesVariableForStaticCallTyped()
        {
            VerifyRefactoringWithResources(
                DataHelper.CreateVariableStaticCall,
                DataHelper.CreateVariableStaticCallGoldTyped,
                1);
        }

        [TestMethod]
        public void IgnoresVoidMethods()
        {
            VerifyNoRefactoring(DataHelper.CreateVariableVoidCall);
        }

        protected override CodeRefactoringProvider CreateProvider()
        {
            return new CreateVariableFromInvocationRefactoringProvider();
        }
    }
}