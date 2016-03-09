namespace SnapRegisters
{
    public interface IDiscount
    {
        double ChangeAmountTo( double amt );
        double Discount();
        bool IsFlat();
    }
}
