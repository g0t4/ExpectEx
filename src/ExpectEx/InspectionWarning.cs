namespace ExpectEx
{
	using System;

	public class InspectionWarning : Exception
	{
		public InspectionWarning(string message) : base(message)
		{
		}
	}
}