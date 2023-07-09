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

using System.Device.Gpio;
using System.Diagnostics;
using HelloWorldWithDotNetNanoFramework.MorseCode;

namespace HelloWorldWithDotNetNanoFramework;

public class Program
{
    // Depending on the board/controller this can also be 13 or 4.
    // See NanoFramework blinky app for more details.
    // e.g.
    // SparkThing Plus ESP32-WROOM-32E: 13
    // ESP32-WROOM-32: 2
    private const int GPIO_PIN_LED = 13;

    private static GpioController s_GpioController;

    public static void Main()
    {
        s_GpioController = new GpioController();

        var led = s_GpioController.OpenPin(GPIO_PIN_LED, PinMode.Output);
        led.Write(PinValue.Low);

        var morseCode = new MorseCodeGenerator(new MorseCodeGeneratorConfiguration
        {
            Transmit = () => led.Write(PinValue.High),
            NoTransmit = () => led.Write(PinValue.Low),
        });

        while (true)
        {
            var message = "Hello, world!";

            Debug.WriteLine(message);

            morseCode.Generate(message);
        }
    }
}