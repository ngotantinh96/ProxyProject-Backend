namespace ProxyProject_Backend.Models.ResponseModels
{

    public class MomoType
    {
        public MomoMsg momoMsg { get; set; }
        public long time { get; set; }
        public string user { get; set; }
        public string lang { get; set; }
        public string msgType { get; set; }
        public bool result { get; set; }
        public string appCode { get; set; }
        public string appVer { get; set; }
        public string channel { get; set; }
        public string deviceOS { get; set; }
        public object ip { get; set; }
        public object localAddress { get; set; }
        public string session { get; set; }
        public Extra extra { get; set; }
    }


    public class TranList
    {
        public string user { get; set; }
        public long tranId { get; set; }
        public long clientTime { get; set; }
        public int tranType { get; set; }
        public int io { get; set; }
        public string partnerId { get; set; }
        public string partnerName { get; set; }
        public string comment { get; set; }
        public int amount { get; set; }
        public int status { get; set; }
        public int moneySource { get; set; }
        public string desc { get; set; }
        public string serviceId { get; set; }
        public int receiverType { get; set; }
        public string extra { get; set; }
        public string channel { get; set; }
        public string otpType { get; set; }
        public string ipAddress { get; set; }
        public string _class { get; set; }
    }

    public class MomoMsg
    {
        public long begin { get; set; }
        public long end { get; set; }
        public List<TranList> tranList { get; set; }
        public string _class { get; set; }
    }

    public class Extra
    {
        public string originalClass { get; set; }
        public string originalPhone { get; set; }
        public string checkSum { get; set; }
    }

}
