

import numpy  
import pandas
import math

BLOCK_SIZE = 1000
MIN_BLOCK_SIZE = 100
MIN_VOLUME = 5*BLOCK_SIZE
MIN_PRICE = 4.0
TREND_BARS_UP = 15
TREND_BARS_DOWN = 30
TREND_BARS_CCI = 20
TREND_BARS = max(TREND_BARS_UP, TREND_BARS_DOWN, TREND_BARS_CCI)


'''
'''

GREEN_RATIO = 0.14

'''
Ratio is calculated as (high-low)/high over period of time TREND_BARS.

Exposure is percentage of cash exposed.

rebalancing_lag (%) is by how much the actual postion can deviate 
from the calculated max exposure in direction of lower exposure.
'''
PARAMETERS_3 = {
    0.48:{"exposure":0.50,"rebalancing_lag":0.02, "regime":(2,"red"), "hedge":0.0,"rebalancing_lag_hedge":0.01},  
    0.25:{"exposure":0.20,"rebalancing_lag":0.02, "regime":(1, "yellow"), "hedge":0.00,"rebalancing_lag_hedge":0.01},
    GREEN_RATIO:{"exposure":0.05,"rebalancing_lag":0.005, "regime":(0, "green"), "hedge":0.05,"rebalancing_lag_hedge":0.05},
}


INPUT_PARAMETERS = PARAMETERS_3


def CCI(df, n):  
    '''
    Commodity Channel Index  
    From https://www.quantopian.com/posts/technical-analysis-indicators-without-talib-code
    See also
    '''
    PP = (df['High'] + df['Low'] + df['Close']) / 3  
    CCI = pandas.Series((PP - pandas.rolling_mean(PP, n)) / pandas.rolling_std(PP, n), name = 'CCI_' + str(n))  
    df = df.join(CCI)  
    return df

def close_all(context, data):  
    for equity in context.portfolio.positions:  
        order_percent(equity, 0)  
        pass

def init_all(context):
    # SID is from https://www.quantopian.com/posts/sid-slash-ticker-set
    sid_uvxy = sid(41969)
    sid_vxx = sid(38054)
    sid_fas = sid(37049)
    sid_spy = sid(8554)
    context.sid_volatility = 0
    context.sid_delta = 1
    context.sids = {context.sid_volatility:sid_vxx, context.sid_delta:sid_spy}
    #log.info("uvxy={0}, fas={1}".format(sid_uvxy, sid_fas))
    
def initialize(context):
    init_all(context)
    context.uptrend = False
    context.downtrend = False
    context.uptrend_days = 0
    context.uptrend_start = 0
    context.downtrend_days = 0
    context.downtrend_start = 0
    context.days = 0
    context.max_exposure_volatility_ratio = 0
    

def get_quote(context, data):
    quotes = {}
    result = True
    for key in context.sids:
        sid = context.sids[key]
        if not data.can_trade(sid):
            return (False, None)
    
        quote = data.history(sid, ["price", "volume", "close"], TREND_BARS, "1m")
        quotes[key] = quote
        # some sanity check of the data
        result = result and (quote["price"].min() > MIN_PRICE)
        result = result and (quote["close"].min() > MIN_PRICE)
        result = result and (quote["volume"].min() > MIN_VOLUME)
        
    return (result, quotes)

def get_position(context, sid):
    amount = 0
    position = context.portfolio.positions[sid]
    amount = position.amount
        
    return amount

def get_positions(context):
    #for pos in context.portfolio.positions:
    #    log.info(pos)
    positions = {}
    in_cash = True
    for key in context.sids:
        sid = context.sids[key]
        position = get_position(context, sid)
        positions[key] = position
        in_cash = in_cash and (position == 0)
    
    return (in_cash, positions)


def uptrend_ratio(historical_data):
    '''
    I declare an uptrend when the volatility remained contained in a narrow channel 
    for TREND_BARS_UP days
    '''
    price_high = price_low = historical_data["close"][0]
    for day in range(TREND_BARS_UP):
        price_1 = historical_data["close"][day]
        price_low = min(price_low, price_1)
        price_high = max(price_high, price_1)
    ratio = (price_high - price_low) / price_high
    return (ratio <= GREEN_RATIO, ratio)

def downtrend_ratio(historical_data):
    '''
    I declare a downtrend when the volatility roughly doubles in TREND_BARS_DOWN days 
    window
    '''
    price_high = price_low = historical_data["close"][0]
    for day in range(TREND_BARS_DOWN):
        price_1 = historical_data["close"][day]
        price_low = min(price_low, price_1)
        price_high = max(price_high, price_1)
    ratio = (price_high - price_low) / price_high
    return (ratio > GREEN_RATIO, ratio)
        
        
def market_conditions(context, data):
    quote_volatility = data.history(context.sids[context.sid_volatility], ["close", "volume"], TREND_BARS+1, "1d")
    uptrend, downtrend = uptrend_ratio(quote_volatility), downtrend_ratio(quote_volatility)
    return (uptrend, downtrend)

