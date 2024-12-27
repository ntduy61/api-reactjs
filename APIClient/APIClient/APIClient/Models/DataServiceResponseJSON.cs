namespace APIClient.Models
{
    public class DataServiceResponseJSON
    {
        public virtual IList<IList<Dictionary<string, string>>> recordsets { get; set; }
        public Dictionary<string, string> output { get; set; }
        public IList<int> rowsAffected { get; set; }
        public int returnValue { get; set; }
    }

    public class DataServiceResponse : DataServiceResponseJSON
    {
        public IList<Dictionary<string, string>> recordsets { get; set; }
        public IList<IList<Dictionary<string, string>>> recordsetsList { get; set; }

        public string JSONString;
    }

}
