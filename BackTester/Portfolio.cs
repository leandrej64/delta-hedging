namespace BackTester; 


public class Portfolio
{
    public double value {get; private set;}
    public double[] composition{get; private set;}
    public double cash {get; private set;}

    public Portfolio(double[] initial_composition,double initial_cash)
    {
        this.cash = initial_cash;
        this.composition = initial_composition;
        this.value = 0;

    }

    public double update_portfolio(double[] new_composition, double[] new_spots,double transaction_rate,double interest_rate,double dt)
    {
        cash *= Math.Exp(interest_rate*dt);
        double asset_value = 0;
        double transaction_cost = 0;
        for(int i=0; i<composition.Length; i++)
        {
            double diff = new_composition[i] - this.composition[i];
            cash-= diff*new_spots[i];
            transaction_cost+= Math.Abs(diff)*new_spots[i]*transaction_rate;
            asset_value+= new_composition[i]*new_spots[i];

        }
        
        this.cash = cash;
        this.value = cash + asset_value;
        this.composition  = new_composition;
        return transaction_cost;
    }


}