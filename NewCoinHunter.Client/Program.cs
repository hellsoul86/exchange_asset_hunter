using NewCoinHunter.Client;
using Newtonsoft.Json;
using Okex.Net;
using Okex.Net.Objects.Core;
using System.Net;

var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "config.json"))!;
var option = new OkexClientOptions();
var handler = new HttpClientHandler { Proxy = new WebProxy(config.Proxy) };
option.HttpClient = new HttpClient(handler);
var okexClient = new OkexClient(option);
okexClient.SetApiCredentials(config.ApiKey,config.ApiSecret,config.PassPhase);
Console.WriteLine($"Trade Asset:{config.Instrument}");
while (true)
{
    try
    {
        var ticker = await okexClient.GetTickerAsync(config.Instrument);
        if (ticker != null)
        {
            Console.WriteLine("Has ticker");
            break;
        }
    }
    catch { }
    Thread.Sleep(100);
}

for (var i = 0; i < config.TradeCount; i++)
{
    var order = await okexClient.PlaceOrderAsync(config.Instrument, Okex.Net.Enums.OkexTradeMode.Cross, Okex.Net.Enums.OkexOrderSide.Buy, Okex.Net.Enums.OkexPositionSide.Net, Okex.Net.Enums.OkexOrderType.MarketOrder, config.TradeAmount);
    var ticker = await okexClient.GetTickerAsync(config.Instrument);
    Thread.Sleep(config.TradeInterval * 1000);
    Console.WriteLine($"Buy round {i + 1}, price:{ticker.Data.LastPrice}.");
}

var instruments = await okexClient.GetInstrumentsAsync( Okex.Net.Enums.OkexInstrumentType.Spot);
var instrument = instruments.Data.First(i => i.Instrument == config.Instrument);
var account = await okexClient.GetAccountBalance_Async(config.Instrument.Split("-")[0]);
var buyTotal = config.TradeAmount * config.TradeCount;
Console.WriteLine();
while (true)
{
    var ticker = await okexClient.GetTickerAsync(config.Instrument);
    var available = account.Data.Details.First().CashBalance;
    var asset = available * ticker.Data.LastPrice;
    var stopPrice = config.StopLossRatio * buyTotal / available!.Value;
    var takeProfitPrice = config.PaybackRatio * buyTotal / available!.Value;
    Console.CursorLeft = 0;
    Console.Write($"[{DateTime.Now}]Price:{ticker.Data.LastPrice}, AssetTotal:{asset}.                  ");
    if (ticker.Data.LastPrice < stopPrice)
    {
        await okexClient.PlaceOrderAsync(config.Instrument, Okex.Net.Enums.OkexTradeMode.Cross, Okex.Net.Enums.OkexOrderSide.Sell, Okex.Net.Enums.OkexPositionSide.Net, Okex.Net.Enums.OkexOrderType.MarketOrder, available!.Value);
        Console.WriteLine();
        Console.WriteLine($"Stop loss Sell:{available.Value},Price:{ticker.Data.LastPrice}");
        break;
    }
    if (ticker.Data.LastPrice > takeProfitPrice)
    {
        var sellAmount = Math.Round(Convert.ToDecimal(buyTotal / ticker.Data.LastPrice), (int)instrument.TickSize);
        await okexClient.PlaceOrderAsync(config.Instrument, Okex.Net.Enums.OkexTradeMode.Cross, Okex.Net.Enums.OkexOrderSide.Sell, Okex.Net.Enums.OkexPositionSide.Net, Okex.Net.Enums.OkexOrderType.MarketOrder, sellAmount);
        Console.WriteLine();
        Console.WriteLine($"Payback Sell:{sellAmount},Price:{ticker.Data.LastPrice}");
        break;
    }
    Thread.Sleep(200);
}

