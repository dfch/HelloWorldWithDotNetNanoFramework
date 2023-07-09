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

using HelloWorldWithDotNetNanoFramework.MorseCode;
using nanoFramework.TestFramework;

namespace HelloWorldWithDotNetNanoFramework.Tests.MorseCode;

[TestClass]
public class MorseCodeGeneratorTests
{
    [TestMethod]
    public void SerialiseUpperCaseAndLowerCaseAreEqual()
    {
        var upperCase = "ARBITRARY STRING";
        var lowerCase = "arbitrary string";

        var cfg = new MorseCodeGeneratorConfiguration();
        var sut = new MorseCodeGenerator(cfg);

        var result1 = sut.Serialise(upperCase);
        var result2 = sut.Serialise(lowerCase);

        Assert.AreEqual(result1, result2);
    }
}
