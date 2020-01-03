namespace ProductCatalogApi.Domain
{
    public class FilterCriteria
    {
        public string Operator { get; set; }
        public string Type { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public object ValueTo { get; set; }

        public FilterCriteria Left { get; set; }
        public FilterCriteria Right { get; set; }
    }
}
