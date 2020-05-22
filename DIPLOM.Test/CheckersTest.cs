using System;
using DIPLOM.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DIPLOM.Test
{
    [TestClass]
    public class CheckersTest
    {
        [TestMethod]
        public void PriceValidation_11_111_false()
        {
            bool actual = Checkers.PriceValidation("11.111");
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void PriceValidation_22_2_true()
        {
            bool actual = Checkers.PriceValidation("22,2");
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void AmountValidation_SpecSymbols_false()
        {
            bool actual = Checkers.AmountValidation("@!#$%^");
            Assert.AreEqual(false, actual);
        }
    }
}
