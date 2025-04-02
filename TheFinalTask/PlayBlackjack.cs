public class BlackjackGame : CasinoGame
{
    private readonly int _numberOfCards;
    private Queue<Card> _deck;
    private List<Card> _playerCards;
    private List<Card> _dealerCards;

    public BlackjackGame(PlayerProfile player, int betAmount, int numberOfCards)
        : base(player, betAmount)
    {
        _numberOfCards = numberOfCards;
    }

    private List<Card> CreateCards()
    {
        var cards = new List<Card>();
        string[] suits = { "♥", "♦", "♣", "♠" };
        string[] ranks = { "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        int[] values = { 6, 7, 8, 9, 10, 10, 10, 10, 11 };
       
        for (int i = 0; i < _numberOfCards / 36 + 1; i++)
        {
            for (int s = 0; s < suits.Length; s++)
            {
                for (int r = 0; r < ranks.Length; r++)
                {
                    cards.Add(new Card(suits[s], ranks[r], values[r]));
                }
            }
        }

        return cards.Take(_numberOfCards).ToList();
    }

    private void Shuffle(List<Card> cards)
    {
        var random = new Random();
        _deck = new Queue<Card>();

        while (cards.Count > 0)
        {
            int index = random.Next(cards.Count);
            _deck.Enqueue(cards[index]);
            cards.RemoveAt(index);
        }
    }

    private int CalculateScore(List<Card> hand)
    {
        int score = hand.Sum(card => card.Value);
        int aceCount = hand.Count(card => card.Rank == "A");

        while (score > 21 && aceCount > 0)
        {
            score -= 10;
            aceCount--;
        }

        return score;
    }

    public override void Play()
    {
        var cards = CreateCards();
        Shuffle(cards);

        _playerCards = new List<Card> { _deck.Dequeue(), _deck.Dequeue() };
        _dealerCards = new List<Card> { _deck.Dequeue(), _deck.Dequeue() };

        int playerScore = CalculateScore(_playerCards);
        int dealerScore = CalculateScore(_dealerCards);

        Console.WriteLine("Ваши карты:");
        _playerCards.ForEach(c => Console.WriteLine(c));
        Console.WriteLine($"Сумма очков: {playerScore}");
        Console.WriteLine();

        Console.WriteLine("Карты дилера:");
        Console.WriteLine(_dealerCards[0]);
        Console.WriteLine("[скрытая карта]");
        Console.WriteLine();

        if (playerScore == 21 && dealerScore == 21)
        {
            Console.WriteLine("У обоих блэкджек! Ничья.");
            Draw();
            return;
        }
        else if (playerScore == 21)
        {
            Console.WriteLine("Блэкджек! Вы выиграли!");
            Win(3); 
            return;
        }
        else if (dealerScore == 21)
        {
            Console.WriteLine("У дилера блэкджек! Вы проиграли.");
            Lose();
            return;
        }

        while (playerScore < 21)
        {
            Console.Write("Хотите взять еще карту? (Yes/No): ");
            string input = Console.ReadLine().ToLower();

            if (input == "Yes")
            {
                _playerCards.Add(_deck.Dequeue());
                playerScore = CalculateScore(_playerCards);

                Console.WriteLine("\nВаши карты:");
                _playerCards.ForEach(c => Console.WriteLine(c));
                Console.WriteLine($"Сумма очков: {playerScore}\n");

                if (playerScore > 21)
                {
                    Console.WriteLine("Перебор! Вы проиграли.");
                    Lose();
                    return;
                }
            }
            else
            {
                break;
            }
        }

        while (dealerScore < 17)
        {
            _dealerCards.Add(_deck.Dequeue());
            dealerScore = CalculateScore(_dealerCards);
        }

        Console.WriteLine("\nКарты дилера:");
        _dealerCards.ForEach(c => Console.WriteLine(c));
        Console.WriteLine($"Сумма очков дилера: {dealerScore}\n");

        if (playerScore > 21)
        {
            Console.WriteLine("Вы проиграли - перебор!");
            Lose();
        }
        else if (dealerScore > 21)
        {
            Console.WriteLine("Дилер проиграл - перебор! Вы выиграли!");
            Win();
        }
        else if (playerScore > dealerScore)
        {
            Console.WriteLine();
            Win();
        }
        else if (playerScore < dealerScore)
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