namespace EntityRelationsLab
{
    public class Pet
    {
        public int Id { get; set; }

        public string Type { get; set; }


        public int? ParentId { get; set; }

        public  virtual Person Parent { get; set; }

    }
}