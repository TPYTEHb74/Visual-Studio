public class Casino
{
    private readonly PlayerProfile _player;
    private readonly ISaveLoadService<string> _saveLoadService;

    public Casino(PlayerProfile player, ISaveLoadService<string> saveLoadService)
    {
        _player = player;
        _saveLoadService = saveLoadService;

        _player.OnCasinoBankrupt += message =>
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        };
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(_player);
            Console.WriteLine("\nДобро пожаловать в казино!");
            Console.WriteLine("1. Играть в Блэкджек");
            Console.WriteLine("2. Играть в кости");
            Console.WriteLine("3. Сохранить профиль");
            Console.WriteLine("4. Выйти");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    PlayBlackjack();
                    break;
                case "2":
                    PlayDice();
                    break;
                case "3":
                    SaveProfile();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    private void PlayBlackjack()
    {
        Console.Write("\nВведите ставку: ");
        if (!int.TryParse(Console.ReadLine(), out int bet) || bet <= 0)
        {
            Console.WriteLine("Неверная ставка!");
            return;
        }

        if (!_player.TryBet(bet))
        {
            Console.WriteLine("Недостаточно средств!");
            return;
        }


        var game = new BlackjackGame(_player, bet, 36);
        game.OnWin += () => Console.WriteLine($"Поздравляем! Вы выиграли {bet * 2}!");
        game.OnLose += () => Console.WriteLine("Увы, вы проиграли.");
        game.OnDraw += () => Console.WriteLine("Ничья! Ваша ставка возвращена.");

        game.Play();
    }

    private void PlayDice()
    {
        Console.Write("\nВведите ставку: ");
        if (!int.TryParse(Console.ReadLine(), out int bet) || bet <= 0)
        {
            Console.WriteLine("Неверная ставка!");
            return;
        }

        if (!_player.TryBet(bet))
        {
            Console.WriteLine("Недостаточно средств!");
            return;
        }

        Console.Write("Введите количество костей: ");
        if (!int.TryParse(Console.ReadLine(), out int diceCount) || diceCount <= 0)
        {
            Console.WriteLine("Неверное количество костей!");
            return;
        }

        var game = new DiceGame(_player, bet, diceCount, 1, 6);
        game.OnWin += () => Console.WriteLine($"Поздравляем! Вы выиграли {bet * 2}!");
        game.OnLose += () => Console.WriteLine("Увы, вы проиграли.");
        game.OnDraw += () => Console.WriteLine("Ничья! Ваша ставка возвращена.");

        game.Play();
    }

    private void SaveProfile()
    {
        _saveLoadService.Save(_player.ToString(), _player.Name);
        Console.WriteLine("Профиль сохранен!");
    }
}
public abstract class CasinoGame
{
    protected readonly PlayerProfile _player;
    protected readonly int _betAmount;

    public event Action OnWin;
    public event Action OnLose;
    public event Action OnDraw;

    protected CasinoGame(PlayerProfile player, int betAmount)
    {
        _player = player;
        _betAmount = betAmount;
    }

    protected void Win(int multiplier = 2)
    {
        _player.RecordWin(_betAmount * multiplier);
        OnWin?.Invoke();
    }

    protected void Lose()
    {
        _player.RecordLoss();
        OnLose?.Invoke();
    }

    protected void Draw()
    {
        _player.RecordDraw();
        OnDraw?.Invoke();
    }

    public abstract void Play();
}
