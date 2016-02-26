using System.Xml;
using System;

namespace CSharpClient
{
    public static class XmlNodeExtension
    {
        public static string Get( this XmlNode node, string field )
        {
            if( node == null )
                throw new NullReferenceException();

            try
            {
                return node.Attributes[field].Value;
            }
            catch( Exception )
            {
                return string.Empty;
            }
        }
    }
}
