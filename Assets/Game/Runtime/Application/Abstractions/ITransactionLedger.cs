namespace Game.Application.Abstractions
{
    public interface ITransactionLedger
    {
        void Append(string operationId, string actionId, string itemId, int currencyCost);
    }
}
