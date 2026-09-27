namespace BackTester;

public sealed class PricingResult
{
    public double Price { get; }
    public double PriceStdDev { get; }
    public double[] Delta { get; }
    public double[] DeltaStdDev { get; }

    public PricingResult(double price, double priceStdDev, double[] delta, double[] deltaStdDev)
    {
        Price = price;
        PriceStdDev = priceStdDev;
        Delta = delta;
        DeltaStdDev = deltaStdDev;
    }
}
