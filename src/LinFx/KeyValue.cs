namespace LinFx
{
    public class KeyValue<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }

    public class KeyValue : KeyValue<string>
    {
    }
}
