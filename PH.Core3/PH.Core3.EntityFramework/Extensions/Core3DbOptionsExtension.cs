using System;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace PH.Core3.EntityFramework.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Infrastructure.RelationalOptionsExtension" />
    public sealed class Core3DbOptionsExtension : RelationalOptionsExtension
    {
        private long? _serviceProviderHash;
        private string _logFragment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Core3DbOptionsExtension"/> class.
        /// </summary>
        public Core3DbOptionsExtension()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core3DbOptionsExtension"/> class.
        /// </summary>
        /// <param name="bs">The bs.</param>
        public Core3DbOptionsExtension(Core3DbOptionsExtension bs)
        {
            Tenant = bs.Tenant;

        }

        /// <summary>Withes the tenant.</summary>
        /// <param name="tenant">The tenant.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// Value cannot be null or empty. - tenant
        /// or
        /// Value cannot be null or whitespace. - tenant
        /// </exception>
        [NotNull]
        public Core3DbOptionsExtension WithTenant([NotNull] string tenant)
        {
            if (string.IsNullOrEmpty(tenant))
                throw new ArgumentException("Value cannot be null or empty.", nameof(tenant));
            if (string.IsNullOrWhiteSpace(tenant))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tenant));

            var clone = (Core3DbOptionsExtension)Clone();
            clone.Tenant = tenant;

            return clone;
        }
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override long GetServiceProviderHashCode()
        {
            if (_serviceProviderHash == null)
            {
                _serviceProviderHash = (base.GetServiceProviderHashCode() * 397) ^ (Tenant?.GetHashCode() ?? 0L);
                
            }

            return _serviceProviderHash.Value;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override string LogFragment
        {
            get
            {
                if (_logFragment == null)
                {
                    var builder = new StringBuilder();

                    builder.Append(base.LogFragment);

                    if (Tenant != null)
                    {
                        builder.Append("Tenant ")
                               .Append(Tenant)
                               .Append(" ");
                    }

                    _logFragment = builder.ToString();
                }

                return _logFragment;
            }
        }

        /// <summary>Gets the tenant.</summary>
        /// <value>The tenant.</value>
        public string Tenant { get; private set; }

        /// <summary>
        ///     Override this method in a derived class to ensure that any clone created is also of that class.
        /// </summary>
        /// <returns> A clone of this instance, which can be modified before being returned as immutable. </returns>
        protected override RelationalOptionsExtension Clone() => new Core3DbOptionsExtension(this);
        

        /// <summary>
        ///     Adds the services required to make the selected options work. This is used when there
        ///     is no external <see cref="T:System.IServiceProvider" /> and EF is maintaining its own service
        ///     provider internally. This allows database providers (and other extensions) to register their
        ///     required services when EF is creating an service provider.
        /// </summary>
        /// <param name="services"> The collection to add services to. </param>
        /// <returns> True if a database provider and was registered; false otherwise. </returns>
        public override bool ApplyServices(IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }
}