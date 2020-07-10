namespace LinFx.Test.ObjectMapping
{
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public Teacher Teacher { get; set; }
    }

    public class PersonDto
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Teacher { get; set; }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
