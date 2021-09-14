using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lumina.Excel;

namespace LuminaAPI.Data
{
    public class SheetTypeCache
    {
        private readonly List< Type > _typeCache = new();

        public IReadOnlyList< Type > TypeCache => _typeCache;

        public SheetTypeCache( Lumina.GameData gameData )
        {
            // exclude any sheet names where they're in a subfolder
            var sheetNames = gameData.Excel.SheetNames.Where( x => !x.Contains( "/" ) );

            // todo: this is shit & should support multiple assemblies
            var typeAssembly = typeof( Lumina.Excel.GeneratedSheets.Achievement ).Assembly;
            var types = typeAssembly.GetTypes();

            foreach( var name in sheetNames )
            {
                var matchingType = types.FirstOrDefault(
                    x => x.GetCustomAttribute< SheetAttribute >()?.Name == name
                );

                if( matchingType == null )
                {
                    continue;
                }

                _typeCache.Add( matchingType );
            }
        }
    }
}