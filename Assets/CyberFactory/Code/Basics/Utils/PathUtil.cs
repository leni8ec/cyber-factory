using System;
using System.Text;

namespace CyberFactory.Basics.Utils {
    public static class PathUtil {

        public const char SEPARATOR = '/';
        private const char ALT_SEPARATOR = '\\';
        private const char VOLUME_SEPARATOR = ':';

        private static readonly StringBuilder SB = new();

        public static string[] Split(string path) {
            return path.Split(SEPARATOR, ALT_SEPARATOR);
        }

        public static string Combine(string path1, string path2, bool forced = false) {
            if (path1 == null || path2 == null)
                throw new ArgumentNullException((path1 == null) ? "path1" : "path2");

            return CombineInternal(path1, path2, forced);
        }

        public static string Combine(string path1, string path2, string path3, bool forced = false) {
            if (path1 == null || path2 == null || path3 == null)
                throw new ArgumentNullException((path1 == null) ? "path1" : (path2 == null) ? "path2" : "path3");

            return CombineInternal(CombineInternal(path1, path2, forced), path3, forced);
        }

        public static string Combine(string path1, string path2, string path3, string path4, bool forced = false) {
            if (path1 == null || path2 == null || path3 == null || path4 == null)
                throw new ArgumentNullException((path1 == null) ? "path1" : (path2 == null) ? "path2" : (path3 == null) ? "path3" : "path4");

            return CombineInternal(CombineInternal(CombineInternal(path1, path2, forced), path3, forced), path4, forced);
        }

        public static string Combine(string path1, string path2, string path3, string path4, string path5, bool forced = false) {
            if (path1 == null || path2 == null || path3 == null || path4 == null || path5 == null)
                throw new ArgumentNullException((path1 == null) ? "path1" : (path2 == null) ? "path2" : (path3 == null) ? "path3" : (path4 == null) ? "path4" : "path5");

            return CombineInternal(CombineInternal(CombineInternal(CombineInternal(path1, path2, forced), path3, forced), path4, forced), path5, forced);
        }

        public static string Combine(params string[] paths) {
            SB.Clear();
            SB.Append(paths[0]);
            for (int i = 1; i < paths.Length; i++) {
                ProcessSeparator(SB);
                SB.Append(paths[i]);
            }

            return SB.ToString();
        }

        /// <summary>
        /// Force add separator even if the path part is empty
        /// </summary>
        public static string CombineForced(params string[] paths) {
            SB.Clear();
            SB.Append(paths[0]);
            for (int i = 1; i < paths.Length; i++) {
                ProcessSeparator(SB, true);
                SB.Append(paths[i]);
            }

            return SB.ToString();
        }


        /// <param name="forced">force add separator even if the path part is empty</param>
        private static string CombineInternal(string path1, string path2, bool forced = false) {
            if (!forced) {
                if (path2.Length == 0)
                    return path1;
                if (path1.Length == 0)
                    return path2;
            }

            SB.Clear();
            SB.Append(path1);
            ProcessSeparator(SB, forced);
            SB.Append(path2);

            return SB.ToString();
        }

        /// <summary>
        /// Checks if there is a separator at the end of the path, if not, then adds it
        /// </summary>
        /// <param name="sb">path</param>
        /// <param name="forced">force add separator even if the path part is empty</param>
        private static void ProcessSeparator(StringBuilder sb, bool forced = false) {
            if (sb.Length == 0 && forced) {
                // add a separator, even if part of the path is empty (only in 'forced' mode)
                sb.Append(SEPARATOR);
            } else {
                char ch = sb[^1];
                if (ch == ALT_SEPARATOR) {
                    sb[^1] = SEPARATOR;
                } else if (ch != SEPARATOR) {
                    sb.Append(SEPARATOR);
                }
            }
        }

        /// <summary>
        /// Detect root path
        /// </summary>
        /// <returns>Return true if path started from '/'</returns>
        public static bool IsRoot(string path) {
            return path.StartsWith(SEPARATOR);
        }

    }
}