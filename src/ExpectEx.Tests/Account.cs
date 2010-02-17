namespace ExpectEx.Tests
{
	public class Account
	{
		private string _Name;

		public decimal Balance { get; protected set; }

		public void Deposit(decimal amount)
		{
			Balance += amount;
		}

		public void Deposits(decimal amount1, decimal amount2)
		{
			Balance += amount1 + amount2;
		}

		public void SetName(string name)
		{
			_Name = name;
		}

		public int AddNumbers(int a, int b)
		{
			return a + b;
		}

		public string GetName()
		{
			return _Name;
		}
	}
}