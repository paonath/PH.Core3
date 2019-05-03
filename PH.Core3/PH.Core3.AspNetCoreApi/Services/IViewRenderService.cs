using System.Threading.Tasks;

namespace PH.Core3.AspNetCoreApi.Services
{
    /// <summary>
    /// Service for render view with given model as a html formatted string
    /// </summary>
    public interface IViewRenderService
    {
        /// <summary>
        /// Render a string from CSHTML View and Model
        /// </summary>
        /// <param name="viewWebPath">View Path (relative to web root)</param>
        /// <param name="model">Model instance</param>
        /// <returns>string</returns>
        Task<string> RenderToStringAsync(string viewWebPath, object model);

        /// <summary>
        /// Render a string from CSHTML View and Model
        /// </summary>
        /// <typeparam name="TModel">Type of Model Instance</typeparam>
        /// <param name="viewWebPath">View Path (relative to web root)</param>
        /// <param name="model">Model instance</param>
        /// <returns>string</returns>
        Task<string> RenderToStringAsync<TModel>(string viewWebPath, TModel model) where TModel : class;
    }
}