
public interface ISellableItem
{
    public int Cost { get; }
    public void Buy(Player player);
}
