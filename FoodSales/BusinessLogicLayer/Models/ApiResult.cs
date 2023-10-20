using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogicLayer.Models
{
	public class ApiResult<T>
	{
		public bool Status { get; set; }
		public object? Message { get; set; }
		public T? Data { get; set; }
		public int ErrorCode { get; }

		public ApiResult(T? data = default, int errCode = 0, bool status = false, object? errMsg = null)
		{
			Data = data;
			Message = errMsg;
			Status = status;
			ErrorCode = errCode;
		}
		public static ApiResult<T> ValidateModel(T data, object message = null)
		{
			return new ApiResult<T>(data, (int)HttpStatusCode.BadRequest,false, message);
		}
		public static ApiResult<T> Failure(string message = "")
		{
			return new ApiResult<T>(default, (int)HttpStatusCode.ExpectationFailed, false, string.IsNullOrEmpty(message) ? "ERROR!" : message);
		}
		public static ApiResult<T> ErrorInProcessing(string message = "", object data = null)
		{
			return new ApiResult<T>((T?)data, (int)HttpStatusCode.InternalServerError,false, string.IsNullOrEmpty(message) ? "Error in processing!" : message);
		}
		public static ApiResult<T> Successfully(T data, string message = "")
		{
			return new ApiResult<T>(data, (int)HttpStatusCode.OK, true, string.IsNullOrEmpty(message) ? "OK" : message);
		}
	}
}
