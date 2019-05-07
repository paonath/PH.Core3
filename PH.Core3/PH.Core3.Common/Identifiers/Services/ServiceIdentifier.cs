using Microsoft.Extensions.Logging;

namespace PH.Core3.Common.Identifiers.Services
{
    /// <summary>
    /// Service Identifier
    /// </summary>
    public struct ServiceIdentifier
    {
        private readonly EventId _eventId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceIdentifier"/> struct.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        public ServiceIdentifier(EventId eventId)
        {
            _eventId = eventId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceIdentifier"/> struct.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public ServiceIdentifier(int id, string name):this(new EventId(id,name))
        {
            
        }

        /// <summary>
        /// Service Id
        /// </summary>
        public int Id => _eventId.Id;

        /// <summary>
        /// Service Name
        /// </summary>
        public string Name => _eventId.Name;


        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="ServiceIdentifier"/>.
        /// </summary>
        /// <param name="i">The identifier.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ServiceIdentifier(int i)
        {
            return new ServiceIdentifier(i);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ServiceIdentifier"/> to <see cref="EventId"/>.
        /// </summary>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator EventId(ServiceIdentifier serviceIdentifier)
        {
            return new EventId(serviceIdentifier.Id, serviceIdentifier.Name);
        }

        /// <summary>Implements the operator +.</summary>
        /// <param name="a">Service Identifier a.</param>
        /// <param name="b">Service Identifier b.</param>
        /// <returns>The result of the operator.</returns>
        public static ServiceIdentifier operator +(ServiceIdentifier a, ServiceIdentifier b)
        {
            var name = $"{a.Name} [{b.Name} - {b.Id}]";
            if (a.Id < b.Id)
            {
                name = $"{b.Name} [{a.Name} - {a.Id}]";
            }

            return new ServiceIdentifier(a.Id + b.Id, name);
        }

        /// <summary>Implements the operator +.</summary>
        /// <param name="a">ServiceIdentifier a.</param>
        /// <param name="b">The EventId b.</param>
        /// <returns>The result of the operator.</returns>
        public static EventId operator +(ServiceIdentifier a, EventId b)
        {
           
            return new EventId(a.Id + b.Id, b.Name);
        }

        /// <summary>Implements the operator +.</summary>
        /// <param name="a">EventId a.</param>
        /// <param name="b">The ServiceIdentifier b.</param>
        /// <returns>The result of the operator.</returns>
        public static EventId operator +(EventId a, ServiceIdentifier b)
        {
            return new EventId(a.Id + b.Id, a.Name);
        }

        /// <summary>Implements the operator -.</summary>
        /// <param name="a">ServiceIdentifier a.</param>
        /// <param name="b">ServiceIdentifier b.</param>
        /// <returns>The result of the operator.</returns>
        public static ServiceIdentifier operator -(ServiceIdentifier a, ServiceIdentifier b)
        {
            var name = $"{a.Name} [{b.Name} - {b.Id}]";
            if (a.Id > b.Id)
            {
                name = $"{b.Name} [{a.Name} - {a.Id}]";
            }

            return new ServiceIdentifier(a.Id - b.Id, name);
        }

        /// <summary>Implements the operator -.</summary>
        /// <param name="a">ServiceIdentifier a.</param>
        /// <param name="b">EventId b.</param>
        /// <returns>The result of the operator.</returns>
        public static EventId operator -(ServiceIdentifier a, EventId b)
        {
            return new EventId(a.Id - b.Id, b.Name);
        }

        /// <summary>Implements the operator -.</summary>
        /// <param name="a">EventId a.</param>
        /// <param name="b">ServiceIdentifier b.</param>
        /// <returns>The result of the operator.</returns>
        public static EventId operator -(EventId a, ServiceIdentifier b)
        {

            return new EventId(a.Id - b.Id, a.Name);
        }
    }
}