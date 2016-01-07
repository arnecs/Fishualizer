using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{

	public class Målinger
	{
	
		private Dictionary<String, Double> data;
		private DateTime date;

		public Målinger ()
		{
			data = new Dictionary<String, Double> ();
		}


		public void setDate(DateTime date) {
			this.date = date;
		}

		public DateTime getDate() {
			return date; 
		}

		public Double getValueForKey(String key) {
			double v;
			if (data.TryGetValue (key, out v)) {
				return v;
			}
			return -1;
		}
	}
}

