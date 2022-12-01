using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investment.ADOApp;


namespace Investment.Models
{
    public class StockExchange
    {

        public static void StockMarketAnalytics()
        {
            List<StockMarket> stockMarkets = App.Connection.StockMarket.ToList();

            foreach(var stock in stockMarkets)
            {
                var lastStock = App.Connection.StockMarket.Where(x => x.IdStock == stock.IdStock).LastOrDefault();

                StockMarket stockMarket = new StockMarket();

                stockMarket.IdStock = stock.IdStock;
                stockMarket.LastTransaction = GeneratePrice((int)lastStock.LastTransaction);

                App.Connection.StockMarket.Add(stockMarket);
                App.Connection.SaveChanges();
            }
        }

        public static int GeneratePrice(int stockPrice)
        {
            Random random = new Random();

            int isFall = random.Next(1, 2);   

            if(isFall == 1)
            {
                return PriceUp();
            } else
            {
                return PriceDown();
            }

            
        }

        public static int PriceUp()
        {
            return 0;
        }

        public static int PriceDown()
        {
            return 0;
        }
    }
}
