using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.DTOS
{
    public class EmployeePerformancesdtoforlist
    {
        
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; } // Employee Name from Users table
            public int Score { get; set; }
           
        

    }
}