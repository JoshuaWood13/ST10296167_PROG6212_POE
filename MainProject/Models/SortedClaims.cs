﻿namespace ST10296167_PROG6212_POE.Models
{
    public class SortedClaims
    {
        public List<Claims> FullTime { get; set; }
        public List<Claims> FlaggedFT { get; set; }
        public List<Claims> PartTime { get; set; }
        public List<Claims> FlaggedPT { get; set; }

        public SortedClaims()
        {
            FullTime = new List<Claims>();
            FlaggedFT = new List<Claims>();
            PartTime = new List<Claims>();
            FlaggedPT = new List<Claims>();
        }
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//