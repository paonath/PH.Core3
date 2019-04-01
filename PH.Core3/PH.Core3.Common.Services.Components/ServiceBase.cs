using System;

namespace PH.Core3.Common.Services.Components
{
    public abstract class ServiceBase : IService
    {
        protected ServiceBase(IIdentifier identifier)
        {
            Identifier = identifier;
        }

        public IIdentifier Identifier { get; }
        
    }
}
