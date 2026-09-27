using BackTester; 

namespace BackTester; 

public class Tester
{
    public string market_data_file{get;}

    public string option_data_file{get;}

    public double period{get;}

    public double t_max{get;}

    public double transaction_rate{get;}

    public double interest_rate{get;}

    public Tester(string p_market_data_file, string p_option_data_file,double p_t_max,double p_period,double p_transaction_rate,double p_interest_rate)
    {
        this.market_data_file = p_market_data_file;
        this.option_data_file = p_option_data_file;
        this.t_max = p_t_max;
        this.period = p_period;
        this.transaction_rate = p_transaction_rate;
        this.interest_rate = p_interest_rate;

    }

    public List<HedgingData> Run()
    {
        List<HedgingData> list = new List<HedgingData>();
        double transaction_costs = 0;
        PricingResult initial_pricing_result = NativePricer.Price(this.option_data_file,this.market_data_file, 0);
        double[] first_spots = NativePricer.GetSpots(this.option_data_file,this.market_data_file, 0);
        double[] share_init = new double[first_spots.Length];
        Portfolio portfolio = new Portfolio(share_init,initial_pricing_result.Price);
        portfolio.update_portfolio(initial_pricing_result.Delta,first_spots,this.transaction_rate,this.interest_rate,0);

        for (double t = this.period; t <= this.t_max; t += this.period)
        {
            PricingResult result = NativePricer.Price(this.option_data_file,this.market_data_file, t);
            double[] spots = NativePricer.GetSpots(this.option_data_file,this.market_data_file, t);

            transaction_costs += portfolio.update_portfolio(result.Delta, spots,this.transaction_rate,this.interest_rate,this.period);

            HedgingData hedging_data = new HedgingData(t, portfolio.value, result.Price, transaction_costs);
            Console.WriteLine($"time={hedging_data.time} portfolio_value={hedging_data.portfolio_value} option_price={hedging_data.option_price} transaction_costs={hedging_data.transaction_costs}");
            list.Add(hedging_data);
        }

        return list;
    }

}