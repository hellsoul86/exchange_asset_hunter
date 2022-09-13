临时搞的小工具，暂时只支持okx的\
配置：\
{\
  "ApiKey": "123456", //换成自己的\
  "ApiSecret": "123456", //换成自己的\
  "PassPhase": "123456", //换成自己的\
  "Proxy": "socks5://127.0.0.1:8888", //换成自己的，支持http,https,socks5\
  "Instrument": "LING-USDT",//换成想要的打新\
  "TradeCount": "3", //定投次数，自己改\
  "TradeInterval": "1", //定投间隔，秒为单位，自己改\
  "TradeAmount": "10", //每次用多少USDT买，自己改\
  "PaybackRatio": "2", //回本线，2=翻倍回本，利润自己玩，自己改\
  "StopLossRatio": "0.5"//止损线，腰斩了止损跑路，自己改，0=不止损\
}\
