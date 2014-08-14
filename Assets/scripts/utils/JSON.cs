using System.Collections.Generic;
using JsonFx.Json;
using JsonFx.Serialization;

public class JSON
{
    static JsonReader reader = new JsonReader ();

    public static TReturn Parse<TReturn> (string _source) where TReturn : class
    {
        return reader.Read (_source) as TReturn;
    }

    public static Dictionary<string, object> Parse (string _source)
    {
        return reader.Read <Dictionary<string, object>> (_source);
    }

    public static string Stringify (object _data)
    {
        var jsonSettings = new DataWriterSettings ();
        jsonSettings.PrettyPrint = true;
        return new JsonWriter (jsonSettings).Write (_data);
    }
}