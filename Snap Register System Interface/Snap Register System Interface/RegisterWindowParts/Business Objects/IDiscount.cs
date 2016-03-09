namespace SnapRegisters
{
    //Interface currently implimented by Item and Coupon.
    //Allows both Sales and Coupons to be in the same List contiained in DiscountList
    public interface IDiscount
    {
        double ChangeAmountTo( double amt );
        double Discount();
        bool IsFlat();
    }
}
