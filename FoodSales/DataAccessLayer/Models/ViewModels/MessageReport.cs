using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.ViewModels
{
	public class MessageReport
	{
		public bool isSuccess { get; set; }

		public string Message { get; set; }

		public MessageReport()
		{

		}

		public MessageReport(bool isSuccess, string Message)
		{
			this.isSuccess = isSuccess;
			this.Message = Message;
		}
	}
}
