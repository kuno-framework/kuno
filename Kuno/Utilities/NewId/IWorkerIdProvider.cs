namespace Kuno.Utilities.NewId
{
    internal interface IWorkerIdProvider
    {
        byte[] GetWorkerId(int index);
    }
}