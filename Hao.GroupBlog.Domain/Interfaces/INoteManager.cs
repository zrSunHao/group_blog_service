namespace Hao.GroupBlog.Domain.Interfaces
{
    public interface INoteManager
    {
        public Task<bool> IsOpen(string id);

        // 添加Note

        // 修改Note

        // 删除Note

        // 列表Note

        // 收藏Note

        // 取消收藏Note

        // 获取NoteContent

        // 保存NoteContent

        // 获取分享的NoteContent

        // 我的Note移动到指定column

        // 分享的Note移动到指定column
    }
}
