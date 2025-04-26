public class PlayDiceGame : CasinoGame
{
    private readonly int _numberOfDice;
    private readonly int _minValue;
    private readonly int _maxValue;
    private List<int> _dice;

    public PlayDiceGame(PlayerProfile player, int betAmount, int numberOfDice, int minValue, int maxValue)
        : base(player, betAmount)
    {
        _numberOfDice = numberOfDice;
        _minValue = minValue;
        _maxValue = maxValue;
    }

    private List<int> CreateDice()
    {
        var random = new Random();
        var dice = new List<int>();

        for (int i = 0; i < _numberOfDice; i++)
        {
            dice.Add(random.Next(_minValue, _maxValue + 1));
        }

        return dice;
    }

    public override void Play()
    {
        Console.WriteLine("Бросаем кости...");

        _dice = CreateDice();
        int playerSum = _dice.Sum();

        Console.WriteLine("Ваши кости:");
        _dice.ForEach(d => Console.Write(d + " "));
        Console.WriteLine($"\nСумма: {playerSum}\n");

        var computerDice = CreateDice();
        int computerSum = computerDice.Sum();

        Console.WriteLine("Кости компьютера:");
        computerDice.ForEach(d => Console.Write(d + " "));
        Console.WriteLine($"\nСумма: {computerSum}\n");

        if (playerSum > computerSum)
        {
            Console.WriteLine();
            Win();
        }
        else if (playerSum < computerSum)
        {
            Console.WriteLine();
            Lose();
        }
        else
        {
            Console.WriteLine("Ничья!");
            Draw();           
        }
    }
}