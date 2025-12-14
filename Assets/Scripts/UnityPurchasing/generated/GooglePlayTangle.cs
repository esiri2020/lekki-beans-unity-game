// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("7KhVhQUc86zz5Pp4vJHzUyiAnT5OSAUQeISTxnVYQRv72FuNe+cVTv4xgrInCuEvp5kWrEMS2ec3Skvu6nEogeT1P0r5UKeEP3vxqPMuybQwt+luZkC2SVIjt4VTqQKHRbSaMTv7RtVf38BjPrbOSBKokNDvGpo5itMjkPaMBEzbJX2IHcssjwY7ITn1R8Tn9cjDzO9DjUMyyMTExMDFxsOqncQQh+qGOiwpUT7IykYa5iEks7k0vROX1ka4bOJSrhrewWBEvWA7gxAWpFb1/AsrdOLlr+capIqK10qNtH01NC0W5pTL+uobUrHwK/p/R8TKxfVHxM/HR8TExXE7gH1+yr9depMBewHV4mbZ6/vqHnbdLZyeIZ80mq9loV0ZesfGxMXE");
        private static int[] order = new int[] { 6,13,3,8,8,8,8,8,8,9,10,11,13,13,14 };
        private static int key = 197;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
