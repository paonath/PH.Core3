using System;
using PH.Core3.Common.Models.ViewModels;

namespace PH.Core3.Test.WebApp.Services
{

    public class NewTestDataDto : INewDto
    {
        public string data  { get; set; }
  
    }

    public class EditTestDataDto : NewTestDataDto, IEditDto<Guid>
    {
        /// <summary>
        /// Unique Id of current class
        /// </summary>
        public Guid Id { get; set; }
    }

    public class TestDataDto : EditTestDataDto, IDtoResult<Guid>
    {
        

        /// <summary>Gets the UTC last updated date and time for current entity.</summary>
        /// <value>The UTC last updated.</value>
        public DateTime? UtcLastUpdated { get; set; }
    }
}