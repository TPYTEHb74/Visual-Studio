public class PlayerProfile
{
    public string Name { get; set; }
    public int Balance { get; private set; }
    public int GamesPlayed { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    private const int MaxBalance = 2000;

    public event Action<int> OnBalanceChanged;
    public event Action OnWin;
    public event Action OnLose;
    public event Action OnDraw;
    public event Action<string> OnCasinoBankrupt;

    public PlayerProfile(string name, int initialBalance)
    {
        Name = name;
        Balance = Math.Min(initialBalance, MaxBalance);

    }

    public void AddBalance(int amount)
    {
        int newBalance = Balance + amount;

        if (newBalance > MaxBalance)
        {
            int excess = newBalance - MaxBalance;
            Balance = MaxBalance;
            OnBalanceChanged?.Invoke(Balance);
            OnCasinoBankrupt?.Invoke($"Вы разорили казино!" +
                $"\nВаш выигрыш превысил максимально возможный банк.\nВы получаете {MaxBalance}," +
                $" а остальные {excess} будут использованы для постройки нового казино!");
        }
        else
        {
            Balance = newBalance;
            OnBalanceChanged?.Invoke(Balance);
        }
    }

    public bool TryBet(int amount)
    {
        if (Balance <= 0)
        {
            Console.WriteLine("Нет денег? Уходи!");
            return false;
        }
        if (Balance >= amount)
        {
            Balance -= amount;
            OnBalanceChanged?.Invoke(Balance);
            return true;
        }

        return false;
    }

 

    public override string ToString()
    {
        return $"Игрок: {Name}\nБаланс: {Balance}";
    }
}