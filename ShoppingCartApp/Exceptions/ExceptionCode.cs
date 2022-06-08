namespace ShoppingCart.Exceptions
{
    public enum ExceptionCode
    {
        Unknown = 0,
        RecordNotFound = 1,
        ApplicationError = 8,
        InvalidRequest = 9,
        CannotUpdateRecord = 10,
        ProductQuantityExceeded = 11
    }
}
