namespace LinFx
{
    public class NameValue<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }

    public class NameValue : NameValue<string>
    {
    }
}
