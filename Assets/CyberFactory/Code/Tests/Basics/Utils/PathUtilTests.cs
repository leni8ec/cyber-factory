using CyberFactory.Basics.Utils;
using NUnit.Framework;

namespace CyberFactory.Tests.Basics.Utils {
    public class PathUtilTests {

        [Test]
        public void Combine() {
            string combine1 = PathUtil.Combine("path1");
            string combine2 = PathUtil.Combine("path1", "path2");
            string combine3 = PathUtil.Combine("path1", "path2", "path3");
            string combine4 = PathUtil.Combine("path1", "path2", "path3", "path4");
            string combine5 = PathUtil.Combine("path1", "path2", "path3", "path4", "path5");
            string combine6 = PathUtil.Combine("path1", "path2", "path3", "path4", "path5", "path6");
            string combine7 = PathUtil.Combine("path1", "path2", "path3", "path4", "path5", "path6", "path7");
            string combine11 = PathUtil.Combine("", "path2");
            string combine12 = PathUtil.Combine(" ", "path2");

            Assert.AreEqual(combine1, "path1");
            Assert.AreEqual(combine2, "path1/path2");
            Assert.AreEqual(combine3, "path1/path2/path3");
            Assert.AreEqual(combine4, "path1/path2/path3/path4");
            Assert.AreEqual(combine5, "path1/path2/path3/path4/path5");
            Assert.AreEqual(combine6, "path1/path2/path3/path4/path5/path6");
            Assert.AreEqual(combine7, "path1/path2/path3/path4/path5/path6/path7");
            Assert.AreEqual(combine11, "path2");
            Assert.AreEqual(combine12, " /path2");
        }

        [Test]
        public void CombineForced() {
            string combine1 = PathUtil.CombineForced("path1");
            string combine11 = PathUtil.Combine("", "path2", true);
            string combine12 = PathUtil.Combine(" ", "path2", true);

            Assert.AreEqual(combine1, "path1");
            Assert.AreEqual(combine11, "/path2");
            Assert.AreEqual(combine12, " /path2");
        }

        [TestCase("/path1", ExpectedResult = new[] { "", "path1" })]
        [TestCase("path1", ExpectedResult = new[] { "path1" })]
        [TestCase("path1/path2", ExpectedResult = new[] { "path1", "path2" })]
        [TestCase("path1/path2/path3", ExpectedResult = new[] { "path1", "path2", "path3" })]
        public string[] Split(string path) {
            return PathUtil.Split(path);
        }

    }
}