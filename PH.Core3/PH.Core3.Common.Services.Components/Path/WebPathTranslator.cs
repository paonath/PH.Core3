using System;
using System.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Services.Path;
// ReSharper disable IdentifierTypo

namespace PH.Core3.Common.Services.Components.Path
{
    public class WebPathTranslator : IWebPathTranslator
    {
        private readonly string _webRootPath;
        private const string Root = "~/";
        private readonly ILogger<WebPathTranslator> _logger;

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

         
        [NotNull]
        public string ToFileSystemPath(string webrelativePath)
        {
            

            if (webrelativePath.StartsWith(Root, StringComparison.InvariantCulture))
                webrelativePath = webrelativePath.Replace(Root, $"{_webRootPath}{System.IO.Path.DirectorySeparatorChar}");

            var pt = webrelativePath.Replace("//", "/").Replace("/", $"{System.IO.Path.DirectorySeparatorChar}")
                                    .Replace($"{System.IO.Path.DirectorySeparatorChar}{System.IO.Path.DirectorySeparatorChar}",
                                             $"{System.IO.Path.DirectorySeparatorChar}");
            _logger?.LogDebug($"PATH '{webrelativePath}' to '{pt}'");
            return pt;

        }

        [NotNull]
        public string ToWebRelativePath([NotNull] FileInfo file)
        {
            return ToWeb(file.FullName);
        }

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

        [NotNull]
        public FileInfo GetFile([NotNull] string webrelativePath)
        {
            return new FileInfo(ToFileSystemPath(webrelativePath));
        }

        [NotNull]
        public DirectoryInfo GetDirectory([NotNull] string webrelativePath)
        {
            return new DirectoryInfo(ToFileSystemPath(webrelativePath));
        }

        #endregion
    }
}