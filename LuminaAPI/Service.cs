using System;

namespace LuminaAPI
{
    /// <summary>
    /// Basic service locator because singletons are shit
    /// </summary>
    /// <typeparam name="T">The class you want to store in the service locator</typeparam>
    public static class Service< T > where T : class
    {
        private static T? _object;

        static Service()
        {
        }

        public static void Set( T obj )
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if( obj == null )
            {
                throw new ArgumentNullException( $"{nameof( obj )} is null!" );
            }

            _object = obj;
        }

        /// <summary>
        /// Create a new instance of <see cref="T"/> if it is trivially constructable.
        /// </summary>
        /// <returns>The newly created instance of <see cref="T"/></returns>
        public static T Set()
        {
            _object = Activator.CreateInstance< T >();

            return _object;
        }

        public static T Set( params object[] args )
        {
            var obj = ( T? )Activator.CreateInstance( typeof( T ), args );
            
            // ReSharper disable once JoinNullCheckWithUsage
            if( obj == null )
            {
                throw new Exception( "what he fuc" );
            }
            
            _object = obj;

            return obj;
        }

        /// <summary>
        /// Gets the instance of <see cref="T"/> stored in the service locator
        /// </summary>
        /// <remarks>
        /// Note that it is undefined behaviour to pull a service that isn't registered and you will crash if you don't check for null.
        /// </remarks>
        /// <returns>The stored instance of <see cref="T"/></returns>
        public static T Get()
        {
            return _object!;
        }
    }
}