def get_max_exposure((delta_uptrend, ratio_uptrend), (delta_downtrend, ratio_downtrend)):
    # I keep some minimal exposure at all times
    exposure = (INPUT_PARAMETERS[GREEN_RATIO])["exposure"]
    rebalancing_lag = (INPUT_PARAMETERS[GREEN_RATIO])["rebalancing_lag"]
    
    exposure_delta = (INPUT_PARAMETERS[GREEN_RATIO])["hedge"]
    rebalancing_lag_delta = (INPUT_PARAMETERS[GREEN_RATIO])["rebalancing_lag"]
    
    # Shall I increase the exposure?
    if delta_uptrend:
        for ratio, values in INPUT_PARAMETERS.items():
            if ratio_uptrend >= ratio:
                exposure = max(exposure, values["exposure"]) 
                rebalancing_lag = max(rebalancing_lag, values["rebalancing_lag"])
                exposure_delta = max(exposure, values["hedge"]) 
                rebalancing_lag_delta = max(rebalancing_lag_delta, values["rebalancing_lag_hedge"])
    elif delta_downtrend:
        for ratio, values in INPUT_PARAMETERS.items():
            if ratio_downtrend >= ratio:
                exposure = max(exposure, values["exposure"]) 
                rebalancing_lag = max(rebalancing_lag, values["rebalancing_lag"])
                exposure_delta = max(exposure, values["hedge"]) 
                rebalancing_lag_delta = max(rebalancing_lag_delta, values["rebalancing_lag_hedge"])
    return (exposure, rebalancing_lag, exposure_delta, rebalancing_lag_delta)
        
def update_trend_days_statistics(context, delta_downtrend, delta_uptrend):
    # TBD consider MIN_BLOCK_SIZE when rebalancing 
    if (delta_downtrend):
        if (not context.downtrend):
            context.downtrend_start = context.days
            #log.info("Start of downtrend expo={0}, maxexpo={1}({2}%), days={3}".format(exposure_volatility, max_exposure_volatility, max_exposure_volatility_ratio, context.downtrend_days))
        context.downtrend_days = context.days - context.downtrend_start
    else:
        if (context.downtrend):
            pass
            #log.info("End of downtrend expo={0}, maxexpo={1}({2}%), days={3}".format(exposure_volatility, max_exposure_volatility, max_exposure_volatility_ratio, context.downtrend_days))
        context.downtrend_days = 0

    if (delta_uptrend):
        if (not context.uptrend):
            context.uptrend_start = context.days
            #log.info("Start of uptrend expo={0}, maxexpo={1}({2}%), days={3}".format(exposure_volatility, max_exposure_volatility, max_exposure_volatility_ratio, context.uptrend_days))
        context.uptrend_days = context.days - context.uptrend_start
    else:
        if (context.uptrend):
            pass
            #log.info("End of uptrend expo={0}, maxexpo={1}({2}%), days={3}".format(exposure_volatility, max_exposure_volatility, max_exposure_volatility_ratio, context.uptrend_days))
        context.uptrend_days = 0
        
    context.uptrend = delta_uptrend
    context.downtrend = delta_downtrend
    
def rebalance_positions(context, data):
    (result, quote) = get_quote(context, data)
    if (not result):
        return;
    
    (in_cash, positions) = get_positions(context)
    exposure_volatility = positions[context.sid_volatility] * quote[context.sid_volatility]["close"][-1]
    exposure_delta = positions[context.sid_delta] * quote[context.sid_delta]["close"][-1]
    portfolio_value = context.portfolio.portfolio_value
    
    
    ((delta_uptrend, ratio_uptrend), (delta_downtrend, ratio_downtrend)) = market_conditions(context, data)
    (max_exposure_volatility_ratio, rebalancing_lag_volatility, max_exposure_delta_ratio, rebalancing_lag_delta) = get_max_exposure((delta_uptrend, ratio_uptrend), (delta_downtrend, ratio_downtrend))
    max_exposure_volatility = -max_exposure_volatility_ratio * portfolio_value
    max_exposure_delta = -max_exposure_delta_ratio * portfolio_value

    update_trend_days_statistics(context, delta_downtrend, delta_uptrend)
        
    if context.max_exposure_volatility_ratio != max_exposure_volatility_ratio:
        log.info("New maxexpo={0:.2f}({1:.2f}), expo={2:.2f}".format(max_exposure_volatility, max_exposure_volatility_ratio, exposure_volatility))
        context.max_exposure_volatility_ratio = max_exposure_volatility_ratio
        
    difference = max_exposure_volatility - exposure_volatility
    # Number of shorted shares can only go up, not stop loss
    if (difference < -rebalancing_lag_volatility*portfolio_value):
        sid_volatility = context.sids[context.sid_volatility]
        order_value(sid_volatility, difference)

    difference = max_exposure_delta - exposure_delta
    if (math.fabs(difference) > rebalancing_lag_delta*portfolio_value):
        sid_delta = context.sids[context.sid_delta]
        order_value(sid_delta, difference)
        
    #record(ratio_downtrend=ratio_downtrend, ratio_uptrend=ratio_uptrend, max_exposure_volatility_ratio=10*max_exposure_volatility_ratio)
    record(exposure_volatility=-exposure_volatility, max_exposure_volatility=-max_exposure_volatility, portfolio_value=portfolio_value, exposure_delta=-exposure_delta)
        
            
def handle_data(context, data):
    '''
    This function is called by the framework every minute
    '''
    rebalance_positions(context, data)

        
def before_trading_start(context, data):
    '''
    Start of the day
    '''
    init_all(context)
    context.days = context.days + 1
