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