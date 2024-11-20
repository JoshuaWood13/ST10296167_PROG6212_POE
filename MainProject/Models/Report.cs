namespace ST10296167_PROG6212_POE.Models
{
    public class Report
    {
        public List<Claims> FilteredClaims { get; set; }
        public double TotalClaimsValue { get; set; }
        public double TotalClaimsCount { get; set; }
        public double TotalHoursWorked { get; set; }
        public double AverageClaimAmount { get; set; }
        public double AverageHoursWorked { get; set; }
        public double AverageHourlyRate { get; set; }

    }
}
