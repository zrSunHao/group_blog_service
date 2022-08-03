namespace Hao.GroupBlog.Domain.Interfaces
{
    public interface INoteManager
    {
        public Task<bool> IsOpen(string id);
    }
}
