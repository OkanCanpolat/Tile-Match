using System.Collections.Generic;

public class TileCommandInvoker : Singleton<TileCommandInvoker>
{
    private List<ITileCommand> commands;

    private void Start()
    {
        commands = new List<ITileCommand>();
    }
    public void AddCommand(ITileCommand command)
    {
        commands.Add(command);
        command.Execute();
    }
    public void UndoCommand()
    {
        if (commands.Count > 0)
        {
            ITileCommand command = commands[commands.Count - 1];
            command.Undo();
            commands.Remove(command);
        }
    }
    public void RemoveCommand(Tile tile)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            if (commands[i].Tile == tile)
            {
                commands.Remove(commands[i]);
                return;
            }
        }
    }
}
