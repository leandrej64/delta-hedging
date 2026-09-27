using System.Reflection;
using System.Runtime.InteropServices;

namespace BackTester;

internal static class NativePricer
{
    private const string LibraryName = "pricer_capi";
    private const string LibraryPath = "/home/javelotl/pricer/build/libpricer_capi.so";

    static NativePricer()
    {
        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), Resolve);
    }

    private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName == LibraryName)
        {
            return NativeLibrary.Load(LibraryPath);
        }
        return IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct PricingResultC
    {
        public double Price;
        public double PriceStdDev;
        public int NbAssets;
        public IntPtr Delta;
        public IntPtr DeltaStdDev;
    }

    [DllImport(LibraryName, CharSet = CharSet.Ansi)]
    private static extern IntPtr price_option(string jsonPath, string marketPath, double t);

    [DllImport(LibraryName)]
    private static extern void free_pricing_result(IntPtr result);

    [StructLayout(LayoutKind.Sequential)]
    private struct SpotsC
    {
        public int NbAssets;
        public IntPtr Spots;
    }

    [DllImport(LibraryName, CharSet = CharSet.Ansi)]
    private static extern IntPtr get_spots(string jsonPath, string marketPath, double t);

    [DllImport(LibraryName)]
    private static extern void free_spots(IntPtr result);

    public static double[] GetSpots(string jsonPath, string marketPath, double t)
    {
        IntPtr ptr = get_spots(jsonPath, marketPath, t);
        try
        {
            SpotsC raw = Marshal.PtrToStructure<SpotsC>(ptr);
            double[] spots = new double[raw.NbAssets];
            Marshal.Copy(raw.Spots, spots, 0, raw.NbAssets);
            return spots;
        }
        finally
        {
            free_spots(ptr);
        }
    }

    public static PricingResult Price(string jsonPath, string marketPath, double t)
    {
        IntPtr ptr = price_option(jsonPath, marketPath, t);
        try
        {
            PricingResultC raw = Marshal.PtrToStructure<PricingResultC>(ptr);
            double[] delta = new double[raw.NbAssets];
            double[] deltaStdDev = new double[raw.NbAssets];
            Marshal.Copy(raw.Delta, delta, 0, raw.NbAssets);
            Marshal.Copy(raw.DeltaStdDev, deltaStdDev, 0, raw.NbAssets);
            return new PricingResult(raw.Price, raw.PriceStdDev, delta, deltaStdDev);
        }
        finally
        {
            free_pricing_result(ptr);
        }
    }
}
