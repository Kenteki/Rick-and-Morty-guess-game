using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rick_Morty_console_game.core
{
    public class KeyManager
    {
        private readonly List<byte[]> _keys = new();
        private readonly List<int> _mortyValues = new();
        private readonly List<string> _hmacs = new();
        private int _currentRound = 0;

        public void GenerateKey(int maxBoxes)
        {
            byte[] key = RandomNumberGenerator.GetBytes(32);
            int mortyValue = RandomNumberGenerator.GetInt32(maxBoxes);
            string hmac = CalculateHMAC(mortyValue, key);

            _keys.Add(key);
            _mortyValues.Add(mortyValue);
            _hmacs.Add(hmac);
            _currentRound++;
        }

        private string CalculateHMAC(int value, byte[] key)
        {
            byte[] valueInBytes = Encoding.UTF8.GetBytes(value.ToString());
            HMac hmac = new HMac(new Sha3Digest(256));
            hmac.Init(new KeyParameter(key));
            hmac.BlockUpdate(valueInBytes, 0, valueInBytes.Length);

            byte[] result = new byte[hmac.GetMacSize()];
            hmac.DoFinal(result, 0);

            return BitConverter.ToString(result).Replace("-", "").ToUpper();
        }

        public int CalculateFairValue(int userChoice, int maxBoxes, int round)
        {
            return (_mortyValues[round] + userChoice) % maxBoxes;
        }

        public string GetHMAC(int round) => _hmacs[round];
        public byte[] GetKey(int round) => _keys[round];
        public int GetMortyValue(int round) => _mortyValues[round];
        public int GetCurrentRound() => _currentRound;
        public int GetRoundCount() => _keys.Count;
    }
}