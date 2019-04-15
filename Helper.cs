using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Po.Utilities
{
    /// <summary>
    /// Contains various helpful methods.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Executes the given action asynchronously using <see cref="Thread"/> class.
        /// Just use <see cref="Task.Run(Action)"/> instead!
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="apartmentState"><see cref="Thread"/> instance's apartment state.</param>
        public static void RunAsync(Action action, ApartmentState apartmentState = ApartmentState.STA)
        {
            var thread = new Thread(() =>
            {
                action();
            });
            thread.SetApartmentState(apartmentState);
            thread.Start();
        }

        /// <summary>
        /// Returns whether the given directory path is valid.
        /// </summary>
        public static bool IsDirectoryPathValid(string path)
        {
            try
            {
                string[] directories = path?.TrimEnd('\\').Split('\\');
                return
                    !string.IsNullOrEmpty(path)
                    && !path.Where(e => Path.GetInvalidPathChars().Contains(e) && e != '\\').Any()
                    && directories.Skip(1).Where(e => !string.IsNullOrEmpty(e)).Count() == directories.Length - 1
                    && Directory.Exists(directories[0] + "\\");
            }
            catch { }
            return false;
        }
        /// <summary>
        /// Returns whether the given file name is valid.
        /// </summary>
        public static bool IsFileNameValid(string fileName)
        {
            try
            {
                return
                    !string.IsNullOrEmpty(fileName)
                    && !fileName.Where(e => Path.GetInvalidFileNameChars().Contains(e)).Any()
                    && !fileName.Contains('\\')
                    && !fileName.Contains(',');
            }
            catch { }
            return false;
        }
        /// <summary>
        /// Returns whether the given file path is valid.
        /// </summary>
        public static bool IsFilePathValid(string filePath)
        {
            try
            {
                string[] levels = filePath?.Split('\\');
                return
                    !string.IsNullOrEmpty(filePath)
                    && IsDirectoryPathValid(string.Concat(levels.Take(levels.Length - 1).Select(e => e + '\\')))
                    && IsFileNameValid(levels.Last());
            }
            catch { }
            return false;
        }
    }
}