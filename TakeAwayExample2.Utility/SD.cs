using System;

namespace TakeAwayExample2.Utility
{
    public class SD
    {
        //These will determine the roles of the users
        public const string ManagerRole = "Manager";
        public const string FrontDeskRole = "Front Desk";
        public const string KitchenRole = "Kitchen";
        public const string CustomerRole = "Customer";

        public const string ShoppingCart = "ShoppingCart";

        public const string StatusSubmitted = "Submitted";
        public const string StatusInProcess = "Being Prepared";
        public const string StatusReady = "Ready for Pickup";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusRejected = "Rejected";
    }
}
