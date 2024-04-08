namespace JvDev
{
    public interface IVisitable
    {
        void Accept(IVisitor message);
    }
}