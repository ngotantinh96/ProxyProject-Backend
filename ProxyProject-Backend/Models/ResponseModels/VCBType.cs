namespace ProxyProject_Backend.Models.ResponseModels
{
    public class ChiTietGiaoDich
    {
        public string SoThamChieu { get; set; }
        public string SoTienGhiCo { get; set; }
        public string MoTa { get; set; }
        public string NgayGiaoDich { get; set; }
        public string CD { get; set; }
    }

    public class Data
    {
        public List<ChiTietGiaoDich> ChiTietGiaoDich { get; set; }
    }

    public class RootObject
    {
        public bool status { get; set; }
        public Data data { get; set; }
    }
}
