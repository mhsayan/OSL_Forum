namespace OSL.Forum.NHibernateBase
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
