using System;
using System.Text;

namespace CSharpClient
{
    public class message
    {
        public static readonly int header_length = 4;
        public static readonly int id_length = 4;
        public static readonly int max_body_length = 200000;
        public static readonly int username_length = 24;

        public message()
        {
            _body_length = 0;
        }

        public message( string buf )
        {
            write( buf );
        }

        public byte[] data()
        {
            return _data;
        }

        public int id_offset()
        {
            return header_length;
        }

        public int body_offset()
        {
            return header_length + id_length;
        }

        public int username_offset()
        {
            return header_length + id_length + _body_length;
        }

        public void body_length( int len )
        {
            _body_length = len > max_body_length ? max_body_length : len;
        }

        public int total_length( bool username_attached )
        {
            if( username_attached )
                return header_length + id_length + _body_length + username_length;
            else
                return header_length + id_length + _body_length;
        }

        public void write( string buf )
        {
            body_length( buf.Length );
            for( int i = 0; i < _body_length && i < buf.Length; ++i )
                _data[body_offset() + i] = Convert.ToByte( ( buf[i] ) );
            encode_header();
        }

        public bool decode_header()
        {
            _body_length = BitConverter.ToInt32( _data, 0 );
            return _body_length >= 0;
        }

        private void encode_header()
        {
            unchecked //IIIIIIIIIII'M A BAAAAD PERSOOOOOONNN
            {
                _data[3] = (byte)( ( _body_length >> 24 ) & 0xFF );
                _data[2] = (byte)( ( _body_length >> 16 ) & 0xFF );
                _data[1] = (byte)( ( _body_length >> 8 ) & 0xFF );
                _data[0] = (byte)( ( _body_length ) & 0xFF );
            }
        }

        public bool decode_id()
        {
            _id = BitConverter.ToInt32( _data, id_offset() );

            return true;
        }

        public void encode_id( int id )
        {
            unchecked //IIIIIIIIIII'M A BAAAAD PERSOOOOOONNN
            {
                _data[3 + id_offset()] = (byte)( ( id >> 24 ) & 0xFF );
                _data[2 + id_offset()] = (byte)( ( id >> 16 ) & 0xFF );
                _data[1 + id_offset()] = (byte)( ( id >> 8 ) & 0xFF );
                _data[0 + id_offset()] = (byte)( ( id ) & 0xFF );
            }
        }

        public bool decode_username()
        {
            byte[] temp = new byte[username_length];
            Array.Copy( _data, username_offset(), temp, 0, username_length );
            _username = Convert.ToString( temp );
            return true;
        }

        public void encode_username( string username )
        {
            _username = username;
            for( int idx = 0; idx < username.Length && idx < username_length; idx++ )
            {
                _data[username_offset() + idx] = Convert.ToByte( username[idx] );
            }
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString( _data, message.header_length + message.id_length, _body_length );
        }

        public byte[] _data { get; private set; } = new byte[header_length + id_length + max_body_length + username_length];
        public int _body_length { get; private set; }
        public int _id { get; private set; }
        string _username;
    }
}
