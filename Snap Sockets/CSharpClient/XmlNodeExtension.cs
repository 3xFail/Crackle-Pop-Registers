using System.Xml;

namespace CSharpClient
{
    public static class XmlNodeExtension
    {
        public static string Get( this XmlNode node, string field )
        {
            try
            {
                return node.Attributes[field].Value;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
