public class Card
{
    private Queue<Card> _deck;
    private readonly int _numberOfCards;
    public string Suit { get; }
    public string Rank { get; }
    public int Value { get; }

    public Card(string suit, string rank, int value)
    {
        Suit = suit;
        Rank = rank;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Rank} {Suit}";
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
}