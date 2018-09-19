using NUnit.Framework;

using SharpEcho.Recruiting.SpellChecker.Contracts;
using SharpEcho.Recruiting.SpellChecker.Core;

namespace SharpEcho.Recruiting.SpellChecker.Tests
{
    [TestFixture]
    class MnemonicSpellCheckerIBeforeETests
    {
        private ISpellChecker SpellChecker;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            SpellChecker = new MnemonicSpellCheckerIBeforeE();
        }
        
        [Test]
        public void Check_That_IE_Is_Correct()
        {
            Assert.IsTrue(SpellChecker.Check("believe"));
            Assert.IsTrue(SpellChecker.Check("fierce"));
            Assert.IsTrue(SpellChecker.Check("collie"));
            Assert.IsTrue(SpellChecker.Check("die"));
            Assert.IsTrue(SpellChecker.Check("friend"));
            Assert.IsTrue(SpellChecker.Check("pie"));
        }

        [Test]
        public void Check_That_EI_Is_Incorrect()
        {
            Assert.IsFalse(SpellChecker.Check("heir"));
            Assert.IsFalse(SpellChecker.Check("protein"));
            Assert.IsFalse(SpellChecker.Check("seeing"));
            Assert.IsFalse(SpellChecker.Check("their"));
            Assert.IsFalse(SpellChecker.Check("veil"));
            Assert.IsFalse(SpellChecker.Check("einstein"));
            Assert.IsFalse(SpellChecker.Check("ceinstein"));
        }

        [Test]
        public void Check_That_CEI_Is_Correct()
        {
            Assert.IsTrue(SpellChecker.Check("ceiling"));
            Assert.IsTrue(SpellChecker.Check("deceive"));
            Assert.IsTrue(SpellChecker.Check("receipt"));
        }

        [Test]
        public void Check_CIE_Is_Incorrect()
        {
            Assert.IsFalse(SpellChecker.Check("science"));
            Assert.IsFalse(SpellChecker.Check("wierdscience"));
        }

        [Test]
        public void Check_Words_Without_I_Or_E()
        {
            Assert.IsTrue(SpellChecker.Check("pool"));
            Assert.IsTrue(SpellChecker.Check("fly"));
            Assert.IsTrue(SpellChecker.Check(""));
            Assert.IsTrue(SpellChecker.Check("tea"));
            Assert.IsTrue(SpellChecker.Check("pin"));
        }
    }
}
