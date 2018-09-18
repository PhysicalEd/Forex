//+------------------------------------------------------------------+
//|                                           LondonOpenBreakout.mq4 |
//|                        Copyright 2017, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2017, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict


// ----- Trading times -----
enum TradingTimes{
   USER_DEFINED = 1,
   LONDON_OPEN = 2,
   LONDON_CLOSE = 3
};

// ----- Parameters -----
input TradingTimes tradeOpenTime = LONDON_CLOSE; // Trade time
input ENUM_TIMEFRAMES timeFrame = PERIOD_H1; // Time period per bar
input int userDefinedHourGMT = 21; // User defined hour (Only for user defined trade time)
input int userDefinedMinuteGMT = 0; // User defined minute (Only for user defined trade time)
input bool isBST = true; // British summer time - Set to true if London is in Daylight Savings (London trading time only)
input int tradeOpenBarOffset = 0; // The number of bars to start trading before or after trade open time
input int tradeCloseBarOffset = 6; // The number of bars after trade open time to stop attempting orders
// EO TODO we shouldn't need this...
input bool isTestMode = false; // Test mode // EO

// ----- Constants -----
int oneHour = 3600;
int timeFrameMinuteConverter = 60;
int currentTimeAndGMTTimeOffset;

// ----- London Open specific constants (Times local to London)-----
int londonOpenHour = 8;
int londonOpenMinute = 00;
int londonCloseHour = 17;
int londonCloseMinute = 0;


// ----- Global variables -----
string tradeDescriptionToken = "";
string tradeTimeToken = "";

datetime tradeTimeServer;
datetime tradeTimeGMT;
datetime nextTradeTimeServer;
datetime nextTradeTimeGMT;
int tradeTimeOffset;

bool isWaitingToStartOrder = false;
bool tradeIsInSession = false;

bool resetTradeTime = false;

// ----- Reference bar related variables -----
double referenceHigh;
double referenceLow;
double referenceOpen;
double referenceClose;
bool isReferenceSet = false;
double TPShort;
double TPLong;
double shortStopLoss;
double takeProfitSpread;
double stopLoss;

bool hasTraded = false;

double marketStopLevelInPips;


void SetTradingTime() {
   
   int tradeHourGMT;
   int tradeMinuteGMT;
   
   // Iterates through each case depending on user's option
   switch(tradeOpenTime){
      case USER_DEFINED:
         tradeDescriptionToken = "a user defined value";         
         tradeHourGMT = userDefinedHourGMT;
         tradeMinuteGMT = userDefinedMinuteGMT;
         break;
      case LONDON_OPEN:
         tradeDescriptionToken = "'London Open'";
         tradeHourGMT = londonOpenHour;
         tradeMinuteGMT = londonOpenMinute;
         
         // If London is on daylight savings         
         if (isBST) tradeHourGMT--;
         break;
      case LONDON_CLOSE:
         tradeDescriptionToken = "'London Close'";         
         tradeHourGMT = londonCloseHour;
         tradeMinuteGMT = londonCloseMinute;
         if (isBST) tradeHourGMT--;
         break;
      
   }
   
   // Get current GMT time to get a time reference
   datetime GMTTime = TimeGMT();
   string tradeTimeStringGMT = TimeYear(GMTTime) + "/" + TimeMonth(GMTTime) + "/" + TimeDay(GMTTime) + " " + tradeHourGMT + ":" + tradeMinuteGMT + ":00";
   tradeTimeGMT = StrToTime(tradeTimeStringGMT);
   tradeTimeServer = tradeTimeGMT - currentTimeAndGMTTimeOffset;

   // If the trade time has gone past already, set the next trade time for the next day
   if (resetTradeTime){      
      nextTradeTimeGMT = tradeTimeGMT + (oneHour * 24);      
      nextTradeTimeServer = nextTradeTimeGMT - currentTimeAndGMTTimeOffset;
      tradeTimeGMT = nextTradeTimeGMT;
      tradeTimeServer = nextTradeTimeServer;
      resetTradeTime = false;
   }
}


