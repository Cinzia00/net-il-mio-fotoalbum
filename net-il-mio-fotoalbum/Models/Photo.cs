namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Visible { get; set; }

        public Photo() { }

        public Photo(string title, string description, string image, bool visible)
        {
            Title = title;
            Description = description;
            Image = image;
            Visible = visible;
        }

        public List<Category>? Categories { get; set; }



    }
}
