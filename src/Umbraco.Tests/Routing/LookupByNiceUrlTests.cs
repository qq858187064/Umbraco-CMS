using System.Configuration;
using NUnit.Framework;
using Umbraco.Web.Routing;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.template;

namespace Umbraco.Tests.Routing
{
	[TestFixture]
	public class LookupByNiceUrlTests : BaseRoutingTest
	{

		/// <summary>
		/// We don't need a db for this test, will run faster without one
		/// </summary>
		protected override bool RequiresDbSetup
		{
			get { return false; }
		}

		[TestCase("/")]
		[TestCase("/default.aspx")] //this one is actually rather important since this is the path that comes through when we are running in pre-IIS 7 for the root document '/' !
		[TestCase("/Sub1")]
		[TestCase("/sub1")]
		[TestCase("/sub1.aspx")]
		public void Match_Document_By_Url_Hide_Top_Level(string urlAsString)
		{
			var routingContext = GetRoutingContext(urlAsString);
			var url = routingContext.UmbracoContext.UmbracoUrl; //very important to use the cleaned up umbraco url
			var docRequest = new DocumentRequest(url, routingContext);
			var lookup = new LookupByNiceUrl();
			ConfigurationManager.AppSettings.Set("umbracoHideTopLevelNodeFromPath", "true");

			var result = lookup.TrySetDocument(docRequest);

			Assert.IsTrue(result);
		}

		[TestCase("/")]
		[TestCase("/default.aspx")] //this one is actually rather important since this is the path that comes through when we are running in pre-IIS 7 for the root document '/' !
		[TestCase("/home/Sub1")]
		[TestCase("/Home/Sub1")] //different cases
		[TestCase("/home/Sub1.aspx")]
		public void Match_Document_By_Url(string urlAsString)
		{
			var routingContext = GetRoutingContext(urlAsString);
			var url = routingContext.UmbracoContext.UmbracoUrl;	//very important to use the cleaned up umbraco url		
			var docRequest = new DocumentRequest(url, routingContext);			
			var lookup = new LookupByNiceUrl();

			var result = lookup.TrySetDocument(docRequest);

			Assert.IsTrue(result);
		}

	}
}