void SetCurrentTimeAndGMTTimeOffset(){
   datetime currentTime = TimeCurrent();
   datetime GMTTime = TimeGMT();
   
  
   // Get formatted GMT time
   string currentGMTTimeString = TimeYear(GMTTime) + "/" + TimeMonth(GMTTime) + "/" + TimeDay(GMTTime) + " " + TimeHour(GMTTime) + ":" + TimeMinute(GMTTime) + ":00";
   datetime currentGMTTimeFormatted = StrToTime(currentGMTTimeString);
      
   // Get formatted curent time
   string currentTimeString = TimeYear(currentTime) + "/" + TimeMonth(currentTime) + "/" + TimeDay(currentTime) + " " + TimeHour(currentTime) + ":" + TimeMinute(currentTime) + ":00";
   datetime currentTimeFormatted = StrToTime(currentTimeString);
      
   currentTimeAndGMTTimeOffset = currentGMTTimeFormatted - currentTimeFormatted;
}


//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
int OnInit(){
   SetCurrentTimeAndGMTTimeOffset();   

   // Set trading time
   SetTradingTime(); 
     
   marketStopLevelInPips = MarketInfo(NULL, MODE_STOPLEVEL) * Point;
     
   MessageBox("Trading time is set to " + tradeDescriptionToken + " which is starts from " + tradeTimeGMT + " in GMT timezone");
     
   return(INIT_SUCCEEDED);
}

//+------------------------------------------------------------------+
//| Helper functions                                                 |
//+------------------------------------------------------------------+
double NormalizeTakeProfit(double takeProfitValue, int orderType) {
   double ask = Ask;
   double bid = Bid;     
   double takeProfitValueAdjusted = takeProfitValue;
      
   // If it is long order
      
    if (orderType == OP_BUY){    
       // Put in place to make sure that the calling function is correct ie. the ask is greater than the stoploss.
       if (ask >= takeProfitValueAdjusted){
          return -1;
       }      
   
       double askAndTPDifference = NormalizeDouble(MathAbs(ask - takeProfitValueAdjusted), 5);      
   
       while(askAndTPDifference < marketStopLevelInPips){
          takeProfitValueAdjusted += marketStopLevelInPips;
          askAndTPDifference = NormalizeDouble(MathAbs(ask - takeProfitValueAdjusted), 5);
       }
   
       return takeProfitValueAdjusted;
    } 
   if (orderType == OP_SELL){    
            
      // Put in place to make sure that the calling function is correct ie. the bid is lesser than the stoploss.
      if (bid <= takeProfitValueAdjusted){
         return -1;
      }      
      double bidAndTPDifference = NormalizeDouble(MathAbs(bid - takeProfitValueAdjusted), 5);      
      while(bidAndTPDifference < marketStopLevelInPips){
         takeProfitValueAdjusted -= marketStopLevelInPips;
         bidAndTPDifference = NormalizeDouble(MathAbs(bid - takeProfitValueAdjusted), 5);
      }
      
      return takeProfitValueAdjusted;
   } 
   
   // Return invalid value by default
   return -1;
}

double NormalizeStopLoss(double stopLossValue, int orderType){
      double ask = Ask;
      double bid = Bid;     
      
      double stopLossValueAdjusted = stopLossValue;
      // If it is long order
      if (orderType == OP_BUY){      
         // Put in place to make sure that the calling function is correct ie. the ask is greater than the stoploss.
         if (ask <= stopLossValueAdjusted){
            return -1;
         } 
         double askAndSLDifference = NormalizeDouble(MathAbs(ask - stopLossValueAdjusted), 5);      
         while(askAndSLDifference < marketStopLevelInPips){
            stopLossValueAdjusted -= marketStopLevelInPips;
            askAndSLDifference = NormalizeDouble(MathAbs(ask - stopLossValueAdjusted), 5);
         }
        
         return stopLossValueAdjusted;
      } 
   
      // If it is short order   
      if(orderType == OP_SELL){
         // Put in place to make sure that the calling function is correct ie. the bid is lesser than the stoploss.
         if (bid >= stopLossValueAdjusted){
            return -1;
         } 
         double bidAndSLDifference = NormalizeDouble(MathAbs(bid - stopLossValueAdjusted), 5);
         
         while(bidAndSLDifference < marketStopLevelInPips){               
            stopLossValueAdjusted += marketStopLevelInPips;
            bidAndSLDifference = NormalizeDouble(MathAbs(bid - stopLossValueAdjusted), 5);
         }
      
         return stopLossValueAdjusted;
   }
   // Return invalid value by default
   return -1;
}

