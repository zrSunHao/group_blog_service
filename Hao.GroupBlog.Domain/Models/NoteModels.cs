namespace Hao.GroupBlog.Domain.Models
{
    public class NoteM
    {
        public long? Id { get; set; }

        public string? ContentId { get; set; }

        public string Name { get; set; }

        public string Keys { get; set; }

        public int Hits { get; set; } = 0;

        public bool Opened { get; set; } = false;

        public string ProfileId { get; set; }

        public string Intro { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }

    public class NoteContentM
    {
        public string Id { get; set; }

        public string Content { get; set; }
    }
}
