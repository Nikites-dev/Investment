using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Investment.ADOApp;


namespace Investment.Models
{
    public class StockExchange
    {

        public static void StockMarketAnalytics(Random random)
        {
            List<StockMarket> stockMarkets = App.Connection.StockMarket.ToList();
            List<Stock> listStocks = App.Connection.Stock.ToList();

            foreach (var stock in listStocks)
            {
                var lastStock = App.Connection.StockMarket.Where(x => x.IdStock == stock.IdStock).ToList().LastOrDefault();

                StockMarket stockMarket = new StockMarket();

                stockMarket.IdStock = stock.IdStock;
                stockMarket.LastTransaction = GeneratePrice((int)lastStock.LastTransaction,  random);

                Stock stockChange = App.Connection.Stock.Where(x => x.IdStock == stock.IdStock).FirstOrDefault();

                stockChange.Price = stockMarket.LastTransaction.Value;


                App.Connection.StockMarket.Add(stockMarket);

                try
                {
                    App.Connection.SaveChanges();
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
              
            }
        }

        public static int GeneratePrice(int stockPrice, Random random)
        {
          //  Random random = new Random();

            int isFall = random.Next(1, 3);   

            if(isFall == 2)
            {
                return PriceUp(stockPrice, random);
            } else
            {
                return PriceDown(stockPrice, random);
            }
        }

        public static int PriceUp(int stockPrice, Random random)
        {

            int xUp = 0;

            if (stockPrice < 0)
            {
                return stockPrice = 1;
            }

            else if (stockPrice < 99)
            {
                xUp = random.Next(0, 2);
                return stockPrice += xUp;   
            } 
            
            else if (stockPrice < 999)
            {
                xUp = random.Next(1, 3);
                return stockPrice += xUp;
            }

            else if (stockPrice < 9999)
            {
                xUp = random.Next(1, 6);
                return stockPrice += xUp;
            }

            else if (stockPrice < 99999)
            {
                xUp = random.Next(10, 100);
                return stockPrice += xUp;
            }

            else if (stockPrice < 999999)
            {
                xUp = random.Next(1000, 10000);
                return stockPrice += xUp;
            }

            else
            {
                return stockPrice;
            }

        }

        public static int PriceDown(int stockPrice, Random random)
        {
            int xDown = 0;

            if (stockPrice < 0)
            {
                return xDown = 1;
            }

            else if (stockPrice < 99)
            {
                xDown = random.Next(0, 2);
                return stockPrice -= xDown;
            }

            else if (stockPrice < 999)
            {
                xDown = random.Next(1, 3);
                return stockPrice -= xDown;
            }

            else if (stockPrice < 9999)
            {
                xDown = random.Next(1, 6);
                return stockPrice -= xDown;
            }

            else if (stockPrice < 99999)
            {
                xDown = random.Next(10, 100);
                return stockPrice -= xDown;
            }

            else if (stockPrice < 999999)
            {
                xDown = random.Next(1000, 10000);
                return stockPrice -= xDown;
            }

            else
            {
                return stockPrice;
            }
        }
    }
}
