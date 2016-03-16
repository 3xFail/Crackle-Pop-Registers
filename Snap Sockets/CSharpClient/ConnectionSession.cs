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
            QueryDB( "GetEmployee_UsernamePassword @0, @1", username, password );

            Authed = Response.Count == 1;
            if( !Authed )
                throw new ArgumentException( "Invalid Username or Password" );
        }

        public void Write( string proc, params object[] args )
        {
            if( !Authed )
                throw new InvalidOperationException( "Un-Authed user cannot perform this action" );

            QueryDB( proc, args );
        }

        private void QueryDB( string proc, params object[] args )
        {
            using( SqlConnection Connection = new SqlConnection( ConnectionString ) )
            {
                Connection.Open();
                using( SqlCommand Command = new SqlCommand( proc, Connection ) )
                {
                    for( int i = 0; i < args.Length; ++i )
                        Command.Parameters.AddNullable( "@" + i.ToString(), args[i] );

                    LoadResponse( Command.ExecuteReader() );
                }
                Connection.Close();
            }
        }

        private void LoadResponse( SqlDataReader reader )
        {
            var doc = new XmlDocument();
            if( reader.Read() )
            {
                try
                {
                    doc.LoadXml( @"<root>" + reader[0] + @"</root>" );
                }
                catch( XmlException ) { } //do nothing it doesn't matter if the XML is invalid. The doc will just be an empty doc which works fine.
            }
            Response = doc.GetElementsByTagName( "row" );
        }
        public XmlNodeList Response { get; private set; }
        private bool Authed = false;
    }
}