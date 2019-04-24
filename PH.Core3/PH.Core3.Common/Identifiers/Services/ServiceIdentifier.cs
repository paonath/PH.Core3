using Microsoft.Extensions.Logging;

namespace PH.Core3.Common.Identifiers.Services
{
    /// <summary>
    /// 
    /// </summary>
    public struct ServiceIdentifier
    {
        private readonly EventId _eventId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        public ServiceIdentifier(EventId eventId)
        {
            _eventId = eventId;
        }

        public ServiceIdentifier(int id, string name):this(new EventId(id,name))
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id => _eventId.Id;

        /// <summary>
        /// 
        /// </summary>
        public string Name => _eventId.Name;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public static implicit operator ServiceIdentifier(int i)
        {
            return new ServiceIdentifier(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceIdentifier"></param>
        public static implicit operator EventId(ServiceIdentifier serviceIdentifier)
        {
            return new EventId(serviceIdentifier.Id, serviceIdentifier.Name);
        }

        public static ServiceIdentifier operator +(ServiceIdentifier a, ServiceIdentifier b)
        {
            var name = $"{a.Name} [{b.Name}]";
            if (a.Id < b.Id)
                name = $"{b.Name} [{a.Name}]";

            return new ServiceIdentifier(a.Id + b.Id, name);
        }

        public static EventId operator +(ServiceIdentifier a, EventId b)
        {
           
            return new EventId(a.Id + b.Id, b.Name);
        }

        public static EventId operator +(EventId a, ServiceIdentifier b)
        {
            return new EventId(a.Id + b.Id, a.Name);
        }

        public static ServiceIdentifier operator -(ServiceIdentifier a, ServiceIdentifier b)
        {
            var name = $"{a.Name} [{b.Name}]";
            if (a.Id > b.Id)
                name = $"{b.Name} [{a.Name}]";

            return new ServiceIdentifier(a.Id - b.Id, name);
        }

        public static EventId operator -(ServiceIdentifier a, EventId b)
        {
            return new EventId(a.Id - b.Id, b.Name);
        }

        public static EventId operator -(EventId a, ServiceIdentifier b)
        {

            return new EventId(a.Id - b.Id, a.Name);
        }
    }
}