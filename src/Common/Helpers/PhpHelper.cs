using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BioEngine.Common.Helpers
{
    public static class PhpHelper
    {
        public static object Deserialize(string serializedObject)
        {
            var serializer = new Serializer();
            return serializer.Deserialize(serializedObject);
        }
    }

    /// <summary>
    ///     Serializer Class.
    /// </summary>
    public class Serializer
    {
        private readonly NumberFormatInfo _nfi;

        private int _pos; //for unserialize
        private Dictionary<ArrayList, bool> _seenArrayLists; //for serialize (to infinte prevent loops) lol
        //types:
        // N = null
        // s = string
        // i = int
        // d = double
        // a = array (hashtable)

        private Dictionary<Hashtable, bool> _seenHashtables; //for serialize (to infinte prevent loops)
        //This member tells the serializer wether or not to strip carriage returns from strings when serializing and adding them back in when deserializing

        //http://www.w3.org/TR/REC-xml/#sec-line-ends

        private readonly Encoding _stringEncoding = new UTF8Encoding();

        public Serializer()
        {
            _nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = "",
                NumberDecimalSeparator = "."
            };
        }

        public string Serialize(object obj)
        {
            _seenArrayLists = new Dictionary<ArrayList, bool>();
            _seenHashtables = new Dictionary<Hashtable, bool>();

            return Serialize(obj, new StringBuilder()).ToString();
        } //Serialize(object obj)

        private StringBuilder Serialize(object obj, StringBuilder sb)
        {
            if (obj == null)
                return sb.Append("N;");
            var s = obj as string;
            if (s != null)
            {
                var str = s;
                str = str.Replace("\r\n", "\n"); //replace \r\n with \n
                str = str.Replace("\r", "\n"); //replace \r not followed by \n with a single \n  Should we do this?
                return sb.Append("s:" + _stringEncoding.GetByteCount(str) + ":\"" + str + "\";");
            }
            if (obj is bool)
                return sb.Append("b:" + ((bool) obj ? "1" : "0") + ";");
            if (obj is int)
            {
                var i = (int) obj;
                return sb.Append("i:" + i.ToString(_nfi) + ";");
            }
            if (obj is double)
            {
                var d = (double) obj;

                return sb.Append("d:" + d.ToString(_nfi) + ";");
            }
            var arrList = obj as ArrayList;
            if (arrList != null)
            {
                if (_seenArrayLists.ContainsKey(arrList))
                    return sb.Append("N;"); //cycle detected
                _seenArrayLists.Add(arrList, true);

                var a = arrList;
                sb.Append("a:" + a.Count + ":{");
                for (var i = 0; i < a.Count; i++)
                {
                    Serialize(i, sb);
                    Serialize(a[i], sb);
                }
                sb.Append("}");
                return sb;
            }
            var hashtable = obj as Hashtable;
            if (hashtable == null) return sb;
            {
                if (_seenHashtables.ContainsKey(hashtable))
                    return sb.Append("N;"); //cycle detected
                _seenHashtables.Add(hashtable, true);

                var a = hashtable;
                sb.Append("a:" + a.Count + ":{");
                foreach (DictionaryEntry entry in a)
                {
                    Serialize(entry.Key, sb);
                    Serialize(entry.Value, sb);
                }
                sb.Append("}");
                return sb;
            }
        } //Serialize(object obj)

        public object Deserialize(string str)
        {
            _pos = 0;
            return DoDeserialize(str);
        } //Deserialize(string str)

        private object DoDeserialize(string str)
        {
            if (str == null || str.Length <= _pos)
                return new object();

            int start, end, length;
            string stLen;
            switch (str[_pos])
            {
                case 'N':
                    _pos += 2;
                    return null;
                case 'b':
                    char chBool;
                    chBool = str[_pos + 2];
                    _pos += 4;
                    return chBool == '1';
                case 'i':
                    start = str.IndexOf(":", _pos, StringComparison.Ordinal) + 1;
                    end = str.IndexOf(";", start, StringComparison.Ordinal);
                    var stInt = str.Substring(start, end - start);
                    _pos += 3 + stInt.Length;
                    return int.Parse(stInt, _nfi);
                case 'd':
                    start = str.IndexOf(":", _pos, StringComparison.Ordinal) + 1;
                    end = str.IndexOf(";", start, StringComparison.Ordinal);
                    var stDouble = str.Substring(start, end - start);
                    _pos += 3 + stDouble.Length;
                    return double.Parse(stDouble, _nfi);
                case 's':
                    start = str.IndexOf(":", _pos, StringComparison.Ordinal) + 1;
                    end = str.IndexOf(":", start, StringComparison.Ordinal);
                    stLen = str.Substring(start, end - start);
                    var bytelen = int.Parse(stLen);
                    length = bytelen;
                    //This is the byte length, not the character length - so we migth  
                    //need to shorten it before usage. This also implies bounds checking
                    if (end + 2 + length >= str.Length) length = str.Length - 2 - end;
                    var stRet = str.Substring(end + 2, length);
                    while (_stringEncoding.GetByteCount(stRet) > bytelen)
                    {
                        length--;
                        stRet = str.Substring(end + 2, length);
                    }
                    _pos += 6 + stLen.Length + length;
                    stRet = stRet.Replace("\n", "\r\n");
                    return stRet;
                case 'a':
                    //if keys are ints 0 through N, returns an ArrayList, else returns Hashtable
                    start = str.IndexOf(":", _pos, StringComparison.Ordinal) + 1;
                    end = str.IndexOf(":", start, StringComparison.Ordinal);
                    stLen = str.Substring(start, end - start);
                    length = int.Parse(stLen);
                    var htRet = new Hashtable(length);
                    var alRet = new ArrayList(length);
                    _pos += 4 + stLen.Length; //a:Len:{
                    for (var i = 0; i < length; i++)
                    {
                        //read key
                        var key = DoDeserialize(str);
                        //read value
                        var val = DoDeserialize(str);

                        if (alRet != null)
                            if (key is int && (int) key == alRet.Count)
                                alRet.Add(val);
                            else
                                alRet = null;
                        htRet[key] = val;
                    }
                    _pos++; //skip the }
                    if (_pos < str.Length && str[_pos] == ';')
                        //skipping our old extra array semi-colon bug (er... php's weirdness)
                        _pos++;
                    if (alRet != null)
                        return alRet;
                    return htRet;
                default:
                    return "";
            } //switch
        } //unserialzie(object)	
    } //class Serializer
}