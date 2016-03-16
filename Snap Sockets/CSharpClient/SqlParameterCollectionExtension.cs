using System;
using System.Data.SqlClient;

namespace CSharpClient
{
    public static class SqlParameterCollectionExtension
    {
        public static SqlParameter AddNullable( this SqlParameterCollection _params, string param_name, object param_val )
        {
            if( param_val.ToString() != string.Empty )
                return _params.AddWithValue( param_name, param_val );
            else return _params.AddWithValue( param_name, DBNull.Value );
        }
    }
}
