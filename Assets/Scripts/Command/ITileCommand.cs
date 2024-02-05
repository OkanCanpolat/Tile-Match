

public interface ITileCommand 
{
    public Tile Tile { get; set; }
    public void Execute();
    public void Undo();
}
