using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Lumina.Data;
using Lumina.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace LuminaAPI.Controllers
{
    [ApiController]
    [Route( "[controller]/{language}" )]
    public class SheetsController : ControllerBase
    {
        private readonly Lumina.Lumina _lumina;
        
        // todo: this is so shit
        private static Dictionary< string, Type > _sheetNameToTypes = null!;
        private static MethodInfo _getSheetT = null!;

        public SheetsController( Lumina.Lumina lumina )
        {
            _lumina = lumina;

            if( _sheetNameToTypes != null )
            {
                return;
            }
            
            _sheetNameToTypes = new();

            var assembly = typeof( Action ).Assembly;
            foreach( var type in assembly.GetTypes().Where( x => x.Namespace == typeof( Action ).Namespace ) )
            {
                _sheetNameToTypes[ type.Name.ToLowerInvariant() ] = type;
            }

            _getSheetT = typeof( Lumina.Lumina )
                .GetMethods( BindingFlags.Instance | BindingFlags.Public )
                .FirstOrDefault( x => x.Name == "GetExcelSheet" && x.GetParameters().Any() );
        }
        
        [HttpGet("{sheetName}/{rowId}")]
        [HttpGet("{sheetName}/{rowId}/{subRowId}")]
        public ActionResult GetSheetRow( Language language, string sheetName, uint rowId, uint subRowId = UInt32.MaxValue )
        {
            sheetName = sheetName.ToLowerInvariant();
            if( !_sheetNameToTypes.TryGetValue( sheetName, out var sheetType ) )
            {
                return NotFound( "no typed sheet found with that name!" );
            }

            // todo: T R U L Y C U R S E D
            var getSheetTyped = _getSheetT.MakeGenericMethod( sheetType );
            var sheet = getSheetTyped.Invoke( _lumina, new object[] { language } );

            var fn = sheet
                .GetType()
                .GetMethods( BindingFlags.Instance | BindingFlags.Public )
                .FirstOrDefault( x => x.Name == "GetRow" && x.GetParameters().Length == 2 );

            var data = fn.Invoke( sheet, new object[] { rowId, subRowId } );
            if( data != null )
            {
                return Ok( data );
            }

            return NotFound();
        }
    }
}