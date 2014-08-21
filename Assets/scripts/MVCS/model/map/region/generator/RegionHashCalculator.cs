using System;
using System.Security.Cryptography;
using System.Text;

namespace mvscs.model
{
    public class RegionHashCalculator
    {
        readonly MD5 Md5 = MD5.Create ();
        int LevelSeed;

        public RegionHashCalculator (int _levelSeed)
        {
            LevelSeed = _levelSeed;
        }

        public int Compute (Point<int> _regionPos)
        {
            var id = _regionPos.ToString () + LevelSeed;
            var stringHash = CalculateMD5Hash (id).Substring (0, 5);
            return Convert.ToInt32 (stringHash, 16);
        }

        string CalculateMD5Hash (string _input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes (_input);
            byte[] hash = Md5.ComputeHash (inputBytes);

            StringBuilder sb = new StringBuilder ();
            for (int i = 0; i < hash.Length; i++)
                sb.Append (hash [i].ToString ("X2"));
            return sb.ToString ();
        }
    }
}

