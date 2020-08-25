using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace NetChat2.Connector.Tests
{
    [TestFixture]
    public class FileLastLineReaderTests
    {
        private string path;

        [SetUp]
        public void Init() => path = Path.Combine(Directory.GetCurrentDirectory(), $"fllr_tests.txt");

        [TearDown]
        public void Cleanup() => File.Delete(path);


        [Test]
        public void SingleLineWithNewLine()
        {
            File.AppendAllText(path, "line1\n");

            string readedline = FileLastLineReader.ReadLastLine(path);

            Assert.NotNull(readedline);
            Assert.AreEqual("line1", readedline);
        }

        [Test]
        public void SingleLineWithoutNewLine()
        {
            File.AppendAllText(path, "line1");

            string readedline = FileLastLineReader.ReadLastLine(path);

            Assert.NotNull(readedline);
            Assert.AreEqual("line1", readedline);
        }

        [Test]
        public void LastLineEndWithNewLine()
        {
            File.AppendAllText(path, "line1\n");
            File.AppendAllText(path, "line2\n");

            string readedline = FileLastLineReader.ReadLastLine(path);

            Assert.NotNull(readedline);
            Assert.AreEqual("line2", readedline);
        }

        [Test]
        public void LastLineEndWithoutNewLine()
        {
            File.AppendAllText(path, "line1\n");
            File.AppendAllText(path, "line2");

            string readedline = FileLastLineReader.ReadLastLine(path);

            Assert.NotNull(readedline);
            Assert.AreEqual("line2", readedline);
        }

        [Test]
        public void FasterThanDumbMethod()
        {
            GenerateFile((int)1e7);
            var dumb = new DiagnosingResult(GetLastLine_DumbMethod);
            var test = new DiagnosingResult(GetLastLine_TestMethod);
            dumb.Runtest(path);
            test.Runtest(path);

            Assert.NotNull(dumb.Result);
            Assert.NotNull(test.Result);
            Console.WriteLine($"Speed results: Dumb method = {dumb.ElapsedTime} ms. vs Testing method = {test.ElapsedTime} ms.\n");
            Assert.Greater(dumb.ElapsedTime, test.ElapsedTime);
        }

        private void GenerateFile(long key)
        {
            using (var fs = File.OpenWrite(path))
            using (var w = new StreamWriter(fs))
            {
                for (var i = 0; i < key; i++)
                    w.WriteLine($"testline-{i}");
            }
        }
        private string GetLastLine_TestMethod(string path)
        {
            return FileLastLineReader.ReadLastLine(path);
        }
        private string GetLastLine_DumbMethod(string path)
        {
            string res = null;
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (sr.Peek() == -1)
                    {
                        res = line;
                        break;
                    }
                }
            }
            return res;
        }
    }


    public delegate string FileReadTestingFunction(string path);

    public class DiagnosingResult
    {
        private FileReadTestingFunction _func;
        public string Result { get; set; }
        public double ElapsedTime { get; set; }

        public DiagnosingResult(FileReadTestingFunction func)
        {
            _func = func;
        }

        public void Runtest(string path)
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            Result = _func(path);
            w.Stop();
            ElapsedTime = w.ElapsedMilliseconds;
        }
    }
}