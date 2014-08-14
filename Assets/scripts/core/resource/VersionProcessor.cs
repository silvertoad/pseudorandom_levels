using System;
using System.Collections.Generic;
using UnityEngine;

namespace core.resources
{
    public class VersionProcessor
    {
        Dictionary<string, ManifestVersionInfo> manifest;
        string staticUrl;

        public void Init (string _source, string _staticUrl)
        {
            manifest = new Dictionary<string, ManifestVersionInfo> ();
            staticUrl = _staticUrl;

            var manifestSource = JSON.Parse (_source);
            foreach (var manifestItem in manifestSource) {
                var manifestInfo = new ManifestVersionInfo (manifestItem.Key, (Dictionary<string, object>)manifestItem.Value);
                manifest.Add (manifestItem.Key, manifestInfo);
            }
        }

        public string Get (string _noVersionUrl)
        {
            var clearUrl = _noVersionUrl.Replace (staticUrl, "");

            if (Has (clearUrl)) {
                return staticUrl + manifest [clearUrl].VersionPath;
            }
            return _noVersionUrl;
        }

        bool Has (string _noVersionUrl)
        {
            return manifest.ContainsKey (_noVersionUrl);
        }
    }

    class ManifestVersionInfo
    {
        public string Path { get; private set; }

        public uint Crc { get; private set; }

        public uint Version { get; private set; }

        public ManifestVersionInfo (string _path, Dictionary<string, object> _versionInfo)
        {
            Path = _path;
            if (!_versionInfo.ContainsKey ("version"))
                throw new VersionProcessorException (string.Format ("Missed required property \"version\" at item: \"{0}\".", _path));
            Version = Convert.ToUInt32 (_versionInfo ["version"]);

            if (_versionInfo.ContainsKey ("crc"))
                Crc = Convert.ToUInt32 (_versionInfo ["crc"]);
        }

        public string VersionPath {
            get { 
                return Path.Insert (Path.LastIndexOf (".", StringComparison.Ordinal), "." + Version);
            }
        }
    }

    public class VersionProcessorException : Exception
    {
        public VersionProcessorException (string _msg) : base (_msg)
        {
        }
    }
}