using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Text;

namespace NetChat2.Connector.Tests
{
    [TestFixture]
    public class FileReadHelperTests
    {
        private string filePath;
        private string filePathBigSize;
        private int BIGFILE_SIZE = (int)1e6;
        private Encoding encoding = Encoding.GetEncoding(1251);

        [Test]
        public void ReadLastLine_SingleLineWithNewLine()
        {
            File.AppendAllText(filePath, "line1\n");

            string readedline = FileReadHelper.ReadLastLine(filePath, encoding);

            Assert.NotNull(readedline);
            Assert.AreEqual("line1", readedline);
        }

        [Test]
        public void ReadLastLine_SingleLineWithoutNewLine()
        {
            File.AppendAllText(filePath, "line1");

            string readedline = FileReadHelper.ReadLastLine(filePath, encoding);

            Assert.NotNull(readedline);
            Assert.AreEqual("line1", readedline);
        }

        [Test]
        public void ReadLastLine_LastLineEndWithNewLine()
        {
            File.AppendAllText(filePath, "line1\n");
            File.AppendAllText(filePath, "line2\n");

            string readedline = FileReadHelper.ReadLastLine(filePath, encoding);

            Assert.NotNull(readedline);
            Assert.AreEqual("line2", readedline);
        }

        [Test]
        public void ReadLastLine_LastLineEndWithoutNewLine()
        {
            File.AppendAllText(filePath, "line1\n");
            File.AppendAllText(filePath, "line2");

            string readedline = FileReadHelper.ReadLastLine(filePath, encoding);

            Assert.NotNull(readedline);
            Assert.AreEqual("line2", readedline);
        }

        [Test]
        public void ReadLastLine_FasterThanDumbMethod()
        {
            var dumb = new DiagnosingResult<string>(() => GetLastLine_DumbMethod(filePathBigSize));
            var test = new DiagnosingResult<string>(() => GetLastLine_TestMethod(filePathBigSize));
            dumb.Runtest();
            test.Runtest();

            Console.WriteLine($"Speed results: Dumb method = {dumb.ElapsedTime} ms. vs Testing method = {test.ElapsedTime} ms.\n");
            Assert.NotNull(dumb.Result);
            Assert.NotNull(test.Result);
            Assert.Greater(dumb.ElapsedTime, test.ElapsedTime);
        }

        //[Test]
        //public void ReadAllLines_BigFile()
        //{
        //    var lines = FileReadHelper.ReadAllLines(filePathBigSize, encoding);

        //    Assert.NotNull(lines);
        //    Assert.AreEqual(BIGFILE_SIZE, lines.Length);
        //    Assert.AreEqual($"testline-{(BIGFILE_SIZE - 1)}", lines[lines.Length - 1]);
        //}

        [Test]
        public async Task ReadAllLinesAsync_BigFile()
        {
            var lines = await FileReadHelper.ReadAllLinesAsync(filePathBigSize, encoding);

            Assert.NotNull(lines);
            Assert.AreEqual(BIGFILE_SIZE, lines.Length);
            Assert.AreEqual($"testline-{(BIGFILE_SIZE - 1)}", lines[lines.Length - 1]);
        }

        [Test]
        public void ReadAllLines_N50BigFile()
        {
            var lines = FileReadHelper.ReadAllLines(filePathBigSize, encoding, 50);

            Assert.NotNull(lines);
            Assert.AreEqual(50, lines.Length);
            Assert.AreEqual($"testline-{(BIGFILE_SIZE - 1 - 49)}", lines[0]);
            Assert.AreEqual($"testline-{(BIGFILE_SIZE - 1)}", lines[49]);
        }

        [Test]
        public async Task ReadAllLinesAsync_N50BigFile()
        {
            var lines = await FileReadHelper.ReadAllLinesAsync(filePathBigSize, encoding, 50);

            Assert.NotNull(lines);
            Assert.AreEqual(50, lines.Length);
            Assert.AreEqual($"testline-{(BIGFILE_SIZE - 1 - 49)}", lines[0]);
            Assert.AreEqual($"testline-{(BIGFILE_SIZE - 1)}", lines[49]);
        }
        [Test]
        public void ReadAllLinesAsync_Cancell_ThrowException()
        {
            CancellationTokenSource s = new CancellationTokenSource();
            s.CancelAfter(2);
            Assert.ThrowsAsync<OperationCanceledException>(async () => await FileReadHelper.ReadAllLinesAsync(filePathBigSize, encoding, 50, s.Token));
        }

        [Test]
        public void ReadAllLines_N50SmallFile()
        {
            GenerateFile(filePath, 10);
            
            var lines = FileReadHelper.ReadAllLines(filePath, encoding, 50);

            Assert.NotNull(lines);
            Assert.AreEqual(10, lines.Length);
            Assert.AreEqual($"testline-0", lines[0]);
            Assert.AreEqual($"testline-9", lines[9]);
        }

        [Test]
        public async Task ReadAllLinesAsync_N50SmallFile()
        {
            GenerateFile(filePath, 10);
            
            var lines = await FileReadHelper.ReadAllLinesAsync(filePath, encoding, 50);

            Assert.NotNull(lines);
            Assert.AreEqual(10, lines.Length);
            Assert.AreEqual($"testline-0", lines[0]);
            Assert.AreEqual($"testline-9", lines[9]);
        }


        [OneTimeSetUp]
        public void InitFixture()
        {
            filePathBigSize = Path.Combine(Directory.GetCurrentDirectory(), $"data{BIGFILE_SIZE}.txt");
            GenerateFile(filePathBigSize, BIGFILE_SIZE);
        }

        [OneTimeTearDown]
        public void CleanupFixture()
        {
            File.Delete(filePathBigSize);
        }

        [SetUp]
        public void Init()
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), $"fllr_tests.txt");
        }

        [TearDown]
        public void Cleanup()
        {
            File.Delete(filePath);
        }


        private void GenerateFile(string path, long key)
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
            return FileReadHelper.ReadLastLine(path, encoding);
        }
        private string GetLastLine_DumbMethod(string path)
        {
            string res = null;
            using (StreamReader sr = new StreamReader(path, encoding))
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


    public class DiagnosingResult<TRes>
    {
        private Func<TRes> _func;
        public TRes Result { get; set; }
        public double ElapsedTime { get; set; }

        public DiagnosingResult(Func<TRes> func)
        {
            _func = func;
        }

        public void Runtest()
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            Result = _func();
            w.Stop();
            ElapsedTime = w.ElapsedMilliseconds;
        }
    }
}