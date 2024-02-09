public class Trainer : Character
{
    private IPaddle _paddle;
    
    protected override void Awake()
    {
        base.Awake();
        _paddle = GetComponent<IPaddle>();
    }

    public override void Init(PlayerInfo info)
    {
        base.Init(info);
        _paddle.Init(info.teamSide);
    }
    
    public override void SetStart()
    {
        _paddle.SetStart();
        _paddle.Serve();
    }
}