void ResetTradeTime(){
      Alert("Trade time reset");
     // Reset global variables
     isReferenceSet = false;
     hasTraded = false;

     SetCurrentTimeAndGMTTimeOffset();   

     // Set trading time
     SetTradingTime(); 
     resetTradeTime = true;
}

//+------------------------------------------------------------------+
//| Expert tick function                                             |
//+------------------------------------------------------------------+
void OnTick(){
   
   // Get the local time of the current bar
   datetime currentBarTime = iTime(NULL, timeFrame, 0);
   

   // Index of the trade open time. It will be zero if it is the current bar or if the bar is in the future   
   int tradeOpenTimeIndex = iBarShift(NULL, timeFrame, tradeTimeServer);
//Alert("tradeOpenTimeIndex " , tradeOpenTimeIndex);
//
//   Alert("tradeTimeServer " , tradeTimeServer);
   
   int referenceBarIndex = tradeOpenTimeIndex - tradeOpenBarOffset;
   //Alert("referenceBarIndex ", referenceBarIndex);
   
   int closeReferenceBarIndex = tradeOpenTimeIndex - tradeCloseBarOffset;
   
   
   // If there are no open orders and it has gone past the closing bar, Reset routine
   // EO TODO TO BE TESTED
   if ((OrdersTotal() == 0 || hasTraded) && closeReferenceBarIndex >= 0){
      // Reset tradeOpenTime...
         Alert("tradeOpenTimeIndex ", tradeOpenTimeIndex);
   Alert("closeReferenceBarIndex ", closeReferenceBarIndex);
      ResetTradeTime();
   }
   
   
   
   double ask = Ask;
   double bid = Bid;
   
   // If the reference bar index is greater than or equal to 1, that means that the reference bar has finished. Get the reference values   
   if (referenceBarIndex >= 1 && !isReferenceSet){   
      ("Hit");
      referenceHigh = iHigh(NULL, timeFrame, referenceBarIndex);
      referenceLow = iLow(NULL, timeFrame, referenceBarIndex);
      
      referenceOpen = iOpen(NULL, timeFrame, referenceBarIndex);
      referenceClose = iClose(NULL, timeFrame, referenceBarIndex);
      
      takeProfitSpread = NormalizeDouble(MathAbs(referenceHigh - referenceLow), 5); 
      
      TPLong = ask + takeProfitSpread;
      TPShort = bid - takeProfitSpread;      
      
      isReferenceSet = true;
      
   } else {     
      // Don't bother checking for the rest... 
      return;
   }   
   
   // Start making orders
   
   // Long
   if(ask >= referenceHigh && !hasTraded) {
      
      // Normalize SL and TP
      double normalizedSL = NormalizeStopLoss(referenceLow, OP_BUY);
      double normalizedTP = NormalizeTakeProfit(TPLong, OP_BUY);
         
      if (normalizedSL != -1 && normalizedTP != -1){              
         OrderSend(NULL, OP_BUY, 0.01, ask, 3, normalizedSL, normalizedTP, NULL, 0, 0, Green);               
         
         hasTraded = true;   
      }
   }
   
   // Short
      
   if(bid <= referenceLow && !hasTraded) {
      // Normalize SL and TP
      double normalizedSL = NormalizeStopLoss(referenceHigh, OP_SELL);
      double normalizedTP = NormalizeTakeProfit(TPShort, OP_SELL);                     
 
      if (normalizedSL != -1 && normalizedTP != -1){
         OrderSend(NULL, OP_SELL, 0.01, bid, 3, normalizedSL, normalizedTP, NULL, 0, 0, Green);               
         hasTraded = true;
      }
   }       

   
   // The index of the bar based on the closeTradeBarOffset user parameter
   int closeReferenceTimeBarIndex; 
   
   //closeReferenceTimeBarIndex = tradeOpenTimeBarID - closeTradeBarOffset;
   
   // Defines whether trading is in session or not   
   bool tradingIsInSession = (tradeTimeGMT > currentBarTime); // && (TimeGMT() < ; //&& (TimeGMT < 
   
   
   // If not, break routine and reset trade time
   //if (!tradingIsInSession) SetTradingTime();
   //return;
   
   //Alert("Hit??");
}
//+------------------------------------------------------------------+
