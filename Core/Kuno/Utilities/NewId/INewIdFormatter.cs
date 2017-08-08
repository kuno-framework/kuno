namespace Kuno.Utilities.NewId
{
    internal interface INewIdFormatter
    {
        string Format(byte[] bytes);
    }
}