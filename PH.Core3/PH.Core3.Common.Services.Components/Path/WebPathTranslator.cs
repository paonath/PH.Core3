using System;
using System.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Services.Path;
// ReSharper disable IdentifierTypo

namespace PH.Core3.Common.Services.Components.Path
{
    
    /// <summary>
    /// Translate Web and FileSystem Path
    /// </summary>
    public class WebPathTranslator : IWebPathTranslator
    {
        private readonly string _webRootPath;
        private const string Root = "~/";
        private readonly ILogger<WebPathTranslator> _logger;

        /// <summary>
        /// Init new instance
        /// </summary>
        /// <param name="webRootPath">Root filesystem path of web root</param>
        /// <param name="logger">logger</param>
        /// <exception cref="ArgumentException"></exception>
        public WebPathTranslator([NotNull] string webRootPath, [CanBeNull] ILogger<WebPathTranslator> logger = null)
        {
            if (string.IsNullOrEmpty(webRootPath))
                throw new ArgumentException("Value cannot be null or empty.", nameof(webRootPath));
            if (string.IsNullOrWhiteSpace(webRootPath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(webRootPath));

            _webRootPath = webRootPath;
            _logger = logger;

            _logger?.LogDebug($"Web Root Path: '{_webRootPath}'");

        }

        #region Implementation of IWebPathTranslator

        /// <summary>
        /// Translate a Web-Relative Path to FileSystem Path
        /// </summary>
        /// <param name="webrelativePath">Web Path relative to the Root directory ~/ </param>
        /// <returns>File System Path</returns>
        [NotNull]
        public string ToFileSystemPath(string webrelativePath)
        {
            

            if (webrelativePath.StartsWith(Root, StringComparison.InvariantCulture))
                webrelativePath = webrelativePath.Replace(Root, $"{_webRootPath}{System.IO.Path.DirectorySeparatorChar}");

            var pt = webrelativePath.Replace("//", "/").Replace("/", $"{System.IO.Path.DirectorySeparatorChar}");
                                  
            pt = $"{_webRootPath}{System.IO.Path.DirectorySeparatorChar}{pt}";
            pt = pt.Replace($"{System.IO.Path.DirectorySeparatorChar}{System.IO.Path.DirectorySeparatorChar}",
                          $"{System.IO.Path.DirectorySeparatorChar}");

            _logger?.LogDebug($"PATH '{webrelativePath}' to '{pt}'");
            return pt;

        }

        /// <summary>
        /// Translate a FileSystem FileInfo to Web-Relative Path string 
        /// </summary>
        /// <param name="file">FileInfo</param>
        /// <returns>Web Path relative to the Root directory</returns>
        [NotNull]
        public string ToWebRelativePath([NotNull] FileInfo file)
        {
            return ToWeb(file.FullName);
        }

        /// <summary>
        /// Translate a FileSystem DirectoryInfo to Web-Relative Path string 
        /// </summary>
        /// <param name="directory">DirectoryInfo</param>
        /// <returns>Web Path relative to the Root directory</returns>
        [NotNull]
        public string ToWebRelativePath([NotNull] DirectoryInfo directory)
        {
            return ToWeb(directory.FullName);
        }

        [NotNull]
        private string ToWeb([NotNull] string fullPath)
        {
            var ff = fullPath.Replace(_webRootPath, Root);

            var r = ff.Replace($"{System.IO.Path.DirectorySeparatorChar}", "/").Replace("//", "/");

            _logger?.LogDebug($"PATH '{fullPath}' to '{r}'");
            return r;
        }

        /// <summary>
        /// Get FileInfo from Web-Relative Path string 
        /// </summary>
        /// <param name="webrelativePath"></param>
        /// <returns>FileInfo</returns>
        [NotNull]
        public FileInfo GetFile([NotNull] string webrelativePath)
        {
            return new FileInfo(ToFileSystemPath(webrelativePath));
        }

        /// <summary>
        /// Get DirectoryInfo from Web-Relative Path string 
        /// </summary>
        /// <param name="webrelativePath"></param>
        /// <returns>DirectoryInfo</returns>
        [NotNull]
        public DirectoryInfo GetDirectory([NotNull] string webrelativePath)
        {
            return new DirectoryInfo(ToFileSystemPath(webrelativePath));
        }

        #endregion
    }
}