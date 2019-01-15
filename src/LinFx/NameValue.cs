namespace LinFx
{
    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// </summary>
    public class NameValue : NameValue<string>
    {
		public NameValue() { }

		public NameValue(string name, string value)
		{
			Name = name;
			Value = value;
		}
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

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// </summary>
        public NameValue() { }

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// </summary>
        public NameValue(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}
