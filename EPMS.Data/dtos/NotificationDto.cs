using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.DTOS
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }

}