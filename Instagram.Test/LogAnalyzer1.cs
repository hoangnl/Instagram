using System;
using NUnit.Framework;
using NSubstitute;

namespace TM.DemoUnitTest
{
    public class LogAnalyzer1
    {
        private ILogger _logger;

        public LogAnalyzer1(ILogger logger)
        {
            _logger = logger;
        }

        public int MinNameLength { get; set; }

        public void Analyze(string filename)
        {
            if (filename.Length < MinNameLength)
            {
                _logger.LogError(string.Format("Filename too short: {0}", filename));
            }
        }
    }

    public class LogAnalyzer2
    {
        private ILogger _logger;
        private IWebService _webService;


        public LogAnalyzer2(ILogger logger, IWebService webService)
        {
            _logger = logger;
            _webService = webService;
        }

        public int MinNameLength { get; set; }

        public void Analyze(string filename)
        {
            if (filename.Length < MinNameLength)
            {
                try
                {
                    _logger.LogError(string.Format("Filename too short: {0}", filename));
                }
                catch (Exception e)
                {
                    _webService.Write("Error From Logger: " + e);

                }
            }
        }
    }
    public class LogAnalyzer3
    {
        private ILogger _logger;
        private IWebService _webService;


        public LogAnalyzer3(ILogger logger, IWebService webService)
        {
            _logger = logger;
            _webService = webService;
        }

        public int MinNameLength { get; set; }

        public void Analyze(string filename)
        {
            if (filename.Length < MinNameLength)
            {
                try
                {
                    _logger.LogError(string.Format("Filename too short: {0}", filename));
                }
                catch (Exception e)
                {
                    _webService.Write(new ErrorInfo(1000, e.Message));

                }
            }
        }
    }

    public class ErrorInfo
    {
        private readonly int _severity;
        private readonly string _message;

        public ErrorInfo(int severity, string message)
        {
            _severity = severity;
            _message = message;
        }

        public int Severity
        {
            get { return _severity; }
        }

        public string Message
        {
            get { return _message; }
        }

        protected bool Equals(ErrorInfo other)
        {
            return _severity == other._severity && string.Equals(_message, other._message);
        }

