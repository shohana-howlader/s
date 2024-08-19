namespace Pactice15_8_24.Models.ViewModel
{
    public class AggregateEmployeeVM
    {
        public decimal MinValue {  get; set; }
        public decimal MaxValue { get; set; }
         public decimal SumValue { get; set; }
        public decimal AvgValue { get; set;}

        public List<GroupByViewModel>GroupByResult { get; set; }

        public List<EmployeeVM>Employees { get; set; }
    }
}
