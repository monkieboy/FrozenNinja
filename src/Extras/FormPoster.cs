using System;
using System.Globalization;
using System.Net;
using System.Text;

namespace Extras
{
	public class FormPoster : IFormPoster
	{
		public IFormResponse Post( Uri url, string formData )
		{
			this.Log().Debug.Message( "Post to {0}", url );
			this.Log().Trace.Message( "data: {0}", formData );
			var webRequest = (HttpWebRequest)WebRequest.Create( url );
			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.GetRequestStream().WriteAndClose( formData, new UTF8Encoding( false ) );
			webRequest.Timeout = 300000;

			var startTime = DateTime.UtcNow;
			var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
			var endTime = DateTime.UtcNow;

			this.Log().Debug.Message( "Received response with HTTP status code {0} ", httpWebResponse.StatusCode );

			var duration = endTime - startTime;
			if( duration > TimeSpan.FromSeconds( 30 ))
			{
				this.Log().Info.Message( "Request to {0} took longer than 30 seconds (actual was {1} seconds)", url, duration.TotalSeconds.ToString( CultureInfo.InvariantCulture ) );
			}

			return new HttpWebFormResponse( httpWebResponse );
		}


		public IFormDataResponse PostRequest( Uri url, IFormDataRequest formData )
		{
			return new FormDataResponse( Post( url, formData.AsRawFormat() ).RawResponse.ReadToEnd() );
		}
	}
}
