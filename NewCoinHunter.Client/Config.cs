using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCoinHunter.Client
{
    public class Config
    {
        public string ApiKey { get; set; } //换成自己的
        public string ApiSecret { get; set; } //换成自己的
        public string PassPhase { get; set; } //换成自己的
        public string Proxy { get; set; } //换成自己的，支持http,https,socks5
        public string Instrument { get; set; }//交易对，自己改
        public int TradeCount { get; set; } //定投次数，自己改
        public int TradeInterval { get; set; } //定投间隔，秒为单位，自己改
        public decimal TradeAmount { get; set; } //每次用多少USDT买，自己改
        public decimal PaybackRatio { get; set; } //回本线，2=翻倍回本，利润自己玩，自己改
        public decimal StopLossRatio { get; set; }//止损线，腰斩了止损跑路，自己改，0=不止损
    }
}
