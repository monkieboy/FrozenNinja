using System;

namespace Extras
{
	public interface IFormPoster
	{
		IFormResponse Post( Uri url, string formData );
		IFormDataResponse PostRequest( Uri url, IFormDataRequest formData );
	}
}
