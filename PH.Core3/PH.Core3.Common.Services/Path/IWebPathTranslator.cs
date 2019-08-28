//using System.IO;
//using JetBrains.Annotations;
//// ReSharper disable IdentifierTypo

//namespace PH.Core3.Common.Services.Path
//{
//    /// <summary>
//    /// Translate Web and FileSystem Path
//    /// </summary>
//    public interface IWebPathTranslator
//    {
//        /// <summary>
//        /// Translate a Web-Relative Path to FileSystem Path
//        /// </summary>
//        /// <param name="webrelativePath">Web Path relative to the Root directory ~/ </param>
//        /// <returns>File System Path</returns>
//        string ToFileSystemPath([NotNull] string webrelativePath);

//        /// <summary>
//        /// Translate a FileSystem FileInfo to Web-Relative Path string 
//        /// </summary>
//        /// <param name="file">FileInfo</param>
//        /// <returns>Web Path relative to the Root directory</returns>
//        string ToWebRelativePath([NotNull] FileInfo file);

//        /// <summary>
//        /// Translate a FileSystem DirectoryInfo to Web-Relative Path string 
//        /// </summary>
//        /// <param name="directory">DirectoryInfo</param>
//        /// <returns>Web Path relative to the Root directory</returns>
//        string ToWebRelativePath([NotNull] DirectoryInfo directory);


//        /// <summary>
//        /// Get FileInfo from Web-Relative Path string 
//        /// </summary>
//        /// <param name="webrelativePath"></param>
//        /// <returns>FileInfo</returns>
//        FileInfo GetFile([NotNull] string webrelativePath);

//        /// <summary>
//        /// Get DirectoryInfo from Web-Relative Path string 
//        /// </summary>
//        /// <param name="webrelativePath"></param>
//        /// <returns>DirectoryInfo</returns>
//        DirectoryInfo GetDirectory([NotNull] string webrelativePath);



//    }
//}