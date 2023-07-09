/**
* Copyright 2023 d-fens GmbH
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace HelloWorldWithDotNetNanoFramework.MorseCode
{
    public class MorseCodeGenerator
    {
        private const ushort DahDitRatio = 3;
        private const ushort CharacterSpacing = 3;
        private const ushort WordSpacing = 7;

        private const int hashtableEntries = 26 + 10 + 1;
        private readonly Hashtable map;

        private readonly MorseCodeGeneratorConfiguration configuration;

        // Mystery: with this ctor my ESP32-S2 controllers hang after deploy
        // (until re-flashed with NanoFramework)
        //public MorseCodeGenerator(Func<MorseCodeGeneratorConfiguration> cfg) : this(cfg.Invoke()) { }

        public MorseCodeGenerator(MorseCodeGeneratorConfiguration cfg)
        {
            Debug.Assert(null != cfg);

            configuration = cfg;

            map = new Hashtable(hashtableEntries)
            {
                { 'a', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dah } },
                { 'b', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { 'c', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dit } },
                { 'd', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { 'e', new MorseCodeSignal[] { MorseCodeSignal.Dit } },
                { 'f', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dit } },
                { 'g', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dit } },
                { 'h', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { 'i', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { 'j', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dah } },
                { 'k', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah } },
                { 'l', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { 'm', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah } },
                { 'n', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dit } },
                { 'o', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah } },
                { 'p', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dit } },
                { 'q', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dah } },
                { 'r', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dit } },
                { 's', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { 't', new MorseCodeSignal[] { MorseCodeSignal.Dah } },
                { 'u', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dah } },
                { 'v', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dah } },
                { 'w', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dah } },
                { 'x', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dah } },
                { 'y', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dah } },
                { 'z', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },

                { '1', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah } },
                { '2', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah } },
                { '3', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dah, MorseCodeSignal.Dah } },
                { '4', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dah } },
                { '5', new MorseCodeSignal[] { MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { '6', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { '7', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { '8', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dit, MorseCodeSignal.Dit } },
                { '9', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dit } },
                { '0', new MorseCodeSignal[] { MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah, MorseCodeSignal.Dah } },

                // A SPACE is always after a character (we trim whitespace).
                // With this, we already have the character spacing (3) and only need to add 4 additional spaces.
                { ' ', new MorseCodeSignal[] { MorseCodeSignal.Off, MorseCodeSignal.Off, MorseCodeSignal.Off, MorseCodeSignal.Off } },
            };
        }

        public void Generate(string value)
        {
            Debug.Assert(!string.IsNullOrEmpty(value));

            foreach (MorseCodeSignal signal in Serialise(value))
            {
                switch (signal)
                {
                    case MorseCodeSignal.Off:
                        configuration.NoTransmit.Invoke();
                        Thread.Sleep(configuration.UnitLengthMs);
                        break;
                    case MorseCodeSignal.Dit:
                        configuration.Transmit.Invoke();
                        Thread.Sleep(configuration.UnitLengthMs);
                        break;
                    case MorseCodeSignal.Dah:
                        configuration.Transmit.Invoke();
                        Thread.Sleep(DahDitRatio * configuration.UnitLengthMs);
                        break;
                    default:
                        throw new ArgumentException($"Invalid {nameof(MorseCodeSignal)} detected.");
                }
            }
        }

        public Array Serialise(string value)
        {
            const char SPACE = ' ';

            var result = new ArrayList();

            var valueAsTrimmedLowerCase = value.Trim().ToLower();

            for (var c = 0; c < valueAsTrimmedLowerCase.Length; c++)
            {
                var currentCharacter = valueAsTrimmedLowerCase[c];

                // Only process A-Z, 0-9 and SPACE characters.
                if (!map.Contains(currentCharacter)) continue;

                var signalling = map[currentCharacter] as MorseCodeSignal[];

                // Treat SPACE before regular characters to account for trailing character spaces.
                if (SPACE == currentCharacter)
                {
                    foreach (var signal in signalling)
                    {
                        result.Add(signal);
                    }

                    continue;
                }

                // Process regular characters.
                for (var s = 0; s < signalling.Length; s++)
                {
                    var signal = signalling[s];

                    result.Add(signal);

                    if (s + 1 == signalling.Length) continue;

                    // Add intra character spacing.
                    result.Add(MorseCodeSignal.Off);
                }

                // No spacing if already at end of message.
                if (c + 1 == valueAsTrimmedLowerCase.Length) continue;

                // Otherwise add character spacing.
                for (var i = 0; i < CharacterSpacing; i++)
                {
                    result.Add(MorseCodeSignal.Off);
                }
            }

            // Append word spacing at end of message.
            for (var i = 0; i < WordSpacing; i++)
            {
                result.Add(MorseCodeSignal.Off);
            }

            return result.ToArray(typeof(MorseCodeSignal)); ;
        }
    }

}
