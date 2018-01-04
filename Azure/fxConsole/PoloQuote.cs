using System;

namespace fxConsole
{
    public class PoloQuote
    {
        public int id { get; set; }
        public decimal last { get; set; }
        public decimal lowestAsk { get; set; }
        public decimal highestBid { get; set; }
        public decimal percentChange { get; set; }
        public decimal baseVolume { get; set; }
        public decimal quoteVolume { get; set; }
        public decimal isFrozen { get; set; }
        public decimal high24hr { get; set; }
        public decimal low24hr { get; set; }

        public override string ToString()
        {
            return string.Format("last:{0} ask:{1} bid:{2} chg:{3} high:{4} low:{5}", last, lowestAsk, highestBid, percentChange, high24hr, low24hr);
        }
    }
}
