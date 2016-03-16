using System.Data.SqlClient;
using System.Xml;
using System;

namespace CSharpClient
{
    public class ConnectionSession
    {
        private readonly string ConnectionString = @"Data Source=aura.students.cset.oit.edu;Initial Catalog=snap;User ID=snap_admin;Password=snap_admin;";
        public ConnectionSession( string username, string password ) //Constructor needs similar code to 'Write' but not the same because of authed check and whatnot.
        {
            using( SqlConnection Connection = new SqlConnection( ConnectionString ) )
            {
                SqlCommand Command = new SqlCommand( "GetEmployee_UsernamePassword @0, @1", Connection );
                Command.Parameters.AddWithValue( "@0", username );
                Command.Parameters.AddWithValue( "@1", password );

                var reader = Command.ExecuteReader();
                var doc = new XmlDocument();

                if( reader.Read() )
                {
                    try
                    {
                        doc.LoadXml( @"<root>" + reader[0] + @"</root>" );
                    }
                    catch( XmlException ) { }
                }
                Response = doc.GetElementsByTagName( "row" );
                Connection.Close();
            }

            Authed = Response.Count == 1;
            if( !Authed )
                throw new ArgumentException( "Invalid Username or Password" );
        }

        public void Write( string proc, params object[] args )
        {
            if( !Authed ) throw new InvalidOperationException( "Un-Authed user cannot perform this action" );

            using( SqlConnection Connection = new SqlConnection( ConnectionString ) )
            {
                Connection.Open();

                SqlCommand Command = new SqlCommand( proc, Connection );
                for( int i = 0; i < args.Length; ++i )
                    Command.Parameters.AddNullable( "@" + i.ToString(), args[i] );

                var reader = Command.ExecuteReader();
                var doc = new XmlDocument();

                if( reader.Read() )
                {
                    try
                    {
                        doc.LoadXml( @"<root>" + reader[0] + @"</root>" );
                    }
                    catch( XmlException ) { }
                }
                Response = doc.GetElementsByTagName( "row" );
                Connection.Close();
            }
        }
        public XmlNodeList Response { get; set; }
        private bool Authed = false;
    }

    public static class ExtensionMethods
    {
        public static SqlParameter AddNullable( this SqlParameterCollection _params, string param_name, object param_val )
        {
            if( param_val.ToString() != string.Empty )
                return _params.AddWithValue( param_name, param_val );
            else return _params.AddWithValue( param_name, DBNull.Value );
        }
    }
}
