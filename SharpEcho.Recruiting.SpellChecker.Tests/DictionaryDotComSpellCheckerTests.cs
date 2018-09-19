using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;

using SharpEcho.Recruiting.SpellChecker.Contracts;
using SharpEcho.Recruiting.SpellChecker.Core;

namespace SharpEcho.Recruiting.SpellChecker.Tests
{
    [TestFixture]
    class DictionaryDotComSpellCheckerTests
    {
        private ISpellChecker SpellChecker;
        private Mock<HttpMessageHandler> FoundHandler;
        private Mock<HttpMessageHandler> NotFoundHandler;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            FoundHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            FoundHandler
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();
            
            NotFoundHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            NotFoundHandler
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound
                })
                .Verifiable();
            
            SpellChecker = new DictionaryDotComSpellChecker(new HttpClient(FoundHandler.Object));
        }

        [Test]
        public void Check_That_Method_Queries_DictionaryDotCom()
        {
            var expectedUri = new Uri("http://dictionary.com/browse/Anything");
            SpellChecker.Check("Anything");
            FoundHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get  // we expected a GET request
                        && req.RequestUri == expectedUri // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public void Check_That_200OK_Is_Correct()
        {
            Assert.IsTrue(SpellChecker.Check("anything"));
        }
        
        [Test]
        public void Check_That_Any_Other_Response_Is_Misspelled()
        {
            var spellChecker = new DictionaryDotComSpellChecker(new HttpClient(NotFoundHandler.Object));
            
            Assert.IsFalse(spellChecker.Check("anything"));
        }

        [Test]
        [Ignore("This is an integration test that will hit a real endpoint")]
        public void Check_That_SharpEcho_Is_Misspelled()
        {
            var spellChecker = new DictionaryDotComSpellChecker();
            Assert.IsFalse(spellChecker.Check("SharpEcho"));
        }

        [Test]
        [Ignore("This is an integration test that will hit a real endpoint")]
        public void Check_That_South_Is_Not_Misspelled()
        {
            var spellChecker = new DictionaryDotComSpellChecker();
            Assert.IsTrue(spellChecker.Check("South"));

        }
    }
}