        //this is needed to make this test pass:
        // Analyze_LoggerThrows_CallsWebServiceWithNSubObjectCompare
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ErrorInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_severity * 397) ^ _message.GetHashCode();
            }
        }
    }

    public interface IWebService
    {
        void Write(string message);
        void Write(ErrorInfo message);
    }

    public interface ILogger
    {
        void LogError(string message);

    }

    public interface IFileNameRules
    {
        bool IsValidLogFileName(string fileName);
    }

    class FakeLogger : ILogger
    {
        public string LastError;

        public void LogError(string message)
        {
            LastError = message;
        }
    }

    [TestFixture]
    class TestsWithoutAnIsolationFramework
    {
        [Test]
        public void Analyze_TooShortFileName_CallLogger()
        {
            FakeLogger logger = new FakeLogger();

            LogAnalyzer1 analyzer = new LogAnalyzer1(logger);

            analyzer.MinNameLength = 6;
            analyzer.Analyze("a.txt");

            StringAssert.Contains("too short", logger.LastError);
        }
    }

    public interface IPerson
    {
        IPerson GetManager();
    }


    [TestFixture]
    public class NSubBasics
    {
        [Test]
        public void SubstituteFor_ForInterfaces_ReturnsAFakeInterface()
        {
            IFileNameRules fakeRules = Substitute.For<IFileNameRules>();

            Assert.IsFalse(fakeRules.IsValidLogFileName("something.bla"));
        }
        [Test]
        public void Returns_ArgAny_IgnoresArgument()
        {
            IFileNameRules fakeRules = Substitute.For<IFileNameRules>();

            fakeRules.IsValidLogFileName(Arg.Any<string>()).Returns(true);

            Assert.IsTrue(fakeRules.IsValidLogFileName("anything, really"));
        }

        [Test]
        public void Returns_ArgAny_Throws()
        {
            IFileNameRules fakeRules = Substitute.For<IFileNameRules>();

            fakeRules.When(x => x.IsValidLogFileName(Arg.Any<string>()))
                     .Do(x => { throw new Exception("fake exception"); });


            Assert.Throws<Exception>(() =>
                                     fakeRules.IsValidLogFileName("anything"));

        }


        [Test]
        public void Returns_ByDefault_WorksForHardCodedArgument()
        {
            IFileNameRules fakeRules = Substitute.For<IFileNameRules>();

            fakeRules.IsValidLogFileName("file.name").Returns(true);

            Assert.IsTrue(fakeRules.IsValidLogFileName("file.name"));
        }


        [Test]
        public void RecursiveFakes_work()
        {
            IPerson p = Substitute.For<IPerson>();

            Assert.IsNotNull(p.GetManager());
            Assert.IsNotNull(p.GetManager().GetManager());
            Assert.IsNotNull(p.GetManager().GetManager().GetManager());
        }

        public interface IPerson
        {
            IPerson GetManager();
        }
    }


    [TestFixture]
    class LogAnalyzerTests1
    {
        [Test]
        public void Analyze_TooShortFileName_CallLogger()
        {
            ILogger logger = Substitute.For<ILogger>();

            LogAnalyzer1 analyzer = new LogAnalyzer1(logger);

            analyzer.MinNameLength = 6;
            analyzer.Analyze("a.txt");

            logger.Received().LogError("Filename too short: a.txt");
        }

        [Test]
        public void Analyze_TooShortFileName_CallLoggerArgMatchers()
        {
            ILogger logger = Substitute.For<ILogger>();

            LogAnalyzer1 analyzer = new LogAnalyzer1(logger);

            analyzer.MinNameLength = 6;
            analyzer.Analyze("a.txt");

            logger.Received().LogError(Arg.Is<string>(s => s.Contains("too short")));
        }

        [Test]
        public void RecursiveFakes()
        {
            IPerson fake = Substitute.For<IPerson>();

            Assert.IsNotNull(fake.GetManager());
            Assert.IsNotNull(fake.GetManager().GetManager().GetManager());
        }
    }


    //public interface IPerson
    //{
    //    IPerson GetManager();
    //}

    [TestFixture]
    class LogAnalyzerTestsWithHandWrittenFakes
    {
        [Test]
        public void Analyze_LoggerThrows_CallsWebService()
        {
            FakeWebService mockWebService = new FakeWebService();

            FakeLogger2 stubLogger = new FakeLogger2();
            stubLogger.WillThrow = new Exception("fake exception");

            LogAnalyzer2 analyzer2 = new LogAnalyzer2(stubLogger, mockWebService);
            analyzer2.MinNameLength = 8;

            string tooShortFileName = "abc.ext";
            analyzer2.Analyze(tooShortFileName);

            Assert.That(mockWebService.MessageToWebService,
                            Is.StringContaining("fake exception"));
        }

        [Test]
        public void Analyze_LoggerThrows_CallsWebServiceWithNSub()
        {
            var mockWebService = Substitute.For<IWebService>();
            var stubLogger = Substitute.For<ILogger>();
            stubLogger.When(
                logger => logger.LogError(Arg.Any<string>()))
                .Do(info => { throw new Exception("fake exception"); });

            var analyzer =
               new LogAnalyzer2(stubLogger, mockWebService);

            analyzer.MinNameLength = 10;
            analyzer.Analyze("Short.txt");

            mockWebService.Received()
             .Write(Arg.Is<string>(s => s.Contains("fake exception")));
        }
        [Test]
        public void Analyze_LoggerThrows_CallsWebServiceWithNSubObject()
        {
            var mockWebService = Substitute.For<IWebService>();
            var stubLogger = Substitute.For<ILogger>();
            stubLogger.When(
                logger => logger.LogError(Arg.Any<string>()))
                .Do(info => { throw new Exception("fake exception"); });

            var analyzer =
               new LogAnalyzer3(stubLogger, mockWebService);

            analyzer.MinNameLength = 10;
            analyzer.Analyze("Short.txt");

            mockWebService.Received()
             .Write(Arg.Is<ErrorInfo>(info => info.Severity == 1000
                 && info.Message.Contains("fake exception")));
        }

        [Test]
        public void Analyze_LoggerThrows_CallsWebServiceWithNSubObjectCompare()
        {
            var mockWebService = Substitute.For<IWebService>();
            var stubLogger = Substitute.For<ILogger>();
            stubLogger.When(
                logger => logger.LogError(Arg.Any<string>()))
                .Do(info => { throw new Exception("fake exception"); });

            var analyzer =
               new LogAnalyzer3(stubLogger, mockWebService);

            analyzer.MinNameLength = 10;
            analyzer.Analyze("Short.txt");

            var expected = new ErrorInfo(1000, "fake exception");
            mockWebService.Received().Write(expected);
        }

    }
    public class FakeWebService : IWebService
    {
        public string MessageToWebService;

        public void Write(string message)
        {
            MessageToWebService = message;
        }

        public void Write(ErrorInfo message)
        {

        }
    }

    public class FakeLogger2 : ILogger
    {
        public Exception WillThrow = null;
        public string LoggerGotMessage = null;


        public void LogError(string message)
        {
            LoggerGotMessage = message;
            if (WillThrow != null)
            {
                throw WillThrow;
            }
        }
    }

}




