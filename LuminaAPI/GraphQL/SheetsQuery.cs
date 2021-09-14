using System.Linq;
using System.Reflection;
using HotChocolate.Types;
using Humanizer;
using Lumina.Data;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace LuminaAPI.GraphQL
{
    public class SheetsQuery
    {
        // [UsePaging]
        // public IEnumerable< AchievementCategory > GetAchievementCategories( [Service] Lumina.GameData gameData ) =>
        //     gameData.GetExcelSheet< AchievementCategory >()!;
        //
        // public AchievementCategory? GetAchievementCategory( [Service] Lumina.GameData gameData, uint rowId ) =>
        //     gameData.GetExcelSheet< AchievementCategory >()?.GetRow( rowId );
    }

    public class SheetsQueryType : ObjectType< SheetsQuery >
    {
        private static MethodInfo? AddSheetTypeFn = null;
        
        protected override void Configure( IObjectTypeDescriptor< SheetsQuery > descriptor )
        {
            if( AddSheetTypeFn == null )
            {
                AddSheetTypeFn = GetType().GetMethod( nameof( AddSheetType ), BindingFlags.Instance | BindingFlags.NonPublic );
            }
            
            AddQuestTextAccessor( descriptor );

            var sheetTypes = Service< Data.SheetTypeCache >.Get();
            foreach( var type in sheetTypes.TypeCache )
            {
                AddSheetTypeFn!.MakeGenericMethod( type ).Invoke( this, new object?[] { descriptor } );
            }
            
            // AddSheetType< Achievement >( descriptor );
            // AddSheetType< Item >( descriptor );
        }

        private void AddQuestTextAccessor( IObjectTypeDescriptor< SheetsQuery > descriptor )
        {
            
        }

        private void AddSheetType< T >( IObjectTypeDescriptor< SheetsQuery > descriptor ) where T : ExcelRow
        {
            // this is shit but it 'works'
            // in the case that the 2nd character is uppercase it's probably an acronym and we shouldn't fuck with the casing
            string sheetName = typeof( T ).Name;
            if( !char.IsUpper( sheetName[ 1 ] ) )
            {
                sheetName = char.ToLowerInvariant( typeof( T ).Name[ 0 ] ) + typeof( T ).Name[ 1.. ];
            }
            
            descriptor
                .Field( sheetName )
                .Argument( "rowId", argumentDescriptor => argumentDescriptor.Type< UnsignedIntType >() )
                // .Type< ObjectType< T > >()
                .Resolve( resolver =>
                {
                    var gameData = Service< Lumina.GameData >.Get();
                    Language language = Language.English;
                    // if( resolver.ContextData.TryGetValue( "language", out var rawLanguage ) )
                    // {
                    // language = (Language)rawLanguage!;
                    // }

                    return gameData.GetExcelSheet< T >( language )!.GetRow( resolver.ArgumentValue< uint >( "rowId" ) );
                } );

            descriptor
                .Field( sheetName.Pluralize() )
                .UsePaging< ObjectType< T > >()
                .Type< ListType< ObjectType< T > > >()
                .Resolve( resolver =>
                {
                    var gameData = Service< Lumina.GameData >.Get();
                    Language language = Language.English;
                    // if( resolver.ContextData.TryGetValue( "language", out var rawLanguage ) )
                    // {
                    // language = (Language)rawLanguage!;
                    // }

                    return gameData.GetExcelSheet< T >( language )!.AsEnumerable();
                } );
        }
    }
}