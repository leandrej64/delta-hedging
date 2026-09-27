using BackTester;
using System.Text.Json;



string market_json = "/home/javelotl/hedging/data/perf.json";
string market_data_file = "/home/javelotl/hedging/data/perf_market.txt";


Tester backTester = new Tester(market_data_file,market_json,1,1.0/365.0,0.001,0.03);
List<HedgingData> list= backTester.Run();

foreach (HedgingData hedging_data in list)
{
    Console.WriteLine($"time={hedging_data.time} portfolio_value={hedging_data.portfolio_value} option_price={hedging_data.option_price} transaction_costs={hedging_data.transaction_costs}");
}

string export_json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
File.WriteAllText("../hedging_data/output.json", export_json);

