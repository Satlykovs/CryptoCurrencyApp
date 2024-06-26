namespace App2;

[System.Serializable]
public class CoinLoreResponse
{
    public List<CoinInfo> data { get; set; }
    public Info info { get; set; }
}

public class CoinInfo
{
    public string id { get; set; }
    public string symbol { get; set; }
    public string name { get; set; }
    public string nameid { get; set; }
    public int rank { get; set; }
    public string price_usd { get; set; }
    public string percent_change_24h { get; set; }
    public string percent_change_1h { get; set; }
    public string percent_change_7d { get; set; }
    public string price_btc { get; set; }
    public string market_cap_usd { get; set; }
    public double volume24 { get; set; }
    public double volume24a { get; set; }
    public string csupply { get; set; }
    public string tsupply { get; set; }
    public string? msupply { get; set; }

}

public class Info{
    public int coins_num { get; set; }
    public int tine { get; set; }
}