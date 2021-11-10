namespace LinFx.Data
{
    public interface ISort
    {
        string PropertyName { get; set; }
        bool Ascending { get; set; }
    }
}
