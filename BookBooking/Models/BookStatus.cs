using System.ComponentModel;

namespace BookBooking.Models
{
    public enum BookStatus
    {
        // 貸し出し可能
        AvailableForLend,
        // 借りている
        Borrowed,
        // 予約中
        Reservation,
        // 予約されている
        Reserved,
        // 貸出禁止
        Restriction,
    }
}

