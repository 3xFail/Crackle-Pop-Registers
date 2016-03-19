using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Data.SqlClient;
using System.Text;

namespace CSharpClient
{
    public class ConnectionSession
    {
        private readonly string ConnectionString = @"Data Source=aura.students.cset.oit.edu;Initial Catalog=snap;User ID=snap_admin;Password=snap_admin;";
        public ConnectionSession( string username, string password )
        {
#if DEBUG
            File.WriteAllText( "query_output.txt", string.Empty ); //empty the query_output file so we can append only queries from this session
#endif

            QueryDB( "GetPass_Username @0", username );

            Authed = Response.Count == 1;
            if( !Authed )
                throw new ArgumentException( "Invalid Username or Password" );

            if( !PasswordHash.ValidatePassword( password, Response[0].Get( "Password" ) ) )
            {
                Authed = false;
                throw new ArgumentException( "Invalid Username or Password" );
            }

            QueryDB( "GetEmployee_Username @0", username );
        }

        public void Write( string proc, params object[] args )
        {
            if( !Authed )
                throw new InvalidOperationException( "Un-Authed user cannot perform this action" );

            QueryDB( proc, args );
        }

        public void WriteNoResponse( string proc, params object[] args )
        {
            if( !Authed )
                throw new InvalidOperationException( "Un-Authed user cannot perform this action" );

            QueryDBNoResponse( proc, args );
        }

        private void QueryDB( string proc, params object[] args )
        {
            using( SqlConnection Connection = new SqlConnection( ConnectionString ) )
            {
                Connection.Open();
                using( SqlCommand Command = new SqlCommand( proc, Connection ) )
                {
                    for( int i = 0; i < args.Length; ++i )
                        Command.Parameters.AddWithValue( "@" + i, args[i] );

                    LoadResponse( Command.ExecuteXmlReader() );
                }
                Connection.Close();
            }
        }

        private void QueryDBNoResponse( string proc, params object[] args )
        {
            using( SqlConnection Connection = new SqlConnection( ConnectionString ) )
            {
                Connection.Open();
                using( SqlCommand Command = new SqlCommand( proc, Connection ) )
                {
                    for( int i = 0; i < args.Length; ++i )
                        Command.Parameters.AddWithValue( "@" + i, args[i] );

                    Command.ExecuteNonQuery();
                }
            }
        }

        private void LoadResponse( XmlReader reader )
        {
            XmlDocument doc = new XmlDocument();
            XPathNavigator xn = new XPathDocument( reader ).CreateNavigator();

            XmlNode root = doc.CreateElement( "root" );
            root.InnerXml = xn.OuterXml;
            doc.AppendChild( root );

            Response = doc.GetElementsByTagName( "row" );

#if DEBUG
            File.AppendAllText( "query_output.txt", doc.OuterXml + "\r\n" );
#endif
        }

        public XmlNodeList Response { get; private set; }
        private bool Authed = false;
    }
}