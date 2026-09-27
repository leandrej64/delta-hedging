namespace BackTester; 


public class HedgingData{ 
    public double time {get;}
    public double portfolio_value {get;} 
    public double option_price {get;}
    public double transaction_costs {get;} 

    public HedgingData(double p_time, double p_value, double p_option_price, double p_transaction_costs){
        this.time = p_time; 
        this.portfolio_value = p_value;
        this.option_price = p_option_price;
        this.transaction_costs = p_transaction_costs;

    }
}

