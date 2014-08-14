using System;
using System.Collections.Generic;
using UnityEngine;

namespace core
{
    public class Config : IConfig
    {
        public AssetPolicies AssetPolicy { get; private set; }

        public string Connection { get; private set; }

        public string CDN { get; set; }

        public Config ()
        {
            var configSource = Resources.Load <TextAsset> ("config").text;
            var source = JSON.Parse (configSource);

            Connection = (string)source ["connection"];
            AssetPolicy = ParseEnum <AssetPolicies> ((string)source ["asset_policy"]); 
        }

        TEnumType ParseEnum<TEnumType> (string _source)
        {
            var enumType = typeof(TEnumType);
            if (!Enum.IsDefined (enumType, _source))
                throw new Exception (string.Format ("Undefined {0}: \"{1}\"", enumType.Name, _source));
            return (TEnumType)Enum.Parse (enumType, _source, true);
        }

        public enum AssetPolicies
        {
            CDN,
            Local
        }
    }
}

public interface IConfig
{
    core.Config.AssetPolicies AssetPolicy { get; }

    string Connection { get; }

    string CDN { get; }
}