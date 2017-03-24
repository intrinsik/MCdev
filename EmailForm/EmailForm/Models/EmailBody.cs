using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;


namespace EmailForm.Models
{
	public class EmailBody
	{

		private string username = HttpContext.Current.Request.ServerVariables["AUTH_USER"];
		private DateTime date = DateTime.Now;

		DirectorySearcher adSearch = new DirectorySearcher();
		SearchResult adSearchResult = adSearch.PropertiesToLoad.Add("sn");
		//SearchResult adSearchResult = adSearch.PropertiesToLoad.Add("sn");
		//adSearchResult = adSearch.PropertiesToLoad.Add("givenName");
		//adSearchResult = adSearch.PropertiesToLoad.Add("mail");
		//adSearchResult = adSearch.PropertiesToLoad.Add("telephoneNumber");


		public DateTime CurrentDate
		{
			get{ return date; }
			//set{ date = value; }
		}

		public string eeName
		{
			get{ return username; }
			//set{ username = value; }
		}
	}
}