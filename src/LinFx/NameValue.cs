namespace LinFx
{
    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// </summary>
    public class NameValue : NameValue<string>
    {
    }

    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NameValue<T>
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public T Value { get; set; }
    }
}
