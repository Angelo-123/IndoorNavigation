/* 
*   Pedometer
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace PedometerU.Platforms {

    using System.Runtime.InteropServices;

    public static class PedometerBridge {
        
        private const string Assembly = "Pedometer";

        public static void Initialize (StepCallback callback) {}
        public static void Release () {}
        public static bool IsSupported () { return false; }
    }
}